using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Modding.Levels;

namespace CustomScene
{
    public class CloudMod : Environment<CloudPropertise>
    {
        void Start()
        {          
            //new CommandRegistration("VP_CloudSize", (string[] args) =>
            //{
            //    if (args.Length < 1)
            //    {
            //        GeoTools.Log("ERROR!");
            //    }
            //    try
            //    {
            //        int cloudSize = int.Parse(args[0]);
            //        if (cloudSize < 0 || cloudSize > 3000) { GeoTools.Log("Your cloud amount is not available. "); }
            //        else
            //        {
            //            this.cloudSize = cloudSize;
            //            LoadCloud();
            //        }
            //    }
            //    catch
            //    {
            //        GeoTools.Log("Could not parse " + args[0] + "to cloud amount");
            //    }
            //    GeoTools.Log("There will be " + cloudSize.ToString() + " clouds" + "\n");
            //}, "Set CloudSize.No bigger than 80 and no less than 10.");

        }

        
        List<GameObject> cloudObjects;  
        CloudsPropertise cloudsPropertise;

        public override CloudPropertise Propertise => throw new NotImplementedException();

        class CloudsPropertise
        {
            public int cloudsSize = 0;
            public Vector3 cloudsPosition = Vector3.zero;
            public Vector3 cloudsScale = new Vector3(1000, 200, 1000);
            public Color cloudsColor = new Color(0.92f, 0.92f, 0.92f, 0.5f);
        }

        public override void ReadEnvironment(SceneFolder scenePack)
        {
            ClearEnvironment();

            try
            {
                foreach (var str in scenePack.SettingFileDatas)
                {
                    string[] chara = str.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    if (chara.Length > 2)
                    {
                        if (chara[0].ToLower() == "cloud" || chara[0].ToLower() == "clouds")
                        {

                            if (chara[1] == "size")
                            {
                                int size = Mathf.Clamp(Convert.ToInt32(chara[2]), 0, 1000);
                                
                                if (size <= 0)
                                {
                                    break;
                                }
                                cloudsPropertise = new CloudsPropertise();
                                cloudsPropertise.cloudsSize = size;
                            }
                            else if (chara[1].ToLower() == "floorscale" || chara[1].ToLower() == "cloudscale" || chara[1].ToLower() == "scale")
                            {
                                cloudsPropertise.cloudsScale = new Vector3(
                                Convert.ToSingle(chara[2]),
                                Convert.ToSingle(chara[3]),
                                Convert.ToSingle(chara[4]));
                            }
                            else if (chara[1].ToLower() == "location")
                            {
                                cloudsPropertise.cloudsPosition = new Vector3(
                                Convert.ToSingle(chara[2]),
                                Convert.ToSingle(chara[3]),
                                Convert.ToSingle(chara[4]));
                            }
                            else if (chara[1].ToLower() == "color")
                            {
                                cloudsPropertise.cloudsColor = new Color(
                                Convert.ToSingle(chara[2]),
                                Convert.ToSingle(chara[3]),
                                Convert.ToSingle(chara[4]),
                                Convert.ToSingle(chara[5]));
                            }
                        }
                    }

                }
#if DEBUG
                GeoTools.Log("Read Cloud Completed!");
#endif
            }

            catch (Exception ex)
            {
                GeoTools.Log("Error! Read Cloud Failed!");
                GeoTools.Log(ex.Message);
                ClearEnvironment();
                return;
            }
        }
        public override void LoadEnvironment()
        {
            try
            {
                if (Mod.prop.CloudTemp == null) return;
                if (cloudsPropertise == null) return;

                int size = cloudsPropertise.cloudsSize;
                if (size <= 0)
                {
                    return;
                }
                else
                {
                    cloudObjects = new List<GameObject>();

                    for (int i = 0; i < size; i++)
                    {
                        cloudObjects.Add(CreateCloudObject(cloudsPropertise));
                    }
#if DEBUG
                    GeoTools.Log("Load Cloud Successfully");
#endif
                }
            }
            catch (Exception ex)
            {
                GeoTools.Log("Error! Load Cloud Failed");
                GeoTools.Log(ex.Message);
                ClearEnvironment();
            }
        }
        public override void ClearEnvironment()
        {
            if (cloudObjects == null) return;
            if (cloudObjects.Count <= 0) return;
#if DEBUG
            GeoTools.Log("Clear Cloud");
#endif
            for (int i = 0; i < cloudObjects.Count; i++)
            {
                UnityEngine.Object.Destroy(cloudObjects[i]);
            }

            cloudObjects = null;
            cloudsPropertise = null;
        }

        GameObject CreateCloudObject(CloudsPropertise cloudPropertise)
        {
            Vector3 position = cloudPropertise.cloudsPosition;
            Vector3 scale = cloudPropertise.cloudsScale;
            Color color = cloudPropertise.cloudsColor;

            GameObject go = (GameObject)UnityEngine.Object.Instantiate(Mod.prop.CloudTemp, new Vector3(
                           UnityEngine.Random.Range(-scale.x + position.x, scale.x + position.x),
                           UnityEngine.Random.Range(position.y, scale.y + position.y),
                           UnityEngine.Random.Range(-scale.z + position.z, scale.z + position.z)),
                           new Quaternion(0, 0, 0, 0));
            go.transform.SetParent(Mod.environmentMod.SceneObjects.transform);
            go.transform.localScale = new Vector3(100, 100, 100);
            go.name = "Cloud Object";
            go.SetActive(true);

            ParticleSystem ps = go.GetComponent<ParticleSystem>();
            ps.startColor = color;

            go.AddComponent<CloudScript>();

            return go;
        }

        void Update()
        {
            //if (Input.GetKeyDown(KeyCode.T))
            //{
            //    BesiegeConsoleController.ShowMessage(":??");


            //    Level.GetCurrentLevel().AddEntity(8003, Vector3.up * 10, transform.rotation, Vector3.one * 3);
               
            //}
        }

    }

    public class CloudPropertise : EnvironmentPropertise
    {
        public int cloudsSize { get; set; } = 0;
        public Vector3 cloudsPosition { get; set; } = Vector3.zero;
        public Vector3 cloudsScale { get; set; } = new Vector3(1000, 200, 1000);
        public Color cloudsColor { get; set; } = new Color(0.92f, 0.92f, 0.92f, 0.5f);
    }

    public class CloudScript : MonoBehaviour
    {
        int time = 0;

        Vector3 axis = new Vector3(UnityEngine.Random.Range(-0.1f, 0.1f), 1f, UnityEngine.Random.Range(-0.1f, 0.1f));

        private void FixedUpdate()
        {

            transform.RotateAround(transform.localPosition, axis, Time.deltaTime * 0.3f);
            if (time++ == 20)
            {
                GetComponent<ParticleSystem>().startSize = UnityEngine.Random.Range(30, 200);
            }

            if (time > 20)
            {
                time = 0;
            }

        }

    }

}
