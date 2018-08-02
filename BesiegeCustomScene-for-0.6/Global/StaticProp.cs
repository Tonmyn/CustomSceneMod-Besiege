using Modding;
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
        public static AssetBundle iteratorVariable1;
        public GameObject TileTemp = null;
        public GameObject WaterTemp = null;
        public GameObject CloudTemp = null;
        public GameObject IceTemp = null;
        public GameObject SnowTemp = null;
        [Obsolete]
        public List<GameObject> MaterialTemp = new List<GameObject>();
        public static string BundlePath = "assets/standard assets/besiegecustomscene/";
        public static Mesh MeshFormBundle(string Objname)
        {
            Mesh mesh = iteratorVariable1.LoadAsset<Mesh>(BundlePath + "Mesh/" + Objname + ".obj");
            return mesh;
        }
        public static Texture TextureFormBundle(string Objname)
        {
            Texture te = iteratorVariable1.LoadAsset<Texture>(BundlePath + "Texture/" + Objname + ".jpg");
            if (te == null) te = iteratorVariable1.LoadAsset<Texture>(BundlePath + "Texture/" + Objname + ".png");
            return te;
        }
        public GameObject GetObjectInScene(string ObjectName)
        {
            try
            {


                //GameObject ObjectTemp = (GameObject)Instantiate(transform.Find(ObjectName).gameObject);
                GameObject ObjectTemp = PrefabMaster.GetPrefab(StatMaster.Category.Weather, 2).transform.FindChild(ObjectName).gameObject;



                ObjectTemp.name = ObjectName + " Temp";
               // UnityEngine.Object.DontDestroyOnLoad(ObjectTemp);
#if DEBUG
                GeoTools.Log("Get " + ObjectName + "Temp Successfully");
#endif
                ObjectTemp.SetActive(false);
                return ObjectTemp;
            }
            catch (Exception ex)
            {
                GeoTools.Log("Error! Get " + ObjectName + " Temp Failed");
                GeoTools.Log(ex.ToString());
                return null;
            }
        }
        private string StartedScene = "";
        public static double t = 5;
        void FixedUpdate()
        {
            if (Isstart > 6 * t) return;
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
                            for (int i = 0; i < names.Length; i++)
                            {
#if DEBUG
                                //GeoTools.Log(names[i]);
#endif
                            }
                        }
#if DEBUG
                        GeoTools.Log("assetBundle succese");
#endif
                    }
                    catch (Exception ex)
                    {
                        GeoTools.Log("Error! assetBundle failed");
                        GeoTools.Log(ex.ToString());
                    }

                }
                if (Isstart == 2 * t)
                {
                    //StartedScene = SceneManager.GetActiveScene().name;
                    //if (StartedScene != "TITLE SCREEN")
                    //{
                    //    return;
                    //}
                    ////GeoTools.OpenScene("TITLE SCREEN");
                    if (CloudTemp != null) return;

                     CloudTemp = GetObjectInScene("CLOUD/RAIN CLOUD");
                    if (CloudTemp == null) return;
                    ParticleSystemRenderer psr = CloudTemp.GetComponent<ParticleSystemRenderer>();
                    psr.receiveShadows = false;
                    psr.sharedMaterial.mainTexture = iteratorVariable1.LoadAsset<Texture>(
                        "Assets/Standard Assets/ParticleSystems/Textures/ParticleCloudWhite.png");
                    psr.shadowCastingMode = ShadowCastingMode.Off;
                    ParticleSystem ps = CloudTemp.GetComponent<ParticleSystem>();
                    ps.startSize = 30;
                    ps.startLifetime = 60;
                    ps.startSpeed = 0.8f;
                    ps.maxParticles = 15;
                    CloudTemp.name = "Cloud Temp";
                    DontDestroyOnLoad(CloudTemp);
                    CloudTemp.SetActive(false);
#if DEBUG
                    GeoTools.Log("Get " + CloudTemp.name + " Successfully");
#endif
                }
                if (Isstart == 3 * t)
                {
                    //                    for (int i = 0; i <= 10; i++)
                    //                    {
                    //                        string jpgName = "WM" + i.ToString();
                    //                        if (!File.Exists(GeoTools.TexturePath + jpgName + ".jpg")) break;
                    //                        GameObject WMTemp = GameObject.CreatePrimitive(PrimitiveType.Plane);
                    //                        WMTemp.GetComponent<Renderer>().material.shader = Shader.Find("Standard");
                    //                        WMTemp.GetComponent<Renderer>().material.SetFloat("_Glossiness", 1);
                    //                        WMTemp.GetComponent<Renderer>().material.mainTexture = GeoTools.LoadTexture(jpgName);
                    //                        WMTemp.name = jpgName + "Temp";
                    //                        DontDestroyOnLoad(WMTemp);
                    //                        WMTemp.SetActive(false);
                    //                        this.MaterialTemp.Add(WMTemp);
                    //#if DEBUG
                    //                        GeoTools.Log("Get " + WMTemp.name + " Successfully");
                    //#endif
                    //                    }
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
                    TileTemp.name = "Tile Temp";
                    TileTemp.SetActive(false);
                    WaterTemp.GetComponent<WaterBase>().sharedMaterial = TileTemp.GetComponent<Renderer>().material;
                    UnityEngine.Object.DontDestroyOnLoad(WaterTemp);
                    WaterTemp.name = "Water Temp";
                    WaterTemp.SetActive(false);
#if DEBUG
                    GeoTools.Log("Get " + TileTemp.name + " Successfully");
#endif
                }
                if (Isstart == 5 * t)
                {
                    SnowTemp = iteratorVariable1.LoadAsset<GameObject>(
                        "assets/standard assets/particlesystems/prefabs/duststom2.prefab");
                    SnowTemp.name = "Snow Temp";
                    SnowTemp.SetActive(false);
                    UnityEngine.Object.DontDestroyOnLoad(SnowTemp);
#if DEBUG
                    GeoTools.Log("Get " + SnowTemp.name + " Successfully");
#endif
                }
            }
            catch (Exception ex)
            {
                GeoTools.Log(ex.ToString());
            }
            if (Isstart < 6 * t) Isstart++;
        }
    }
}
