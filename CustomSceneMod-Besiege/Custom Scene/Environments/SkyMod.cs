using Modding;
using Modding.Serialization;
using System;
using System.Collections.Generic;
//using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;

namespace CustomScene
{
    public class SkyMod : EnvironmentMod<SkyPropertise>,IResourceLoader
    {
        public override string Path { get; }
        public override bool Data { get; set; }
        public override bool Enabled { get; protected set; } = false;
        public override SkyPropertise Propertise { get; set; }
        public override string PropertisePath { get { return Path + "SkyPropertise.xml"; } }
        public ResourceLoader resourceLoader { get; } = ResourceLoader.Instance;


        private GameObject skyObject;
        private GameObject orginSkySphere;

        public SkyMod(string path,bool data)
        {
            Path = path + @"Sky\";
            Data = data;
            //PropertisePath = Path + @"\SkyPropertise.xml";

            try
            {
                if (isExistPropertiseFile)
                {
                    Propertise = ModIO.DeserializeXml<SkyPropertise>(PropertisePath, Data);
                    Enabled = true;
                }
                else
                {
                    Propertise = new SkyPropertise();
                    ModIO.CreateDirectory(Path, Data);
                    ModIO.SerializeXml(Propertise, PropertisePath, Data);
                    Enabled = true;
                }
            }
            catch (Exception e)
            {
                Propertise = new SkyPropertise();
                Debug.Log("Sky Propertise File Format is wrong...");
                Debug.Log(e.Message);
            }

            if (Enabled)
            {
                TotalWorkNumber = 1;
            }
        }
        public override void Load(Transform parent)
        {
            if (!Enabled || TotalWorkNumber <= 0) return;

            skyObject = new GameObject("Sky Object");
            skyObject.transform.SetParent(parent);


            var msf = skyObject.AddComponent<mySmoothFollow>();
            msf.target = Camera.main.transform;
            msf.smoothAmount = 1000000f;

            var r = skyObject.AddComponent<Rotate>();
            r.degreesPerSecond = Propertise.AngularVelocity;
            r.rotateAxis = Propertise.Axis;
            r.unscaledTime = true;
            r.worldSpace = true;
            r.sineSpeed = 0;
            r.sine = false;

            var go = new GameObject("");
            go.transform.localScale = Propertise.Scale;
            resourceLoader.LoadVirtualObject(go, Propertise, Path, Data, skyObject.transform, processResource);
            
            CurrentWorkNumber++;

            orginSkySphere = GameObject.Find("STAR SPHERE");
            if (orginSkySphere != null)
            {
                orginSkySphere.SetActive(false);
            }
        }
        public override void Clear()
        {
            if (orginSkySphere != null) orginSkySphere.SetActive(true);

            if (skyObject == null) return;
            UnityEngine.Object.Destroy(skyObject);
        }
        private void processResource(GameObject gameObject,SkyPropertise propertise)
        { 
        
        }


    }

    public class SkyPropertise : MeshPropertise, IEnvironmentPropertise
    {
        [CanBeEmpty]
        public Vector3 Axis { get; set; } = Vector3.zero;
        [CanBeEmpty]
        public float AngularVelocity { get; set; } = 0f;

        public SkyPropertise()
        {
            Scale = Vector3.one * 100f;
        }
    }
}
