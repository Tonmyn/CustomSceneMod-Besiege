//using System.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using static BesiegeCustomScene.SceneMod;

namespace BesiegeCustomScene
{
    public class SceneUI : MonoBehaviour
    {
        //private bool isSimulating = false;
        //UI
        private int _FontSize = 15 ;
        private Rect windowRect;
        private int windowID = GeoTools.GetWindowID();//spaar.ModLoader.Util.GetWindowID();
        private bool ShowGUI;
        private List<string> _ButtonName;
        private List<string> _SceneName;
        [Obsolete]
        private string ScenePath = GeoTools.ScenePath;
        private string ScenePackPath = GeoTools.ScenePackPath;

        KeyCode _DisplayUI;
        KeyCode _ReloadUI;

        /// <summary>地图包列表</summary>
        List<ScenePack> ScenePacks = new List<ScenePack>();

        /// <summary>
        /// 默认界面
        /// </summary>
        void DefaultUI()
        {
            DefaultItem();

            _FontSize = (int)(Screen.width * 0.005 + 8);
            ShowGUI = true;

            windowRect = new Rect(Screen.width * 0.05f, Screen.height * 0.908f, 585f, 50f);
            _DisplayUI = KeyCode.F9;
            _ReloadUI = KeyCode.F5;
        }

        /// <summary>
        /// 默认按钮项目
        /// </summary>
        void DefaultItem()
        {
            _ButtonName = new List<string>();
            _SceneName = new List<string>();
        }

        [Obsolete]
        void ReadUI()
        {
            DefaultUI();
            try
            {
                StreamReader srd;
                if (File.Exists(GeoTools.UIPath + "CHN.txt"))
                {
                    GeoTools.Log("zh-CN UI " + Screen.width.ToString() + "*" + Screen.height.ToString());
                    srd = File.OpenText(GeoTools.UIPath + "CHN.txt");
                }
                else
                {
                    GeoTools.Log("en-US UI " + Screen.width.ToString() + "*" + Screen.height.ToString());
                    srd = File.OpenText(GeoTools.UIPath + "EN.txt");
                }
                while (srd.Peek() != -1)
                {
                    string str = srd.ReadLine();
                    string[] chara = str.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    if (chara.Length > 2)
                    {
                        if (chara[0] == "_Scene")
                        {
                            if (chara[1] == "buttonname")
                            {
                                _ButtonName.Add(chara[2]);
                            }
                            else if (chara[1] == "scenename")
                            {
                                _SceneName.Add(chara[2]);
                            }
                            else if (chara[1] == "display_UI")
                            {
                                KeyCode outputkey;
                                if (GeoTools.StringToKeyCode(chara[2], out outputkey)) _DisplayUI = outputkey;
                            }
                            else if (chara[1] == "reload_UI")
                            {
                                KeyCode outputkey;
                                if (GeoTools.StringToKeyCode(chara[2], out outputkey)) _ReloadUI = outputkey;
                            }
                            else if (chara[1] == "show_on_start")
                            {
                                if (chara[2] == "0" || chara[2] == "OFF") ShowGUI = false;
                                else ShowGUI = true;
                            }
                        }
                        else if (chara[0] == Screen.width.ToString() + "*" + Screen.height.ToString() + "_Scene")
                        {
                            if (chara[1] == "window_poistion")
                            {
                                windowRect.x = Convert.ToSingle(chara[2]);
                                windowRect.y = Convert.ToSingle(chara[3]);
                            }
                        }
                    }
                }
                srd.Close();
                filtUI();
                if (_ButtonName.Count != _SceneName.Count || _ButtonName.Count < 0)
                {
                    GeoTools.Log("LoadUISetting Failed!Button Error!");
                    DefaultItem();
                }
                else
                {
                    GeoTools.Log("LoadUISetting Completed!");
                }

            }
            catch (Exception ex)
            {
                GeoTools.Log("Error! LoadUISetting Failed!");
                GeoTools.Log(ex.ToString());
                DefaultUI();
                return;
            }
        }

