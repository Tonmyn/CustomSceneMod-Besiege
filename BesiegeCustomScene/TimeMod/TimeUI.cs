using spaar.ModLoader;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BesiegeCustomScene
{
    public enum ModUnit
    {
        kmh = 0,
        ms = 1,
        mach = 2,
    };
    public class TimeUI : UnityEngine.MonoBehaviour
    {
        private GameObject startingBlock;
        bool validBlock = false; bool isSimulating = false;
        private int windowID = spaar.ModLoader.Util.GetWindowID();
        private int _accstep = 1;
        //private bool _mode = false;
        private Rect windowRect = new Rect(15f, 100f, 150f, 150f);
        private ModUnit Unit = ModUnit.kmh;
        KeyCode _DisplayUI = KeyCode.F9;
        KeyCode _ReloadUI = KeyCode.F5;
        KeyCode _GetCenter = KeyCode.W;
        KeyCode _GetLevelInfo = KeyCode.Q;
        KeyCode _GetShader = KeyCode.E;
        private int _FontSize = 15;
        public bool ShowGUI = true;
        public bool EnableCommond = true;
        private string _overloadUI = "Overload";
        private string Overload = "0"; private Vector3 _V = new Vector3(0, 0, 0);
        private string _coordinatesUI = "Coordinates";
        private string X = "0"; private string Y = "0"; private string Z = "0";
        private string _velocityUI = "Velocity(km/h)";
        private string V = "0";
        private string _distanceUI = "Distance";
        private float _Distance = 0; private string Distance = "0"; private Vector3 _Position = new Vector3(0, 0, 0);
        private string _timeUI = "Time";
        private string MTime = "";
        private string _timerUI = "Timer";
        public DateTime _MStartTime;
        //control timer by other component
        public bool _TimerSwith = false;
        private string MTimer = "";
        void DefaultUI()
        {
            _FontSize = (int)(Screen.width * 0.005 + 8);
            ShowGUI = true;
            windowRect = new Rect(15f, 100f, 150f, 150f);
            _DisplayUI = KeyCode.F9;
            _ReloadUI = KeyCode.F5;
            _GetCenter = KeyCode.W;
            _GetLevelInfo = KeyCode.Q;
            _GetShader = KeyCode.E;
            Unit = 0;

            _TimerSwith = false;
            validBlock = false;
            isSimulating = false;
            _Position = new Vector3(0, 0, 0);
            _V = new Vector3(0, 0, 0);
            _accstep = 1;

            _MStartTime = System.DateTime.Now;
            MTime = _MStartTime.ToShortDateString();
            MTimer = "00:00:00";

            _timeUI = "Time";
            _coordinatesUI = "Coordinates";
            _velocityUI = "Velocity";
            _distanceUI = "Distance";
            _overloadUI = "Overload";
            _timerUI = "Timer";
        }
        void ReadUI()
        {
            DefaultUI();
            //Debug.Log(Screen.currentResolution.ToString());
            try
            {
                StreamReader srd;
                if (File.Exists(GeoTools.UIPath + "CHN.txt"))
                {
                    srd = File.OpenText(GeoTools.UIPath + "CHN.txt");
                }
                else
                {
                    srd = File.OpenText(GeoTools.UIPath + "EN.txt");
                }
                //Debug.Log(Ci + "  " + Screen.width.ToString() + "*" + Screen.height.ToString());
                while (srd.Peek() != -1)
                {
                    string str = srd.ReadLine();
                    string[] chara = str.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    if (chara.Length > 2)
                    {
                        if (chara[0] == "_Timer")
                        {
                            if (chara[1] == "unit")
                            {
                                if (chara[2] == "kmh") this.Unit = ModUnit.kmh;
                                if (chara[2] == "ms") this.Unit = ModUnit.ms;
                                if (chara[2] == "mach") this.Unit = ModUnit.mach;
                            }
                            else if (chara[1] == "overload")
                            {
                                if (chara[2] == "OFF") _overloadUI = string.Empty;
                                else _overloadUI = chara[2];
                            }
                            else if (chara[1] == "time")
                            {
                                if (chara[2] == "OFF") _timeUI = string.Empty;
                                else _timeUI = chara[2];
                            }
                            else if (chara[1] == "coordinates")
                            {
                                if (chara[2] == "OFF") _coordinatesUI = string.Empty;
                                else _coordinatesUI = chara[2];
                            }
                            else if (chara[1] == "velocity")
                            {
                                if (chara[2] == "OFF") _velocityUI = string.Empty;
                                else _velocityUI = chara[2];
                            }
                            else if (chara[1] == "timer")
                            {
                                if (chara[2] == "OFF") _timerUI = string.Empty;
                                else _timerUI = chara[2];
                            }
                            else if (chara[1] == "distance")
                            {
                                if (chara[2] == "OFF") _distanceUI = string.Empty;
                                else _distanceUI = chara[2];
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
                            else if (chara[1] == "Get_Center")
                            {
                                KeyCode outputkey;
                                if (GeoTools.StringToKeyCode(chara[2], out outputkey)) _GetCenter = outputkey;
                            }
                            else if (chara[1] == "Get_LevelInfo")
                            {
                                KeyCode outputkey;
                                if (GeoTools.StringToKeyCode(chara[2], out outputkey)) _GetLevelInfo = outputkey;
                            }
                            else if (chara[1] == "Get_Shader")
                            {
                                KeyCode outputkey;
                                if (GeoTools.StringToKeyCode(chara[2], out outputkey)) _GetShader = outputkey;
                            }
                            else if (chara[1] == "show_on_start")
                            {
                                if (chara[2] == "0" || chara[2] == "OFF") ShowGUI = false;
                                else ShowGUI = true;
                            }
                        }
                        else if (chara[0] == Screen.width.ToString() + "*" + Screen.height.ToString() + "_Timer")
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
                if (Unit == ModUnit.kmh) { _velocityUI += "(km/h)"; }
                else if (Unit == ModUnit.ms) { _velocityUI += "(m/s)"; }
                else if (Unit == ModUnit.mach) { _velocityUI += "(mach)"; }
                filtUI();
                Debug.Log("TimerUISetting Completed!");
            }
            catch (Exception ex)
            {
                Debug.Log("Error! TimerUISetting Failed!");
                Debug.Log(ex.ToString());
                DefaultUI();
                return;
            }
        }
        private void filtUI()
        {
            string str2 = ""; double str2width = 0, str2height = 0;
            if (_distanceUI.Length != 0) { str2height++; if (_distanceUI.Length > str2.Length) str2 = _distanceUI; }
            if (_coordinatesUI.Length != 0) { str2height++; if (_coordinatesUI.Length > str2.Length) str2 = _coordinatesUI; }
            if (_velocityUI.Length != 0) { str2height++; if (_velocityUI.Length > str2.Length) str2 = _velocityUI; }
            if (_timeUI.Length != 0) { str2height++; if (_timeUI.Length > str2.Length) str2 = _timeUI; }
            if (_timerUI.Length != 0) { str2height++; if (_timerUI.Length > str2.Length) str2 = _timerUI; }
            if (_overloadUI.Length != 0) { str2height++; if (_overloadUI.Length > str2.Length) str2 = _overloadUI; }
            for (int i = 0; i < str2.Length; i++)
            {
                if (str2[i] >= 0x4e00 && str2[i] <= 0x9fbb) { str2width += 0.8; }
                else
                {
                    str2width += 0.4;
                }
            }
            windowRect.height = (int)(_FontSize * str2height + _FontSize * (str2height - 1) * 0.7f);
            windowRect.width = (int)(_FontSize * str2width * 3);
        }
        void Start()
        {
            ReadUI();
            if (!EnableCommond) return;
            Commands.RegisterCommand("VP_Help", (args, notUses) =>
             {
                 Debug.Log("VP_Help");
                 Debug.Log("VP_GetCenter");
                 Debug.Log("VP_GetAssets name");
                 Debug.Log("VP_GetLevelInfo");
                 Debug.Log("VP_GetShader");
                 return "\n";
             }, "Get Help");
            Commands.RegisterCommand("VP_GetLevelInfo", (args, notUses) =>
            {
                GeoTools.GetLevelInfo();
                return "\n";
            }, "Get Level Info");
            Commands.RegisterCommand("VP_GetShader", (args, notUses) =>
            {
                GeoTools.PrintShader();
                return "\n";
            }, "Get Shader");
            Commands.RegisterCommand("VP_GetCenter", delegate (string[] args, IDictionary<string, string> notUses)
            {
                GeoTools.GetCenter();
                return "\n";
            }, "Get Machine Center");
            Commands.RegisterCommand("VP_GetAssets", delegate (string[] args, IDictionary<string, string> notUses)
            {
                if (args.Length > 0)
                {
                    Debug.Log("AssetBundleName:" + args[0]);
                    GeoTools.PrintAssetBundle(args[0]);
                }
                return "\n";
            }, "Get Assets Names");
        }
        void DoWindow(int windowID)
        {
            GUIStyle style = new GUIStyle
            {
                normal = { textColor = Color.white },
                alignment = TextAnchor.MiddleLeft,
                active = { background = Texture2D.whiteTexture, textColor = Color.black },
                margin = { top = 5 },
                fontSize = _FontSize
            };
            GUIStyle style1 = new GUIStyle
            {
                normal = { textColor = Color.white },
                alignment = TextAnchor.MiddleRight,
                active = { background = Texture2D.whiteTexture, textColor = Color.black },
                margin = { top = 5 },
                fontSize = _FontSize
            };

            GUILayout.BeginVertical(new GUILayoutOption[0]);

            if (_coordinatesUI.Length != 0)
            {
                GUILayout.BeginHorizontal(new GUILayoutOption[0]);
                if ((X.Length + Y.Length + Z.Length) < 13)
                {
                    GUILayout.Label(_coordinatesUI, style, new GUILayoutOption[0]);
                }
                GUILayout.Label(X + " / " + Y + " / " + Z, style1, new GUILayoutOption[0]);
                GUILayout.EndHorizontal();
            }
            if (_velocityUI.Length != 0)
            {
                GUILayout.BeginHorizontal(new GUILayoutOption[0]);
                GUILayout.Label(_velocityUI, style, new GUILayoutOption[0]);
                GUILayout.Label(V, style1, new GUILayoutOption[0]);
                GUILayout.EndHorizontal();
            }
            if (_distanceUI.Length != 0)
            {
                GUILayout.BeginHorizontal(new GUILayoutOption[0]);
                GUILayout.Label(_distanceUI + "(km)", style, new GUILayoutOption[0]);
                GUILayout.Label(Distance, style1, new GUILayoutOption[0]);
                GUILayout.EndHorizontal();
            }
            if (_overloadUI.Length != 0)
            {
                GUILayout.BeginHorizontal(new GUILayoutOption[0]);
                GUILayout.Label(_overloadUI + "(g)", style, new GUILayoutOption[0]);
                GUILayout.Label(Overload, style1, new GUILayoutOption[0]);
                GUILayout.EndHorizontal();
            }
            if (_timeUI.Length != 0)
            {
                GUILayout.BeginHorizontal(new GUILayoutOption[0]);
                GUILayout.Label(_timeUI, style, new GUILayoutOption[0]);
                GUILayout.Label(MTime, style1, new GUILayoutOption[0]);
                GUILayout.EndHorizontal();
            }
            if (_timerUI.Length != 0)
            {
                GUILayout.BeginHorizontal(new GUILayoutOption[0]);
                if (GUILayout.Button(_timerUI, style, new GUILayoutOption[0]))
                {
                    _TimerSwith = false; MTimer = "00:00:00";
                }
                if (GUILayout.Button("[" + MTimer + "]", style1, new GUILayoutOption[0]))
                {
                    _TimerSwith = !_TimerSwith;
                    if (_TimerSwith && MTimer == "00:00:00") { _MStartTime = DateTime.Now; }
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
            GUI.DragWindow(new Rect(0f, 0f, this.windowRect.width, this.windowRect.height));
        }
        void OnGUI()
        {
            if (ShowGUI)
            {
                this.windowRect = GUI.Window(this.windowID, this.windowRect, new GUI.WindowFunction(DoWindow), "", GUIStyle.none);
            }
        }
        void Update()
        {
            if (Input.GetKeyDown(_DisplayUI) && Input.GetKey(KeyCode.LeftControl))
            {
                ShowGUI = !ShowGUI;
            }
            if (Input.GetKeyDown(_ReloadUI) && Input.GetKey(KeyCode.LeftControl))
            {
                ReadUI();
            }
            if (Input.GetKeyDown(_GetLevelInfo) && Input.GetKey(KeyCode.LeftControl))
            {
                GeoTools.GetLevelInfo();
            }
            if (Input.GetKeyDown(_GetCenter) && Input.GetKey(KeyCode.LeftControl))
            {
                GeoTools.GetCenter();
            }
            if (Input.GetKeyDown(_GetShader) && Input.GetKey(KeyCode.LeftControl))
            {
                GeoTools.PrintShader();
            }
        }
        GameObject Dlight = null;
        bool LoadBlock()
        {
            try
            {
                startingBlock = GameObject.Find("StartingBlock");
                Dlight = GameObject.Find("Directional light");
            }
            catch { }
           
            if (startingBlock == null)
            {
                startingBlock = GameObject.Find("bgeL0");
                if (startingBlock == null)
                {
                    MyBlockInfo info = UnityEngine.Object.FindObjectOfType<MyBlockInfo>();
                    if (info != null)
                    {
                        startingBlock = info.gameObject;
                    }
                    if (startingBlock == null)
                    {
                        validBlock = false; return validBlock;
                    }
                    else
                    {
                        validBlock = true; return validBlock;
                    }
                }
                else
                {
                    validBlock = true; return validBlock;
                }
            }
            else
            {
                validBlock = true; return validBlock;
            }
        }
        void FixedUpdate()
        {
            if (!ShowGUI) return;
            if (StatMaster.isSimulating && isSimulating == false)
            {
                LoadBlock();
                if (validBlock) { _Position = startingBlock.GetComponent<Rigidbody>().position; }
                else { _Position = new Vector3(0, 0, 0); }
                isSimulating = true;
                Debug.Log("isSimulating:" + validBlock.ToString());
            }
            else if (!StatMaster.isSimulating && isSimulating == true)
            {
                _Distance = 0;
                Distance = "0";
                X = "0"; Y = "0"; Z = "0";
                V = "0";
                Distance = "0";
                Overload = "0";
                isSimulating = false;
                validBlock = false;

            }
            if (_coordinatesUI.Length != 0)
            {
                if (validBlock)
                {
                    float t1 = startingBlock.GetComponent<Rigidbody>().position.x;
                    float t2 = startingBlock.GetComponent<Rigidbody>().position.y;
                    float t3 = startingBlock.GetComponent<Rigidbody>().position.z;
                    X = string.Format("{0:N0}", t1);
                    Y = string.Format("{0:N0}", t2);
                    Z = string.Format("{0:N0}", t3);
                }
                else
                {
                    X = "0"; Y = "0"; Z = "0";
                }
            }
            if (_velocityUI.Length != 0)
            {
                if (_accstep == 25 || _accstep == 50)
                {
                    if (validBlock)
                    {
                        Vector3 v1 = startingBlock.GetComponent<Rigidbody>().velocity;
                        if (Unit == ModUnit.kmh) { V = string.Format("{0:N0}", v1.magnitude * 3.6f); }
                        else if (Unit == ModUnit.ms) { V = string.Format("{0:N0}", v1.magnitude); }
                        else if (Unit == ModUnit.mach) { V = string.Format("{0:N2}", v1.magnitude / 340f); }
                    }
                    else
                    {
                        V = "0";
                    }
                }
            }
            if (_distanceUI.Length != 0)
            {
                if (_accstep == 50 && Dlight!=null)
                {
                    float d2 = _Position.magnitude;
                    if (d2 <= 2500)
                    {
                        if (Dlight.GetComponent<Light>().shadowBias != 0.15f) Dlight.GetComponent<Light>().shadowBias = 0.15f;
                    }
                    if (d2 > 2500 && d2 <= 3500)
                    {
                        if (Dlight.GetComponent<Light>().shadowBias != 0.35f) Dlight.GetComponent<Light>().shadowBias = 0.35f;
                    }
                    else if (d2 > 3500 && d2 <= 4500)
                    {
                        if (Dlight.GetComponent<Light>().shadowBias != 0.55f) Dlight.GetComponent<Light>().shadowBias = 0.55f;
                    }
                    if (d2 > 4500 && d2 <= 5500)
                    {
                        if (Dlight.GetComponent<Light>().shadowBias != 0.75f) Dlight.GetComponent<Light>().shadowBias = 0.75f;
                    }
                    else if (d2 > 5500)
                    {
                        if (Dlight.GetComponent<Light>().shadowBias != 0.95f) Dlight.GetComponent<Light>().shadowBias = 0.95f;
                    }
                }
                if (_accstep == 10 || _accstep == 20 || _accstep == 30 || _accstep == 40 || _accstep == 50)
                {
                    if (validBlock)
                    {
                        Distance = string.Format("{0:N2}", _Distance / 1000f);
                        Vector3 v2 = _Position - startingBlock.GetComponent<Rigidbody>().position;
                        _Distance += v2.magnitude;
                        _Position = startingBlock.GetComponent<Rigidbody>().position;
                    }
                    else
                    {
                        Distance = "0";
                    }
                }
            }
            if (_overloadUI.Length != 0)
            {
                if (_accstep == 25 || _accstep == 50)
                {
                    if (validBlock)
                    {
                        Vector3 v1 = startingBlock.GetComponent<Rigidbody>().velocity;
                        float _overload = 0;
                        float timedomain = Time.fixedDeltaTime * 25f;
                        if (timedomain > 0) _overload =
                                   Vector3.Dot((_V - v1), base.transform.up) / timedomain / 38.5f + Vector3.Dot(Vector3.up, base.transform.up) - 1;
                        _V = v1;
                        Overload = string.Format("{0:N2}", _overload);
                    }
                    else
                    {
                        Overload = "0";
                    }
                }
            }
            if (_timeUI.Length != 0)
            {
                if (_accstep == 50)
                {
                    MTime = System.DateTime.Now.ToString("HH:mm:ss");
                }
            }
            if (_timerUI.Length != 0)
            {
                if (_accstep == 7 || _accstep == 14 || _accstep == 21 || _accstep == 28 || _accstep == 35 || _accstep == 42 || _accstep == 49)
                {
                    if (_TimerSwith)
                    {
                        TimeSpan span = DateTime.Now - _MStartTime;
                        DateTime n = new DateTime(span.Ticks);
                        MTimer = n.ToString("mm:ss:ff");
                    }
                }

            }
            if (_accstep >= 50) { _accstep = 0; }
            _accstep++;
        }
    }
    /////////////////////////
}

