using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Modding.Levels;
using Modding.Serialization;
using Vector3 = UnityEngine.Vector3;
using Modding;
using System.Threading;

namespace CustomScene
{
    public class CloudMod : EnvironmentMod<CloudPropertise>
    {
        public override string Path { get; }
        public override bool Data { get; set; }
        public override CloudPropertise Propertise { set; get; }
        public override string PropertisePath { get { return Path + "CloudPropertise.xml"; } }
        public override bool Enabled { get; protected set; } = false;

        private GameObject cloudObject;
        private List<GameObject> cloudUnitObjects;

        public CloudMod(string path, bool data)
        {
            Path = path + @"Cloud\";
            Data = data;
            //PropertisePath = Path + @"\SkyPropertise.xml";

            try
            {
                if (isExistPropertiseFile)
                {
                    Propertise = ModIO.DeserializeXml<CloudPropertise>(PropertisePath, Data);
                    Enabled = true;
                }
                else
                {
                    Propertise = new CloudPropertise();
                    ModIO.CreateDirectory(Path, Data);
                    ModIO.SerializeXml(Propertise, PropertisePath, Data);
                    Enabled = true;
                }
            }
            catch (Exception e)
            {
                Propertise = new CloudPropertise();
                Debug.Log("Cloud Propertise File Format is wrong...");
                Debug.Log(e.Message);
            }

            if (Enabled)
            {
                TotalWorkNumber = Propertise.Size;



            }
        }

        public override void Load(Transform parent)
        {
            if (!Enabled || TotalWorkNumber <= 0) return;

            cloudObject = new GameObject("Cloud Object");
            cloudObject.transform.SetParent(parent);

            cloudUnitObjects = new List<GameObject>();
            for (int i = 0; i < Propertise.Size; i++)
            {
                cloudUnitObjects.Add(CreateCloudObject());
                CurrentWorkNumber++;
                Thread.Sleep((int)Time.deltaTime * 200);
            }

            GameObject CreateCloudObject()
            {
                var position = Propertise.Position;
                var scale = Propertise.Scale;
                var color = Propertise.Color;
                var unitScale = Propertise.CloudUnitScale;
                var unitSizeBounds = Propertise.CloudUnitRandomSizeBounds;


                var randomPosition = new Vector3(
                               UnityEngine.Random.Range(-scale.x + position.x, scale.x + position.x),
                               UnityEngine.Random.Range(position.y, scale.y + position.y),
                               UnityEngine.Random.Range(-scale.z + position.z, scale.z + position.z));

                GameObject go = (GameObject)UnityEngine.Object.Instantiate(getCloudTemp(), randomPosition, Quaternion.identity, cloudObject.transform);
                go.transform.localScale = unitScale;
                go.name = "Cloud Unit Object";
                go.SetActive(true);

                ParticleSystem ps = go.GetComponent<ParticleSystem>();
                ps.startColor = color;

                var cs = go.AddComponent<CloudScript>();
                cs.SizeRandomBounds = unitSizeBounds;

                return go;

                GameObject getCloudTemp()
                {
                    var lp = UnityEngine.Object.Instantiate(PrefabMaster.GetPrefab(StatMaster.Category.Weather, 2).transform.FindChild("CLOUD").GetChild(0).gameObject);
                    lp.SetActive(false);
                    return lp;
                }
            }
        }
        public override void Clear()
        {
            if (cloudObject == null) return;
            UnityEngine.Object.Destroy(cloudObject);
        }

    }

    public class CloudPropertise : TransformPropertise, IEnvironmentPropertise
    {
        [CanBeEmpty]
        public int Size { get; set; } = 500;
        [CanBeEmpty]
        public Vector3 CloudUnitScale { get; set; } = Vector3.one * 30f;
        [CanBeEmpty]
        public Vector2 CloudUnitRandomSizeBounds{ get; set; } = new Vector2(20, 50);
        [CanBeEmpty]
        public Color Color { get; set; } = new Color(0.92f, 0.92f, 0.92f, 0.5f);

        public CloudPropertise()
        {
            Position = new Vector3(0, 200f, 0);
            Scale = new Vector3(1000f, 200f, 1000f);
        }
    }

    public class CloudScript : MonoBehaviour
    {
        public int timeBound = 120;
        public Vector2 SizeRandomBounds = Vector2.one * 10f;

        int time = 0;
        Vector3 axis = new Vector3(UnityEngine.Random.Range(-0.1f, 0.1f), 1f, UnityEngine.Random.Range(-0.1f, 0.1f));

        private void FixedUpdate()
        {

            transform.RotateAround(transform.localPosition, axis, Time.deltaTime * 0.3f);
            if (time++ == timeBound)
            {
                GetComponent<ParticleSystem>().startSize = UnityEngine.Random.Range(SizeRandomBounds.x, SizeRandomBounds.y);
            }

            if (time > timeBound)
            {
                time = 0;
            }

        }

    }

}
