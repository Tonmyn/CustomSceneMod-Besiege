using System;
using System.Collections.Generic;
using UnityEngine;
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
        public GameObject cloudTemp = null;
        public List<GameObject> MaterialTemp = new List<GameObject>();
        public GameObject iceTemp = null;
        public GameObject snowTemp = null;
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
        public static double t = 2;
        void FixedUpdate()
        {
            try
            {
                if (Isstart == 1)
                {
                    StartedScene = SceneManager.GetActiveScene().name;
                    GeoTools.OpenScene("TITLE SCREEN");
                    cloudTemp = GetObjectInScene("CLoud");
                    GameObject matTemp = GetObjectInScene("Water");
                    if (matTemp != null) this.MaterialTemp.Add(matTemp);
                }
                // if (Isstart == 5 * t) { GeoTools.OpenScene("36"); }
                // if (Isstart == 10 * t) { snowTemp = GetObjectInScene("Snow"); }
                // if (Isstart == 20 * t) { GeoTools.OpenScene(StartedScene); }
                if (Isstart == 5 * t)
                {
                    GameObject WM = GameObject.CreatePrimitive(PrimitiveType.Plane);
                    WM.GetComponent<Renderer>().material.shader = Shader.Find("Standard");
                    WM.GetComponent<Renderer>().material.SetFloat("_Glossiness", 1);
                    WM.GetComponent<Renderer>().material.mainTexture = GeoTools.LoadTexture("WM1");
                    WM.name = "WMTemp";
                    UnityEngine.Object.DontDestroyOnLoad(WM);
                    Debug.Log("Get " + WM.name + "Temp Successfully");
                    WM.SetActive(false);
                    this.MaterialTemp.Add(WM);
                }
                if (Isstart == 15 * t)
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
                if (Isstart == 20 * t)
                {
                                    
                    WaterTemp = new GameObject();
                    WaterTemp.AddComponent<WaterBase>();
                    WaterTemp.AddComponent<SpecularLighting>();
                    WaterTemp.AddComponent<PlanarReflection>();
                    WaterTemp.AddComponent<GerstnerDisplace>();

                    TileTemp = iteratorVariable1.LoadAsset<GameObject>(
                 "assets/standard assets/environment/water/water4/prefabs/tileonly.prefab");
                    TileTemp.AddComponent<WaterTile>();
                    TileTemp.GetComponent<WaterTile>().reflection = WaterTemp.GetComponent<PlanarReflection>();
                    TileTemp.GetComponent<WaterTile>().waterBase = WaterTemp.GetComponent<WaterBase>();
                    Material mat = TileTemp.GetComponent<Renderer>().material;          
                    mat.SetColor("_SpecularColor", Color.black);                   
                    UnityEngine.Object.DontDestroyOnLoad(TileTemp);    
                    TileTemp.SetActive(false);
                
                    WaterTemp.GetComponent<WaterBase>().sharedMaterial = TileTemp.GetComponent<Renderer>().material;
                    UnityEngine.Object.DontDestroyOnLoad(WaterTemp);
                    WaterTemp.name = "WaterTemp";
                    WaterTemp.SetActive(false);
                   
                    Debug.Log("Get TileTemp Successfully");
                }
            }
            catch (Exception ex)
            {
                Debug.Log(ex.ToString());
            }
            if (Isstart <= 50 * t) Isstart++;
        }
    }
}
