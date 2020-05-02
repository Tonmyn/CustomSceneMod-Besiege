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
    public class SceneMod : EnvironmentMod<ScenePropertise>
    {
        public override string Path { get; }
        public override string PropertisePath { get { return Path + "ScenePropertise.xml"; } }
        public override bool Data { get; set; }
        public override ScenePropertise Propertise { get; set; }
        public override bool Enabled { get; protected set; } = false;

        #region Environment
        public TerrainMod  TerrainMod;
        public SkyMod SkyMod;
        public CloudMod CloudMod;
        #endregion

        public GameObject SceneObject;

        public SceneMod(string path,bool data = false)
        {
            Path = path + @"\";
            Data = data;
            //PropertisePath = Path + @"\ScenePropertise.xml";
            try
            {
                if (isExistPropertiseFile)
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
                TerrainMod = new TerrainMod(Path,Data);
                SkyMod = new SkyMod(Path, Data);
                CloudMod = new CloudMod(Path, Data);
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
            CloudMod.Load(SceneObject.transform);
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
            CloudMod.Clear();
            #endregion

            if (SceneObject == null) return;
            UnityEngine.Object.Destroy(SceneObject);
        }
    }

    public class ScenePropertise : TransformPropertise, IEnvironmentPropertise
    {
        [CanBeEmpty]
        public string Name { get; set; } = "Scene's Name";
        [CanBeEmpty]
        public string Author { get; private set; } = "Scene's Author";
        [CanBeEmpty]
        public string Description { get; private set; } = "Scene's Description";
    }
}
