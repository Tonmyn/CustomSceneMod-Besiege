using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BesiegeCustomScene
{
    public class TriggerUI : MonoBehaviour
    {
        void Start()
        {
            ReadUI();
        }
        void OnDisable()
        {
            ClearTrigger();
        }
        void OnDestroy()
        {
            ClearTrigger();
        }
        void OnGUI()
        {
            if (ShowGUI)
            {
                this.windowRect = GUI.Window(this.windowID, this.windowRect, new GUI.WindowFunction(DoWindow), "", GUIStyle.none);
            }
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

            if (trigger.Length != 0)
            {
                GUILayout.BeginHorizontal(new GUILayoutOption[0]);
                if (GUILayout.Button(trigger, style, new GUILayoutOption[0]))
                {
                    TriggerIndex = -1; TriggerIndex2 = -1;
                }                     
                GUILayout.Label((TriggerIndex + 1).ToString() + "/"+ TriggerSize.ToString(), style1, new GUILayoutOption[0]);       
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
            GUI.DragWindow(new Rect(0f, 0f, this.windowRect.width, this.windowRect.height));
        }
        void FixedUpdate()
        {
            if (!ShowGUI) return;
            if (StatMaster.isSimulating)
            {
                if (TriggerIndex == 0 && TriggerIndex2 == -1)
                {
                    if (this.gameObject.GetComponent<TimeUI>()._TimerSwith == false)
                    {
                        this.gameObject.GetComponent<TimeUI>()._TimerSwith = true;
                        this.gameObject.GetComponent<TimeUI>()._MStartTime = DateTime.Now;
                    }
                }
                if (TriggerIndex == meshtriggers.Length - 1 && TriggerIndex2 != TriggerIndex)
                {
                    this.gameObject.GetComponent<TimeUI>()._TimerSwith = false;
                }
                TriggerIndex2 = TriggerIndex;
            }
            else
            {
                TriggerIndex = -1; TriggerIndex2 = -1;
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
        }
        ////////////////////////////
        private GameObject[] meshtriggers;
        public static int TriggerIndex = -1;
        int TriggerIndex2 = -1;//record the prevail statement of triggerindex
        private int TriggerSize = 0;
        public bool ShowGUI = false;
        private int _FontSize = 15;
        KeyCode _DisplayUI = KeyCode.F10;
        KeyCode _ReloadUI = KeyCode.F5;
        string trigger = "[Trigger]";
        private int windowID = spaar.ModLoader.Util.GetWindowID();
        private Rect windowRect = new Rect(15f, 234f, 150f, 50f);
        void DefaultUI()
        {
            trigger = "[Trigger]";
            _FontSize = 15;
            ShowGUI = false;
            windowRect = new Rect(15f, 234f, 150f, 50f);
            _DisplayUI = KeyCode.F10;
            _ReloadUI = KeyCode.F5;
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
                        if (chara[0] == "_Trigger")
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
                            else if (chara[1] == "trigger")
                            {
                                if (chara[2] == "OFF") trigger = string.Empty;
                                else trigger = chara[2];
                            }
                        }
                        else if (chara[0] == Screen.width.ToString() + "*" + Screen.height.ToString() + "_Trigger")
                        {
                            if (chara[1] == "fontsize")
                            {
                                _FontSize = Convert.ToInt32(chara[2]);
                            }
                            else if (chara[1] == "window_poistion")
                            {
                                windowRect.x = Convert.ToSingle(chara[2]);
                                windowRect.y = Convert.ToSingle(chara[3]);
                            }
                            else if (chara[1] == "window_scale")
                            {
                                windowRect.width = Convert.ToSingle(chara[2]);
                                windowRect.height = Convert.ToSingle(chara[3]);
                                windowRect.y += windowRect.height + 10;
                            }
                            else if (chara[1] == "show_on_start")
                            {
                                if (chara[2] == "0") ShowGUI = false;
                                else ShowGUI = true;
                            }
                        }
                    }
                }
                srd.Close();
                Debug.Log("TriggerUISetting Completed!");
            }
            catch (Exception ex)
            {
                Debug.Log("TriggerUISetting Failed!");
                Debug.Log(ex.ToString());
                DefaultUI();
                return;
            }
        }
        string ScenePath = GeoTools.ScenePath;
        public void ReadScene(string SceneName)
        {
            try
            {
                Debug.Log(Application.dataPath);
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
                        #region Triggers
                        if (chara[0] == "Triggers")
                        {
                            if (chara[1] == "size")
                            {
                                this.TriggerSize = Convert.ToInt32(chara[2]);
                                LoadTrigger();
                            }
                        }
                        else if (chara[0] == "Trigger")
                        {
                            int i = Convert.ToInt32(chara[1]);
                            if (chara[2] == "mesh")
                            {
                                meshtriggers[i].GetComponent<MeshFilter>().mesh = GeoTools.MeshFromObj(chara[3]);
                            }
                            else if (chara[2] == "wmesh")
                            {
                                meshtriggers[i].GetComponent<MeshFilter>().mesh = GeoTools.WMeshFromObj(chara[3]);
                            }
                            else if (chara[2] == "scale")
                            {
                                meshtriggers[i].transform.localScale = new Vector3(
                               Convert.ToSingle(chara[3]),
                               Convert.ToSingle(chara[4]),
                               Convert.ToSingle(chara[5]));
                            }
                            else if (chara[2] == "location")
                            {
                                meshtriggers[i].transform.localPosition = new Vector3(
                                Convert.ToSingle(chara[3]),
                                Convert.ToSingle(chara[4]),
                                Convert.ToSingle(chara[5]));
                            }
                            else if (chara[2] == "rotation")
                            {
                                meshtriggers[i].transform.rotation = new Quaternion(
                                Convert.ToSingle(chara[3]),
                                Convert.ToSingle(chara[4]),
                                Convert.ToSingle(chara[5]),
                                Convert.ToSingle(chara[6]));
                            }
                            else if (chara[2] == "eulerangles")
                            {
                                meshtriggers[i].transform.Rotate(new Vector3(
                                Convert.ToSingle(chara[3]),
                                Convert.ToSingle(chara[4]),
                                Convert.ToSingle(chara[5])), Space.Self);
                            }
                            else if (chara[2] == "euleranglesworld")
                            {
                                meshtriggers[i].transform.Rotate(new Vector3(
                                Convert.ToSingle(chara[3]),
                                Convert.ToSingle(chara[4]),
                                Convert.ToSingle(chara[5])), Space.World);
                            }
                            else if (chara[2] == "fromtorotation")
                            {
                                meshtriggers[i].transform.rotation = Quaternion.FromToRotation(
                              new Vector3(Convert.ToSingle(chara[3]),
                                Convert.ToSingle(chara[4]),
                                Convert.ToSingle(chara[5])),
                                new Vector3(Convert.ToSingle(chara[6]),
                                Convert.ToSingle(chara[7]),
                                Convert.ToSingle(chara[8]))
                                );
                            }
                            else if (chara[2] == "shader")
                            {
                                meshtriggers[i].GetComponent<MeshRenderer>().material.shader = Shader.Find("chara[3]");
                            }
                            else if (chara[2] == "texture")
                            {
                                meshtriggers[i].GetComponent<MeshRenderer>().material.mainTexture = GeoTools.LoadTexture(chara[3]);
                            }
                            else if (chara[2] == "stexture")
                            {
                                meshtriggers[i].GetComponent<MeshRenderer>().sharedMaterial.mainTexture = GeoTools.LoadTexture(chara[3]);
                            }
                            else if (chara[2] == "color")
                            {
                                meshtriggers[i].GetComponent<MeshRenderer>().material.color = new Color(
                                Convert.ToSingle(chara[3]),
                                Convert.ToSingle(chara[4]),
                                Convert.ToSingle(chara[5]),
                                Convert.ToSingle(chara[6]));
                            }
                            else if (chara[2] == "meshcollider")
                            {
                                meshtriggers[i].GetComponent<MeshCollider>().sharedMesh = GeoTools.MeshFromObj(chara[3]);
                                meshtriggers[i].GetComponent<MeshCollider>().convex = true;
                                meshtriggers[i].GetComponent<MeshCollider>().isTrigger = true;
                            }
                            else if (chara[2] == "wmeshcollider")
                            {
                                meshtriggers[i].GetComponent<MeshCollider>().sharedMesh = GeoTools.WMeshFromObj(chara[3]);
                                meshtriggers[i].GetComponent<MeshCollider>().convex = true;
                                meshtriggers[i].GetComponent<MeshCollider>().isTrigger = true;
                            }
                            ///////////////////////////////////////////////
                        }
                        #endregion
                    }
                }
                srd.Close();
                Debug.Log("ReadMeshTrigger Completed!");
            }
            catch (Exception ex)
            {
                Debug.Log("ReadMeshTrigger Failed!");
                Debug.Log(ex.ToString());
                return;
            }
        }
        public void LoadTrigger()
        {
            try
            {
                ClearTrigger();
                if (TriggerSize > 100) TriggerSize = 100;
                if (TriggerSize < 0) TriggerSize = 0;
                if (TriggerSize > 0)
                {
                    meshtriggers = new GameObject[TriggerSize];
                    for (int i = 0; i < meshtriggers.Length; i++)
                    {
                        meshtriggers[i] = GameObject.CreatePrimitive(PrimitiveType.Plane);
                        meshtriggers[i].GetComponent<MeshCollider>().sharedMesh.Clear();
                        meshtriggers[i].GetComponent<MeshFilter>().mesh.Clear();
                        meshtriggers[i].AddComponent<MTrigger>();
                        meshtriggers[i].GetComponent<MTrigger>().Index = i;
                        meshtriggers[i].name = "_meshtrigger" + i.ToString();
                    }
                }
            }
            catch (System.Exception ex)
            {
                Debug.Log("LoadTrigger Failed!");
                Debug.Log(ex.ToString());
            }
        }
        public void ClearTrigger()
        {
            if (meshtriggers == null) return;
            if (meshtriggers.Length <= 0) return;
            Debug.Log("ClearMeshTriggers");
            for (int i = 0; i < meshtriggers.Length; i++)
            {
                Destroy(meshtriggers[i]);
            }
        }
    }
}