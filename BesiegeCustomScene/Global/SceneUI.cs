//using System.Threading.Tasks;
using spaar;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BesiegeCustomScene
{
    public class SceneUI : MonoBehaviour
    {
        private bool isSimulating = false;
        //UI
        private int _FontSize = 15;
        private Rect windowRect = new Rect(Screen.width *0.05f, Screen.height *0.908f, 585f, 50f);
        private int windowID = spaar.ModLoader.Util.GetWindowID();
        private bool ShowGUI = true;
        private List<string> _ButtonName = new List<string>();
        private List<string> _SceneName = new List<string>();
        private string ScenePath = GeoTools.ScenePath;
        KeyCode _DisplayUI = KeyCode.F9;
        KeyCode _ReloadUI = KeyCode.F5;
        void DefaultUI()
        {
            _ButtonName.Clear(); _SceneName.Clear();
            _FontSize = (int)(Screen.width * 0.005 + 8);
            ShowGUI = true;
            windowRect = new Rect(Screen.width * 0.05f, Screen.height*0.908f, 585f, 50f);
            _DisplayUI = KeyCode.F9;
            _ReloadUI = KeyCode.F5;
        }
        void DefaultItem()
        {
            _ButtonName.Clear(); _SceneName.Clear();
        }
        void ReadUI()
        {
            DefaultUI();
            try
            {
                StreamReader srd;
                if (File.Exists(GeoTools.UIPath + "CHN.txt"))
                {
                    Debug.Log("zh-CN UI " + Screen.width.ToString() + "*" + Screen.height.ToString());
                    srd = File.OpenText(GeoTools.UIPath + "CHN.txt");
                }
                else
                {
                    Debug.Log("en-US UI " + Screen.width.ToString() + "*" + Screen.height.ToString());
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
                    Debug.Log("LoadUISetting Failed!Button Error!");
                    DefaultItem();
                }
                else
                {
                    Debug.Log("LoadUISetting Completed!");
                }
            }
            catch (Exception ex)
            {
                Debug.Log("Error! LoadUISetting Failed!");
                Debug.Log(ex.ToString());
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
                if (str2[i] >= 0x4e00 && str2[i] <= 0x9fbb) { str2width += 0.8; }
                else
                {
                    str2width += 0.4;
                }
            }
            windowRect.width = (int)(_FontSize * (str2width + (_ButtonName.Count - 1)));
            windowRect.height = (int)(_FontSize * 2);
        }
        public void ReadScene(string SceneName)
        {
            try
            {
                //Debug.Log(Application.dataPath);
                if (!File.Exists(ScenePath + SceneName + ".txt"))
                {
                    Debug.Log("Scene File not exists!");
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
                                    Debug.Log("farClipPlane Error");
                                    Debug.Log(ex.ToString());
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
                                    Debug.Log("focusLerpSmooth Error");
                                    Debug.Log(ex.ToString());
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
                                        Debug.Log("fog error");
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                }
                srd.Close();
                Debug.Log("ReadSceneUI Completed!");
            }
            catch (Exception ex)
            {
                Debug.Log("ReadSceneUI Failed!");
                Debug.Log(ex.ToString());
                return;
            }
        }
        void ClearScene()
        {
            try
            {
                this.gameObject.GetComponent<MeshMod>().ClearMeshes();
            }
            catch { }
            try
            {
                this.gameObject.GetComponent<TriggerUI>().ClearTrigger();
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
        void LoadScene(string SceneName)
        {
            if (SceneManager.GetActiveScene().name != "2")
            {
                SceneManager.LoadScene("2", LoadSceneMode.Single);//打开level  
            }
            else
            {
                HideFloorBig();
                this.ReadScene(SceneName);
                try { this.gameObject.GetComponent<MeshMod>().ReadScene(SceneName); } catch { }
                try { this.gameObject.GetComponent<TriggerUI>().ReadScene(SceneName); } catch { }
                try { this.gameObject.GetComponent<WaterMod>().ReadScene(SceneName); } catch { }
                try { this.gameObject.GetComponent<CloudMod>().ReadScene(SceneName); } catch { }
                try { this.gameObject.GetComponent<SnowMod>().ReadScene(SceneName); } catch { }
            }
        }
        /// ///////////////////////////////////////
        void FixedUpdate()
        {
            if (StatMaster.isSimulating && isSimulating == false)
            {
                isSimulating = true; this.ShowGUI = false;
            }
            else if (!StatMaster.isSimulating && isSimulating == true)
            {
                isSimulating = false; this.ShowGUI = true;
            }
        }
        void Update()
        {
            if (Input.GetKeyDown(_ReloadUI) && Input.GetKey(KeyCode.LeftControl))
            {
                ReadUI();
            }
            if (Input.GetKeyDown(_DisplayUI) && Input.GetKey(KeyCode.LeftControl))
            {
                ShowGUI = !ShowGUI;
            }
        }
        void Start()
        {
            ReadUI();
        }
        void OnDisable()
        {
            ClearScene();
        }
        void OnDestroy()
        {
            ClearScene();
        }
        ////////////////////////////////////////////////////
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
                if (GUILayout.Button(_ButtonName[i], style, new GUILayoutOption[0]) && !StatMaster.isSimulating)
                { LoadScene(_SceneName[i]); }
            }
            GUILayout.EndHorizontal();
            GUI.DragWindow(new Rect(0f, 0f, this.windowRect.width, this.windowRect.height));
        }
        private void OnGUI()
        {
            if (ShowGUI)
            {
                this.windowRect = GUI.Window(this.windowID, this.windowRect, new GUI.WindowFunction(DoWindow), "", GUIStyle.none);
            }
        }
        /// <summary>
        /// ////////////////////
        /// </summary>
        private Vector3 fpos = new Vector3();
        private Vector3 gpos = new Vector3();
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
    }
}