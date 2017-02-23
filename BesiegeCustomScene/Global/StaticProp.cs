using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

namespace BesiegeCustomScene
{
    public class Prop : MonoBehaviour
    {
        void Start()
        {
            Isstart = 0;
        }
        private int Isstart = 0;
        public AssetBundle iteratorVariable1;
        public GameObject TileTemp = null;
        public GameObject WaterTemp = null;
        public GameObject CloudTemp = null;
        public GameObject IceTemp = null;
        public GameObject SnowTemp = null;
        public List<GameObject> MaterialTemp = new List<GameObject>();
        public GameObject GetObjectInScene(string ObjectName)
        {
            try
            {
                GameObject ObjectTemp = (GameObject)Instantiate(GameObject.Find(ObjectName));
                ObjectTemp.name = ObjectName + "Temp";
                UnityEngine.Object.DontDestroyOnLoad(ObjectTemp);
                Debug.Log("Get " + ObjectName + "Temp Successfully");
                ObjectTemp.SetActive(false);
                return ObjectTemp;
            }
            catch (Exception ex)
            {
                Debug.Log("Error! Get " + ObjectName + "Temp Failed");
                Debug.Log(ex.ToString());
                return null;
            }
        }
        private string StartedScene = "";
        public static double t = 5;
        void FixedUpdate()
        {
            if (Isstart > 5 * t) return;
            try
            {
                if (Isstart == 1 * t)
                {
                    try
                    {
                        WWW iteratorVariable0 = new WWW("file:///" + GeoTools.ShaderPath + "water5");
                        iteratorVariable1 = iteratorVariable0.assetBundle;
                        if (iteratorVariable1 != null)
                        {
                            string[] names = iteratorVariable1.GetAllAssetNames();
                            for (int i = 0; i < names.Length; i++) { Debug.Log(names[i]); }
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.Log("Error! assetBundle failed");
                        Debug.Log(ex.ToString());
                    }

                }
                if (Isstart == 2*t)
                {
                    StartedScene = SceneManager.GetActiveScene().name;
                    GeoTools.OpenScene("TITLE SCREEN");
                    CloudTemp = GetObjectInScene("CLoud");             
                    ParticleSystemRenderer psr = CloudTemp.GetComponent<ParticleSystemRenderer>();
                    psr.receiveShadows = false;
                    psr.sharedMaterial.mainTexture = GeoTools.LoadTexture("ParticleCloudWhite");
                    psr.shadowCastingMode = ShadowCastingMode.Off;
                    ParticleSystem ps = CloudTemp.GetComponent<ParticleSystem>();
                    ps.startSize = 30;
                    ps.startLifetime = 60;
                    ps.startSpeed = 0.8f;
                    ps.maxParticles = 15;
                    CloudTemp.name = "CloudTemp";
                    DontDestroyOnLoad(CloudTemp);                   
                    CloudTemp.SetActive(false);                   
                    Debug.Log("Get " + CloudTemp.name + " Successfully");
                }           
                if (Isstart == 3 * t)
                {
                    GameObject WM0 = GameObject.CreatePrimitive(PrimitiveType.Plane);
                    WM0.GetComponent<Renderer>().material.shader = Shader.Find("Standard");
                    WM0.GetComponent<Renderer>().material.SetFloat("_Glossiness", 1);
                    WM0.GetComponent<Renderer>().material.mainTexture = GeoTools.LoadTexture("WM0");
                    WM0.name = "WM0Temp";
                    DontDestroyOnLoad(WM0);
                    WM0.SetActive(false);
                    this.MaterialTemp.Add(WM0);
                    Debug.Log("Get " + WM0.name + " Successfully");

                    GameObject WM1 = GameObject.CreatePrimitive(PrimitiveType.Plane);
                    WM1.GetComponent<Renderer>().material.shader = Shader.Find("Standard");
                    WM1.GetComponent<Renderer>().material.SetFloat("_Glossiness", 1);
                    WM1.GetComponent<Renderer>().material.mainTexture = GeoTools.LoadTexture("WM1");
                    WM1.name = "WM1Temp";
                    DontDestroyOnLoad(WM1);                 
                    WM1.SetActive(false);
                    this.MaterialTemp.Add(WM1);
                    Debug.Log("Get " + WM1.name + " Successfully");
                }
               
                if (Isstart == 4 * t)
                {                          
                    WaterTemp = new GameObject();
                    WaterTemp.AddComponent<WaterBase>();
                    WaterTemp.AddComponent<SpecularLighting>();
                    WaterTemp.AddComponent<PlanarReflection>();
                    WaterTemp.AddComponent<GerstnerDisplace>();                  
                    TileTemp = iteratorVariable1.LoadAsset<GameObject>(
                        "assets/standard assets/environment/water/water4/prefabs/TileOnly.prefab");
                    TileTemp.AddComponent<WaterTile>();
                    TileTemp.GetComponent<WaterTile>().reflection = WaterTemp.GetComponent<PlanarReflection>();
                    TileTemp.GetComponent<WaterTile>().waterBase = WaterTemp.GetComponent<WaterBase>();                 
                    Material mat = TileTemp.GetComponent<Renderer>().material;
                    GeoTools.ResetWaterMaterial(ref mat);               
                    UnityEngine.Object.DontDestroyOnLoad(TileTemp);
                    TileTemp.name = "TileTemp";
                    TileTemp.SetActive(false);                
                    WaterTemp.GetComponent<WaterBase>().sharedMaterial = TileTemp.GetComponent<Renderer>().material;
                    UnityEngine.Object.DontDestroyOnLoad(WaterTemp);
                    WaterTemp.name = "WaterTemp";
                    WaterTemp.SetActive(false);              
                    Debug.Log("Get "+ TileTemp.name+" Successfully");
                }
            }
            catch (Exception ex)
            {
                Debug.Log(ex.ToString());
            }
            if (Isstart <= 5 * t) Isstart++;
        }
    }
}
