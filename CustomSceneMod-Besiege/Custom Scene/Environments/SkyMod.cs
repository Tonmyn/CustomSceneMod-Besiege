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
        public override string PropertisePath { get { return Path + @"\SkyPropertise.xml"; } }

        private GameObject SkyObject;
        private GameObject orginSkySphere;
    
        public override SkyPropertise Propertise { get; set; }
        public override bool Data { get; set; }
        public override bool Enabled { get; protected set; }
        public ResourceLoader resourceLoader { get; } = ResourceLoader.Instance;



        public SkyMod(string path,bool data)
        {
            Path = path + @"\Sky";
            Data = data;
            //PropertisePath = Path + @"\SkyPropertise.xml";

            try
            {
                if (isExist)
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

            SkyObject = new GameObject("Sky Object");
            SkyObject.transform.SetParent(parent);

            SkyObject.AddComponent<CameraFollower>();

            var go = new GameObject("");
            resourceLoader.LoadVirtualObject(go, Propertise, Path, Data, SkyObject.transform, processResource);
            CurrentWorkNumber++;


            orginSkySphere = GameObject.Find("STAR SPHERE");
            if (orginSkySphere != null)
            {
                orginSkySphere.SetActive(false);
            }
        }

        private void processResource(GameObject gameObject,SkyPropertise propertise)
        { 
        
        }

        public override void Clear()
        {
            if (orginSkySphere != null) orginSkySphere.SetActive(true);

            if (SkyObject == null) return;
            UnityEngine.Object.Destroy(SkyObject);
        }   
    }

    public class SkyPropertise : MeshPropertise,IEnvironmentPropertise
    {
        [CanBeEmpty]
        public Vector3 Axis { get; set; } = Vector3.zero;
        [CanBeEmpty]
        public float AngularVelocity { get; set; } = 0f;
    }

    public class CameraFollower : MonoBehaviour
    {
        Camera mainCamera;

        void Start()
        {
            mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        }

        void Update()
        {
            this.transform.position = mainCamera.transform.position;
            this.transform.localScale = Vector3.one * mainCamera.farClipPlane / /*42*/ 10;
        }

    }
}
