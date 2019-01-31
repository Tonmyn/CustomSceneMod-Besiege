using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BesiegeCustomScene
{
    public class CustomSceneMod : MonoBehaviour
    {
    
        /// <summary>地图包路径</summary>
        public static string ScenePacksPath;
        /// <summary>地图包列表</summary>
        public List<ScenePack> ScenePacks;
        /// <summary>地图包设置参数</summary>
        public SceneSetting Camera;

        public bool WorldBoundariesEnable = true;

        public bool FloorBigEnable = true;

        public bool FogEnable = true;

        /// <summary>
        /// 地图包设置参数结构体
        /// </summary>
        public struct SceneSetting
        {
            public int farClipPlane;
            public float focusLerpSmooth;
            public Vector3 fog;
            public bool SSAO;
            
            //public static SceneSetting None { get; } = new SceneSetting { farClipPlane = 3000, focusLerpSmooth = 100, fog = Vector3.one, SSAO = false };
        }

        /// <summary>地图包类</summary>
        public class ScenePack
        {

            /// <summary>
            /// 地图路径
            /// </summary>
            public string Path;

            /// <summary>
            /// 地图名称
            /// </summary>
            public string Name;

            /// <summary>
            /// 网格文件路径
            /// </summary>
            public string MeshsPath;

            /// <summary>
            /// 贴图文件路径
            /// </summary>
            public string TexturesPath;

            /// <summary>
            /// 设置文件路径
            /// </summary>
            public string SettingFilePath;

            /// <summary>
            /// 地图包类型
            /// </summary>
            public enum SceneType
            {
                /// <summary>可用</summary>
                Enabled = 0,
                /// <summary>不可用</summary>
                Disable = 1,
                /// <summary>空</summary>
                Empty = 3,
            }

            public SceneType Type;

            public ScenePack(DirectoryInfo folderName)
            {

                Name = folderName.Name;
                Path = folderName.FullName;
                MeshsPath = Path + "/Meshs";
                TexturesPath = Path + "/Textures";
                SettingFilePath = string.Format("{0}/setting.txt", Path);

                if (!File.Exists(string.Format("{0}/setting.txt", Path)))
                {
                    Type = SceneType.Empty;
                }
                else
                {
                    Type = SceneType.Enabled;
                }

            }

        }



        //UI.SceneSettingUI sUI;

        void Awake()
        {

            ScenePacksPath = GeoTools.ScenePackPath;

            ScenePacks = ReadScenePacks(ScenePacksPath);

            GameObject customSceneMod = gameObject;

            customSceneMod.AddComponent<MeshMod>();
            customSceneMod.AddComponent<CubeMod>();
            customSceneMod.AddComponent<SnowMod>();
            customSceneMod.AddComponent<CloudMod>();
            customSceneMod.AddComponent<WaterMod>();
            customSceneMod.AddComponent<SkyMod>();
            
        }

        void Start()
        {

            SceneManager.sceneLoaded += (Scene s, LoadSceneMode lsm) =>
            {
                WorldBoundariesEnable = true;

                FloorBigEnable = true;

                FogEnable = true;
            };
        }

        void OnDisable()
        {
            ClearScene();
        }

        void OnDestroy()
        {
            ClearScene();
        }



        /// <summary>
        /// 读取指定路径下所有地图包
        /// </summary>
        /// <param name="scenesPackPath">地图包路径</param>
        /// <returns></returns>
        public List<ScenePack> ReadScenePacks(string scenesPackPath)
        {
            List<ScenePack> SPs = new List<ScenePack>() { };

            if (!Directory.Exists(scenesPackPath))
            {
                GeoTools.Log("Error! Scenes Path Directory not exists!");
                return SPs;
            }

            DirectoryInfo TheFolder = new DirectoryInfo(scenesPackPath);

            //遍历文件夹
            foreach (DirectoryInfo NextFolder in TheFolder.GetDirectories())
            {
                SPs.Add(new ScenePack(NextFolder));
            }

            return SPs;
        }

        public List<ScenePack> ReloadScenePacks()
        {
            return ReadScenePacks(ScenePacksPath);
        }

//        //读取地图地图设定 参数:地图包

//        public void ReadSceneSetting(ScenePack scenePack)
//        {
//            Resources.UnloadUnusedAssets();


//            try
//            {
//#if DEBUG
//                GeoTools.Log(Application.dataPath);
//#endif
//                if (!File.Exists(scenePack.SettingFilePath))
//                {
//                    GeoTools.Log("Error! Scene File not exists!");
//                    return;
//                }

//                FileStream fs = new FileStream(scenePack.SettingFilePath, FileMode.Open);
//                //打开数据文件
//                StreamReader srd = new StreamReader(fs, Encoding.Default);

//                while (srd.Peek() != -1)
//                {
//                    string str = srd.ReadLine();
//                    string[] chara = str.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
//                    if (chara.Length > 2)
//                    {
//                        #region Camera
//                        if (chara[0] == "Camera")
//                        {
//                            if (chara[1] == "farClipPlane")
//                            {
//                                try
//                                {
//                                    GameObject.Find("Main Camera").GetComponent<Camera>().farClipPlane = Convert.ToInt32(chara[2]);
//                                }
//                                catch (Exception ex)
//                                {
//                                    GeoTools.Log("farClipPlane Error");
//                                    GeoTools.Log(ex.ToString());
//                                }
//                            }
//                            else if (chara[1] == "focusLerpSmooth")
//                            {
//                                try
//                                {
//                                    if (chara[2] == "Infinity")
//                                    {
//                                        GameObject.Find("Main Camera").GetComponent<MouseOrbit>().focusLerpSmooth = float.PositiveInfinity;
//                                    }
//                                    else
//                                    {
//                                        GameObject.Find("Main Camera").GetComponent<MouseOrbit>().focusLerpSmooth = Convert.ToSingle(chara[2]);
//                                    }
//                                }
//                                catch (Exception ex)
//                                {
//                                    GeoTools.Log("focusLerpSmooth Error");
//                                    GeoTools.Log(ex.ToString());
//                                }
//                            }
//                            else if (chara[1] == "fog")
//                            {
//                                try
//                                {
//                                    GameObject.Find("Fog Volume").transform.localScale = new Vector3(0, 0, 0);
//                                }
//                                catch
//                                {
//                                    try
//                                    {
//                                        GameObject.Find("Fog Volume Dark").transform.localScale = new Vector3(0, 0, 0);
//                                    }
//                                    catch
//                                    {
//                                        GeoTools.Log("fog error");
//                                    }
//                                }
//                            }
//                            else if (chara[1] == "SSAO")
//                            {
//                                //if (chara[2] == "OFF")
//                                //{
//                                //    GeoTools.Log("SSAO OFF");
//                                //    OptionsMaster.SSAO = true;
//                                //    FindObjectOfType<ToggleAO>().Set();
//                                //}
//                                //else if (chara[2] == "ON")
//                                //{
//                                //    GeoTools.Log("SSAO ON");
//                                //    OptionsMaster.SSAO = false;
//                                //    FindObjectOfType<ToggleAO>().Set();
//                                //}

//                            }

//                        }
//                        #endregion
//                    }
//                }
//                srd.Close();

//                GeoTools.Log("ReadSceneUI Completed!");
//            }
//            catch (Exception ex)
//            {
//                GeoTools.Log("ReadSceneUI Failed!");
//                GeoTools.Log(ex.ToString());
//                return;
//            }
//        }



        //读取地图地图设定 参数:地图包
        public void ReadSceneSetting(ScenePack scenePack)
        {
            Resources.UnloadUnusedAssets();

            SceneSetting SS = new SceneSetting();

            try
            {
#if DEBUG
                GeoTools.Log(Application.dataPath);
#endif
                if (!File.Exists(scenePack.SettingFilePath))
                {
                    GeoTools.Log("Error! Scene File not exists!");
                    return;
                }

                FileStream fs = new FileStream(scenePack.SettingFilePath, FileMode.Open);
                //打开数据文件
                StreamReader srd = new StreamReader(fs, Encoding.Default);

                while (srd.Peek() != -1)
                {
                    string str = srd.ReadLine();
                    string[] chara = str.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    if (chara.Length > 2)
                    {
                        #region Camera
                        if (chara[0] == nameof(Camera))
                        {
                            if (chara[1] == nameof(Camera.farClipPlane))
                            {
                                try
                                {
                                    GameObject.Find("Main Camera").GetComponent<Camera>().farClipPlane = Convert.ToInt32(chara[2]);
                                }
                                catch (Exception ex)
                                {
                                    GeoTools.Log("farClipPlane Error");
                                    GeoTools.Log(ex.ToString());
                                }
                            }
                            else if (chara[1] == nameof(SS.focusLerpSmooth))
                            {
                                try
                                {
                                    if (chara[2] == "Infinity")
                                    {
                                        GameObject.Find("Main Camera").GetComponent<MouseOrbit>().focusLerpSmooth = float.PositiveInfinity;
                                    }
                                    else
                                    {
                                        GameObject.Find("Main Camera").GetComponent<MouseOrbit>().focusLerpSmooth = Convert.ToSingle(chara[2]);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    GeoTools.Log("focusLerpSmooth Error");
                                    GeoTools.Log(ex.ToString());
                                }
                            }
                            else if (chara[1] == nameof(SS.fog))
                            {
                                try
                                {
                                    GameObject.Find("Fog Volume").transform.localScale = new Vector3(0, 0, 0);
                                }
                                catch
                                {
                                    try
                                    {
                                        GameObject.Find("Fog Volume Dark").transform.localScale = new Vector3(0, 0, 0);
                                    }
                                    catch
                                    {
                                        GeoTools.Log("fog error");
                                    }
                                }
                            }
                            else if (chara[1] == nameof(SceneSetting.SSAO))
                            {
                                //if (chara[2] == "OFF")
                                //{
                                //    GeoTools.Log("SSAO OFF");
                                //    OptionsMaster.SSAO = true;
                                //    FindObjectOfType<ToggleAO>().Set();
                                //}
                                //else if (chara[2] == "ON")
                                //{
                                //    GeoTools.Log("SSAO ON");
                                //    OptionsMaster.SSAO = false;
                                //    FindObjectOfType<ToggleAO>().Set();
                                //}

                            }

                        }
                        #endregion
                    }
                }
                srd.Close();

                GeoTools.Log("ReadSceneSetting Completed!");
            }
            catch (Exception ex)
            {
                GeoTools.Log("ReadSceneSetting Failed!");
                GeoTools.Log(ex.ToString());
                return;
            }
        }

        /// <summary>
        /// 加载地图包
        /// </summary>
        /// <param name="ScenePack">地图包</param>
        IEnumerator ILoadScenePack(ScenePack scenePack)
        {
            if (SceneManager.GetActiveScene().name != "2")
            {
                SceneManager.LoadScene("2", LoadSceneMode.Single);//打开level  
            }
            yield return null;

            loadScenePack(scenePack);

        }

        /// <summary>
        /// 加载地图包 多人模式下
        /// </summary>
        /// <param name="ScenePack">地图包</param>
        IEnumerator ILoadScenePack_Multiplayer(ScenePack scenePack)
        {

            yield return null;

            loadScenePack(scenePack);

        }

        void loadScenePack(ScenePack scenePack)
        {

            hideFloorBig();
            ReadSceneSetting(scenePack);
            try { GetComponent<MeshMod>().ReadScene(scenePack); } catch { }
            try { GetComponent<TriggerMod>().ReadScene(scenePack); } catch { }
            try { GetComponent<CubeMod>().ReadScene(scenePack); } catch { }
            try { GetComponent<WaterMod>().ReadScene(scenePack); } catch { }
            try { GetComponent<CloudMod>().ReadScene(scenePack); } catch { }
            try { GetComponent<SnowMod>().ReadScene(scenePack); } catch { }
            try { GetComponent<SkyMod>().ReadScene(scenePack); } catch { }
        }

        /// <summary>
        /// 加载地图包
        /// </summary>
        /// <param name="ScenePack">地图包列表序号</param>
        public void LoadScenePack(int index)
        {
#if DEBUG
            GeoTools.Log("load scene pack");
#endif
            if (BesiegeNetworkManager.Instance == null)
            {
#if DEBUG
                GeoTools.Log("load scene pack not in multiplayers");
#endif
                StartCoroutine(ILoadScenePack(ScenePacks[index]));
                return;

            }


            if (BesiegeNetworkManager.Instance.isActiveAndEnabled && BesiegeNetworkManager.Instance.isConnected)
            {
#if DEBUG
                GeoTools.Log("load scene pack in multiplayers");
#endif
                StartCoroutine(ILoadScenePack_Multiplayer(ScenePacks[index]));
                return;
            }

            //StartCoroutine(ILoadScenePack(ScenePacks[index]));
        }

        //清除地图
        public void ClearScene()
        {
            Resources.UnloadUnusedAssets();

            try
            {
                this.gameObject.GetComponent<MeshMod>().ClearMeshes();
            }
            catch { }
            try
            {
                this.gameObject.GetComponent<TriggerMod>().ClearTrigger();
            }
            catch { }
            try
            {
                this.gameObject.GetComponent<CubeMod>().ClearCube();
            }
            catch { }
            try
            {
                this.gameObject.GetComponent<WaterMod>().ClearWater();
            }
            catch { }
            try
            {
                this.gameObject.GetComponent<CloudMod>().ClearCloud();
            }
            catch { }
        }


        #region 隐藏/显示地面 空气墙 雾
 
        /// <summary>
        /// 隐藏地面
        /// </summary>
        public void HideFloorBig()
        {

            if (!FloorBigEnable)
            {
                UnhideFloorBig();
                return;
            }

            hideFloorBig();

        }

        private void hideFloorBig()
        {

            GameObject floorBig = GameObject.Find("FloorBig");

            try
            {
                floorBig.GetComponent<BoxCollider>().enabled = false;
                floorBig.GetComponent<MeshRenderer>().enabled = false;
                foreach (var v in floorBig.GetComponentsInChildren<Renderer>())
                {
                    v.enabled = false;
                }

                FloorBigEnable = false;
            }
            catch{ }
        }

        /// <summary>
        /// 还原FloorBig
        /// </summary>
        private void UnhideFloorBig()
        {

            GameObject floorBig = GameObject.Find("FloorBig");

            try
            {
                floorBig.GetComponent<BoxCollider>().enabled = true;
                floorBig.GetComponent<MeshRenderer>().enabled = true;
                foreach (var v in floorBig.GetComponentsInChildren<Renderer>())
                {
                    v.enabled = true;
                }
                FloorBigEnable = true;
            }
            catch { }

        }

        /// <summary>
        /// 隐藏空气墙
        /// </summary>
        public void HideWorldBoundaries()
        {

            try
            {

                //单人模式下
                GameObject WorldBoundaries_Large = GameObject.Find("WORLD BOUNDARIES_LARGE");

                Set_WorldBoundaries(WorldBoundaries_Large);

            }
            catch (Exception e)
            {
                GeoTools.Log(e.Message);
                WorldBoundariesEnable = !WorldBoundariesEnable;
            }

            try
            {
                //多人模式下
                GameObject WorldBoundaries = GameObject.Find("WORLD BOUNDARIES");

                Set_WorldBoundaries(WorldBoundaries);

            }
            catch(Exception e)
            {
                GeoTools.Log(e.Message);
                WorldBoundariesEnable = !WorldBoundariesEnable;
            }

            void Set_WorldBoundaries(GameObject WorldBoundaries)
            {
                WorldBoundariesEnable = !WorldBoundariesEnable;

                foreach (BoxCollider BC in WorldBoundaries.GetComponentsInChildren<BoxCollider>())
                {
                    BC.isTrigger = !WorldBoundariesEnable;
                }

                foreach (Renderer MR in WorldBoundaries.GetComponentsInChildren<Renderer>())
                {
                    MR.enabled = WorldBoundariesEnable;
                }
                
            }
         

        }

        /// <summary>
        /// 隐藏雾
        /// </summary>
        public void HideFog()
        {
            FogEnable = !FogEnable;

            //try
            //{
            //    GameObject mainCamera = GameObject.Find("Main Camera");
            //    mainCamera.GetComponent<ColorfulFog>().enabled = FogEnable;
            //}
            //catch
            //{ }

            try
            {
                GameObject fogSPHERE = GameObject.Find("FOG SPHERE");
                fogSPHERE.GetComponent<MeshRenderer>().enabled = FogEnable;
            }
            catch
            { }

            try
            {
                GameObject.Find("Fog Volume").GetComponent<MeshRenderer>().enabled = FogEnable;
            }
            catch (Exception e)
            {
                GeoTools.Log(e.Message);
            }

        }
        #endregion
    }

   

}