        /// <summary>
        /// 读取UI参数 带地图包数据
        /// </summary>
        /// <param name="scenePacks">地图包</param>
        void ReadUI(List<ScenePack> scenePacks)
        {

            DefaultUI();

            try
            {
                StreamReader srd;
                if (File.Exists(GeoTools.UIPath + "CHN.txt"))
                {
                    GeoTools.Log("zh-CN UI " + Screen.width.ToString() + "*" + Screen.height.ToString());
                    srd = File.OpenText(GeoTools.UIPath + "CHN.txt");
                }
                else
                {
                    GeoTools.Log("en-US UI " + Screen.width.ToString() + "*" + Screen.height.ToString());
                    srd = File.OpenText(GeoTools.UIPath + "EN.txt");
                }
                while (srd.Peek() != -1)
                {
                    string str = srd.ReadLine();
                    string[] chara = str.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    if (chara.Length > 2)
                    {
                        if (chara[0] == "_Scene")
                        {
 
                            if (chara[1] == "display_UI")
                            {
                                KeyCode outputkey;
                                if (GeoTools.StringToKeyCode(chara[2], out outputkey)) _DisplayUI = outputkey;
                            }
                            else if (chara[1] == "reload_UI")
                            {
                                KeyCode outputkey;
                                if (GeoTools.StringToKeyCode(chara[2], out outputkey)) _ReloadUI = outputkey;
                            }
                            else if (chara[1] == "show_on_start")
                            {
                                if (chara[2] == "0" || chara[2] == "OFF") ShowGUI = false;
                                else ShowGUI = true;
                            }
                        }
                        else if (chara[0] == Screen.width.ToString() + "*" + Screen.height.ToString() + "_Scene")
                        {
                            if (chara[1] == "window_poistion")
                            {
                                windowRect.x = Convert.ToSingle(chara[2]);
                                windowRect.y = Convert.ToSingle(chara[3]);
                            }
                        }
                    }
                }
                srd.Close();

                foreach (ScenePack _sp in scenePacks)
                {
                    if (_sp.Type == ScenePack.SceneType.Enabled)
                    {
                        _ButtonName.Add(string.Format("[{0}]", _sp.Name));
                        _SceneName.Add(_sp.Name);
                    }

                }

                filtUI();

                if (_ButtonName.Count != _SceneName.Count || _ButtonName.Count < 0)
                {
                    GeoTools.Log("LoadUISetting Failed!Button Error!");
                    DefaultItem();
                }
                else
                {
                    GeoTools.Log("LoadUISetting Completed!");
                }

            }
            catch (Exception ex)
            {
                GeoTools.Log("Error! LoadUISetting Failed!");
                GeoTools.Log(ex.ToString());
                DefaultUI();
                return;
            }
        }

        private void filtUI()
        {

            string str2 = ""; double str2width = 0;
            for (int i = 0; i < _ButtonName.Count; i++)
            {
                str2 += _ButtonName[i];
            }
            for (int i = 0; i < str2.Length; i++)
            {
                if (str2[i] >= 0x4e00 && str2[i] <= 0x9fbb)
                {
                    str2width += 0.8;
                }
                else
                {
                    str2width += 0.4;
                }
            }
            windowRect.width = (int)(_FontSize * (str2width + (_ButtonName.Count - 1)));
            windowRect.height = (int)(_FontSize * 2);
        }

