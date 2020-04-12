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
        public override bool Data { get; set; }
        public override ScenePropertise Propertise { get; set; }
        public override bool Enabled { get; protected set; } = false;
     

        #region Environment
        public TerrainMod  TerrainMod;
        //public SkyMod SkyMod;
        #endregion

        public GameObject SceneObject;

        public Scene(string path,bool data = false)
        {
            Path = path;
            Data = data;
            var propertisePath = Path + @"\ScenePropertise.xml";
            try
            {
                if (ModIO.ExistsFile(propertisePath, Data))
                {
                    Propertise = ModIO.DeserializeXml<ScenePropertise>(propertisePath, Data);
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
                TerrainMod = new TerrainMod(Path,data);
            }
        }


        //internal override void Read()
        //{
        //    TerrainMod = new TerrainMod(Path);
        //    //SkyMod.Read();
        //}

        public override void Load(Transform transform)
        {
            if (!Enabled) return;

            SceneObject = new GameObject("Scene Object");
            SceneObject.transform.SetParent(transform);

            TerrainMod.Load(SceneObject.transform);
            //SkyMod.Load();
        }

        public override void Clear()
        {
            TerrainMod.Clear();
            //SkyMod.Clear();
        }
    }

    public class ScenePropertise : EnvironmentPropertise
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
