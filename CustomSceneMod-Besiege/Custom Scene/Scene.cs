using Modding.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Modding;
using Vector3 = Modding.Serialization.Vector3;

namespace CustomScene
{
    public class Scene : EnvironmentMod<ScenePropertise>
    {
        public override string Path { get; }
        public override string PropertisePath { get { return Path + @"\ScenePropertise.xml"; } }
        public override bool Data { get; set; }
        public override ScenePropertise Propertise { get; set; }
        public override bool Enabled { get; protected set; } = false;

        #region Environment
        public TerrainMod  TerrainMod;
        public SkyMod SkyMod;
        #endregion

        public GameObject SceneObject;

        public Scene(string path,bool data = false)
        {
            Path = path;
            Data = data;
            //PropertisePath = Path + @"\ScenePropertise.xml";
            try
            {
                if (isExist)
                {
                    Propertise = ModIO.DeserializeXml<ScenePropertise>(PropertisePath, Data);
                    Enabled = true;
                }
                else
                {
                    Propertise = new ScenePropertise();
                    ModIO.CreateDirectory(Path, Data);
                    ModIO.SerializeXml(Propertise, PropertisePath, Data);
                    Enabled = true;
                }
            }
            catch(Exception e)
            {
                Propertise = new ScenePropertise();
                Debug.Log("Scene Propertise File Format is wrong...");
                Debug.Log(e.Message);
            }

            if (Enabled)
            {

                #region Environment
                TerrainMod = new TerrainMod(Path,data);
                SkyMod = new SkyMod(Path, data);

                #endregion
            }
        }

        public override void Load(Transform transform)
        {
            if (!Enabled) return;

            SceneObject = new GameObject(string.Format("Scene -{0}-", Propertise.Name));
            SceneObject.transform.SetParent(transform);

            #region Environment
            TerrainMod.Load(SceneObject.transform);
            SkyMod.Load(SceneObject.transform);
            #endregion

            SceneObject.transform.position = Propertise.Position;
            SceneObject.transform.rotation = Quaternion.Euler(Propertise.Rotation);
            SceneObject.transform.localScale = Propertise.Scale;
        }
        public override void Clear()
        {
            #region Environment
            TerrainMod.Clear();
            SkyMod.Clear();
            #endregion

            if (SceneObject == null) return;
            UnityEngine.Object.Destroy(SceneObject);
        }

        //public  override void Create(string name,bool data = false)
        //{
        //    var path = @"Scenes\" + name;
        //    if (!ModIO.ExistsDirectory(path, data))
        //    {
        //        ModIO.CreateDirectory(path, data);
        //        ModIO.SerializeXml(new ScenePropertise() { Name = name }, PropertisePath, data);

        //        #region Environment
        //        TerrainMod.Create(path, data);
        //        SkyMod.Create(path, data);
        //        //ModIO.CreateDirectory(path + @"\Terrain", data);
        //        //ModIO.SerializeXml(new TerrainPropertise(), path + @"\Terrain\TerrainPropertise.xml", data);

        //        //ModIO.CreateDirectory(path + @"\Sky", data);
        //        //ModIO.SerializeXml(new SkyPropertise(), path + @"\Sky\SkyPropertise.xml", data);
        //        #endregion
        //        Debug.Log("Create Scene is success");
        //    }
        //    else
        //    {
        //        Debug.Log("Scene is existed...");
        //    }
        //}
    }

    public class ScenePropertise : Element, IEnvironmentPropertise
    {
        [CanBeEmpty]
        public string Name { get; set; } = "Scene's Name";
        [CanBeEmpty]
        public string Author { get; private set; } = "Scene's Author";
        [CanBeEmpty]
        public string Description { get; private set; } = "Scene's Description";
        [CanBeEmpty]
        public Vector3 Position { get; set; } = Vector3.zero;
        [CanBeEmpty]
        public Vector3 Rotation { get; set; } = Vector3.zero;
        [CanBeEmpty]
        public Vector3 Scale { get; set; } = Vector3.one;
    }
}