        //读取地图 参数:配置文件名
        [Obsolete]
        public void ReadScene(string SceneName)
        {
            try
            {
#if DEBUG
                GeoTools.Log(Application.dataPath);
#endif
                if (!File.Exists(ScenePath + SceneName + ".txt"))
                {
                    GeoTools.Log("Scene File not exists!");
                    return;
                }
                StreamReader srd = File.OpenText(ScenePath + SceneName + ".txt");
                while (srd.Peek() != -1)
                {
                    string str = srd.ReadLine();
                    string[] chara = str.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    if (chara.Length > 2)
                    {
                        #region Camera
                        if (chara[0] == "Camera")
                        {
                            if (chara[1] == "farClipPlane")
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
                            else if (chara[1] == "focusLerpSmooth")
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
                            else if (chara[1] == "fog")
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
                            else if (chara[1] == "SSAO")
                            {
                                //if (chara[2] == "OFF" )
                                //{
                                //    GeoTools.Log("SSAO OFF");
                                //    OptionsMaster.SSAO = true;
                                //    FindObjectOfType<ToggleAO>().Set();
                                //}
                                //else if (chara[2] == "ON"  )
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
                GeoTools.Log("ReadSceneUI Completed!");
            }
            catch (Exception ex)
            {
                GeoTools.Log("ReadSceneUI Failed!");
                GeoTools.Log(ex.ToString());
                return;
            }
        }   
        //读取地图地图设定 参数:地图包
        public void ReadSceneSetting(ScenePack scenePack)
        {
            Resources.UnloadUnusedAssets();

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
                        if (chara[0] == "Camera")
                        {
                            if (chara[1] == "farClipPlane")
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
                            else if (chara[1] == "focusLerpSmooth")
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
                            else if (chara[1] == "fog")
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
                            else if (chara[1] == "SSAO")
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

                GeoTools.Log("ReadSceneUI Completed!");
            }
            catch (Exception ex)
            {
                GeoTools.Log("ReadSceneUI Failed!");
                GeoTools.Log(ex.ToString());
                return;
            }
        }

        //读取Scenes文件夹下所有地图包
        public List<ScenePack> ReadScenePacks(string scenesPackPath)
        {
            List<ScenePack> SPs = new List<ScenePack>();

            DirectoryInfo TheFolder = new DirectoryInfo(scenesPackPath);

            //遍历文件夹
            foreach (DirectoryInfo NextFolder in TheFolder.GetDirectories())
            {
                SPs.Add(new ScenePack(NextFolder));
            }

            return SPs;
        }
        
        //清除地图
        void ClearScene()
        {
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
        //加载地图
        [Obsolete]
        void LoadScene(string SceneName)
        {
            if (SceneManager.GetActiveScene().name != "2")
            {
                SceneManager.LoadScene("2", LoadSceneMode.Single);//打开level  
               
            }
            else
            {
                HideFloorBig();
                ReadScene(SceneName);
                try { GetComponent<MeshMod>().ReadScene(SceneName); } catch { }
                try { GetComponent<TriggerMod>().ReadScene(SceneName); } catch { }
                try { GetComponent<CubeMod>().ReadScene(SceneName); } catch { }
                try { GetComponent<WaterMod>().ReadScene(SceneName); } catch { }
                try { GetComponent<CloudMod>().ReadScene(SceneName); } catch { }
                try { GetComponent<SnowMod>().ReadScene(SceneName); } catch { }
            }
        }
        //加载地图
        [Obsolete]
        IEnumerator ILoadScene(string SceneName)
        {
            if (SceneManager.GetActiveScene().name != "2")
            {
                SceneManager.LoadScene("2", LoadSceneMode.Single);//打开level  
            }
            yield return null;

            HideFloorBig();
            ReadScene(SceneName);
            try { GetComponent<MeshMod>().ReadScene(SceneName); } catch { }
            try { GetComponent<TriggerMod>().ReadScene(SceneName); } catch { }
            try { GetComponent<CubeMod>().ReadScene(SceneName); } catch { }
            try { GetComponent<WaterMod>().ReadScene(SceneName); } catch { }
            try { GetComponent<CloudMod>().ReadScene(SceneName); } catch { }
            try { GetComponent<SnowMod>().ReadScene(SceneName); } catch { }

        }
        //加载地图
        IEnumerator ILoadScene(ScenePack scenePack)
        {
            if (SceneManager.GetActiveScene().name != "2")
            {
                SceneManager.LoadScene("2", LoadSceneMode.Single);//打开level  
            }
            yield return null;

            HideFloorBig();      
            ReadSceneSetting(scenePack);
            try { GetComponent<MeshMod>().ReadScene(scenePack); } catch { }
            try { GetComponent<TriggerMod>().ReadScene(scenePack); } catch { }
            try { GetComponent<CubeMod>().ReadScene(scenePack); } catch { }
            try { GetComponent<WaterMod>().ReadScene(scenePack); } catch { }
            try { GetComponent<CloudMod>().ReadScene(scenePack); } catch { }
            try { GetComponent<SnowMod>().ReadScene(scenePack); } catch { }

        }

        //加载地图多人模式下
        IEnumerator ILoadScene_Multiplayer(ScenePack scenePack)
        {

            yield return null;

            HideFloorBig();
            ReadSceneSetting(scenePack);
            try { GetComponent<MeshMod>().ReadScene(scenePack); } catch { }
            try { GetComponent<TriggerMod>().ReadScene(scenePack); } catch { }
            try { GetComponent<CubeMod>().ReadScene(scenePack); } catch { }
            try { GetComponent<WaterMod>().ReadScene(scenePack); } catch { }
            try { GetComponent<CloudMod>().ReadScene(scenePack); } catch { }
            try { GetComponent<SnowMod>().ReadScene(scenePack); } catch { }
         
        }

        //void FixedUpdate()
        //{
        //    if (StatMaster.levelSimulating && isSimulating == false)
        //    {
        //        isSimulating = true; this.ShowGUI = false;
        //    }
        //    else if (!StatMaster.levelSimulating && isSimulating == true)
        //    {
        //        isSimulating = false; this.ShowGUI = true;
        //    }
        //}

        void Update()
        {
            //if (Input.GetKeyDown(_ReloadUI) && Input.GetKey(KeyCode.LeftControl))
            //{
            //    ReloadScenes();
            //}
            //if (Input.GetKeyDown(_DisplayUI) && Input.GetKey(KeyCode.LeftControl))
            //{
            //    ShowGUI = !ShowGUI;
            //}

            //ReloadScenes();


        }

        void Start()
        {
            //ReadUI();

            ReloadScenes();

            GetComponent<UI.SceneSettingUI>().scenePacks = ScenePacks;

//            SceneManager.sceneLoaded += (Scene s,LoadSceneMode lsm)=>
//            {
//                HideFloorBig_ExceptDownFloor();
//#if DEBUG
//                GeoTools.Log("加载地图后事件");
//#endif
//            };

        }

        void OnDisable()
        {
            ClearScene();
        }

        void OnDestroy()
        {
            ClearScene();
        }

        void ReloadScenes()
        {
            //读取地图包信息
            ScenePacks = ReadScenePacks(ScenePackPath);
            //读取地图界面
            ReadUI(ScenePacks);
        }
        
        public void DoWindow(int windowID)
        {
            GUIStyle style = new GUIStyle
            {
                normal = { textColor = Color.white },
                alignment = TextAnchor.MiddleCenter,
                active = { background = Texture2D.whiteTexture, textColor = Color.black },
                margin = { top = 5 },
                fontSize = _FontSize
            };
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            for (int i = 0; i < _SceneName.Count; i++)
            {
                if (GUILayout.Button(_ButtonName[i], style, new GUILayoutOption[0]) && !StatMaster.levelSimulating)
                {
                    //LoadScene(_SceneName[i]);
                    //StartCoroutine(ILoadScene(_SceneName[i]));
#if DEBUG
                    GeoTools.Log("load scene");
#endif
                    if (BesiegeNetworkManager.Instance == null)
                    {
#if DEBUG
                        GeoTools.Log("load scene not in multiplayers");
#endif
                        StartCoroutine(ILoadScene(ScenePacks[i]));
                        return;

                    }


                    if (BesiegeNetworkManager.Instance.isActiveAndEnabled && BesiegeNetworkManager.Instance.isConnected)
                    {
#if DEBUG
                        GeoTools.Log("load scene in multiplayers");
#endif
                        StartCoroutine(ILoadScene_Multiplayer(ScenePacks[i]));
                        return;
                    }



                }
            }
            GUILayout.EndHorizontal();
            GUI.DragWindow(new Rect(0f, 0f, this.windowRect.width, this.windowRect.height));
        }

        private void OnGUI()
        {
            if (Input.GetKeyDown(_ReloadUI) && Input.GetKey(KeyCode.LeftControl))
            {
                ReloadScenes();
            }
            if (Input.GetKeyDown(_DisplayUI) && Input.GetKey(KeyCode.LeftControl))
            {
                ShowGUI = !ShowGUI;
            }

            if (ShowGUI)
            {
                this.windowRect = GUI.Window(this.windowID, this.windowRect, new GUI.WindowFunction(DoWindow), "", GUIStyle.none);

            }
        }

#region 隐藏/显示地面
        private Vector3 fpos = new Vector3();
        private Vector3 gpos = new Vector3();

        /// <summary>
        /// 隐藏地面
        /// </summary>
        public void HideFloorBig()
        {
            try
            {
                if (GameObject.Find("FloorGrid").transform.localScale != Vector3.zero) gpos = GameObject.Find("FloorGrid").transform.localScale;
                GameObject.Find("FloorGrid").transform.localScale = new Vector3(0, 0, 0);
            }
            catch { }
            try
            {
                GameObject.Find("Main Camera").GetComponent<Camera>().farClipPlane = 2500;
            }
            catch { }
            try
            {
                if (GameObject.Find("FloorBig").transform.localScale != Vector3.zero) fpos = GameObject.Find("FloorBig").transform.localScale;
                GameObject.Find("FloorBig").transform.localScale = new Vector3(0, 0, 0);
            }
            catch { }

            HideFloorBig_ExceptDownFloor();
        }

        /// <summary>
        /// 隐藏空气墙
        /// </summary>
        public void HideFloorBig_ExceptDownFloor()
        {

            try
            {
                GameObject.Find("WORLD BOUNDARIES").transform.localScale = new Vector3(0, 0, 0);
                GameObject.Find("WorldBounds_Back").transform.localScale = new Vector3(0, 0, 0);
                GameObject.Find("WorldBounds_Front").transform.localScale = new Vector3(0, 0, 0);
                GameObject.Find("WorldBounds_Left").transform.localScale = new Vector3(0, 0, 0);
                GameObject.Find("WorldBounds_Right").transform.localScale = new Vector3(0, 0, 0);
                GameObject.Find("WorldBoundaryBack").transform.GetChild(0).GetComponent<Renderer>().enabled = false;
                GameObject.Find("WorldBoundaryFront").transform.GetChild(0).GetComponent<Renderer>().enabled = false;
                GameObject.Find("WorldBoundaryLeft").transform.GetChild(0).GetComponent<Renderer>().enabled = false;
                GameObject.Find("WorldBoundaryRight").transform.GetChild(0).GetComponent<Renderer>().enabled = false;
            }
            catch { }
        }

        /// <summary>
        /// 还原FloorBig
        /// </summary>
        public void UnhideFloorBig()
        {
            try
            {
                if (fpos != Vector3.zero) GameObject.Find("FloorBig").transform.localScale = fpos;
            }
            catch { }
            try
            {
                if (fpos != Vector3.zero) GameObject.Find("FloorGrid").transform.localScale = gpos;
            }
            catch { }
            try
            {
                GameObject.Find("Main Camera").GetComponent<Camera>().farClipPlane = 1500;
            }
            catch { }
        }
#endregion

    }
}