using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BesiegeCustomScene.UI
{
    class ToolBoxSettingUI : MonoBehaviour
    {
       

        struct ToolBoxUI_Language
        {
            public string _ToolBoxWindowUI;
            public string _PositionUI;
            public string _VelocityUI;
            public string _DistanceUI;
            public string _AccelerationUI;
            public string _OverloadUI;
            public string _TimeUI;
            public string _TimerUI;
            public string _TriggerUI;

            public static ToolBoxUI_Language DefaultLanguage { get; } = new ToolBoxUI_Language
            {
                _ToolBoxWindowUI = "Tool Box",
                _PositionUI = "Position",
                _VelocityUI = "Velocity",
                _DistanceUI = "Distance",
                _AccelerationUI = "Acceleration",
                _OverloadUI = "Overload",
                _TimeUI = "Time",
                _TimerUI = "Timer",
                _TriggerUI = "Trigger"
            };
        }

        bool ShowGUI;

        int windowID = GeoTools.GetWindowID();

        Rect windowRect = new Rect(15f, 100f, 180f, 200f);

        ToolBoxUI_Language toolBoxUI_Language;



        event Click VelocityButtonClickEvent;
        event Click RetimeButtonClickEvent;
        event Click TimerButtonClickEvent;
        event Click TriggerButtonClickEvent;

        //int triggerIndex;
        //int triggerSize;

        TimerMod timerMod;
        //TriggerMod triggerMod;
        BlockInformationMod blockInformationMod;

        void Start()
        {
            ShowGUI = true;

            initLanguage();
            initEvent();

            timerMod = gameObject.AddComponent<TimerMod>();
            //triggerMod = gameObject.AddComponent<TriggerMod>();
            blockInformationMod = gameObject.AddComponent<BlockInformationMod>();
        }

        void initLanguage()
        {
            LanguageFile currentLanuage = GetComponent<LanguageManager>().Get_CurretLanguageFile();

            toolBoxUI_Language = ToolBoxUI_Language.DefaultLanguage;
            if (currentLanuage != null)
            {
                Dictionary<int, string> translation = currentLanuage.dic_Translation;
                toolBoxUI_Language._ToolBoxWindowUI = translation[200];
                toolBoxUI_Language._PositionUI = translation[204];
                toolBoxUI_Language._VelocityUI = translation[205];
                toolBoxUI_Language._AccelerationUI = translation[201];
                toolBoxUI_Language._OverloadUI = translation[202];
                toolBoxUI_Language._DistanceUI = translation[206];
                toolBoxUI_Language._TimeUI = translation[203];
                toolBoxUI_Language._TimerUI = translation[207];
                toolBoxUI_Language._TriggerUI = translation[208];
            }
        }
        void initEvent()
        {
            VelocityButtonClickEvent += () => 
            {
#if DEBUG
                BesiegeConsoleController.ShowMessage("velocity button click event ");
#endif
            };
            RetimeButtonClickEvent += () => 
            {
#if DEBUG
                BesiegeConsoleController.ShowMessage("retime button click event ");
#endif
            };
            TimerButtonClickEvent += () =>
            {
#if DEBUG
                BesiegeConsoleController.ShowMessage("timer button click event ");
#endif
            };
            TriggerButtonClickEvent += () => 
            {
#if DEBUG
                BesiegeConsoleController.ShowMessage("trigger button click event ");
#endif
            };
        }

        void OnGUI()
        {
            if (ShowGUI && GeoTools.isBuilding())
            {
                windowRect = GUI.Window(windowID, windowRect, new GUI.WindowFunction(TimerWindow),toolBoxUI_Language._ToolBoxWindowUI);
            }
        }

        void TimerWindow(int windowID)
        {
          

            GUILayout.BeginVertical(new GUILayoutOption[0]);
            {
                GUILayout.BeginHorizontal(new GUILayoutOption[0]);
                {
                    GUILayout.Label(toolBoxUI_Language._PositionUI);
                    GUILayout.Label(blockInformationMod.Position.ToString());
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal(new GUILayoutOption[0]);
                {
                    //GUILayout.Label(string.Format("{0} {1}" ,toolBoxUI_Language._VelocityUI,blockInformationMod.velocityUnit));
                    if (GUILayout.Button(toolBoxUI_Language._VelocityUI))
                    {
                        blockInformationMod.changedVelocityUnit();
                        VelocityButtonClickEvent();
                    }

                    GUILayout.Label(string.Format("{0} {1}", blockInformationMod.Velocity.magnitude.ToString("#0.00"), blockInformationMod.velocityUnit));
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal(new GUILayoutOption[0]);
                {
                    GUILayout.Label(toolBoxUI_Language._AccelerationUI);
                    GUILayout.Label(blockInformationMod.Acceleration.ToString("#0.00"));
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal(new GUILayoutOption[0]);
                {
                    GUILayout.Label(toolBoxUI_Language._DistanceUI);
                    GUILayout.Label(string.Format("{0:N2}", blockInformationMod.Distance / 1000f));
                }
                GUILayout.EndHorizontal();


                GUILayout.BeginHorizontal(new GUILayoutOption[0]);
                {
                    GUILayout.Label(toolBoxUI_Language._OverloadUI);
                    GUILayout.Label(string.Format("{0}(g)", blockInformationMod.Overload.ToString("#0.00")));
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal(new GUILayoutOption[0]);
                {
                    GUILayout.Label(toolBoxUI_Language._TimeUI);
                    GUILayout.Label(timerMod.CurrentSystemTime);
                }
                GUILayout.EndHorizontal();


                GUILayout.BeginHorizontal(new GUILayoutOption[0]);
                {
                    if (GUILayout.Button(toolBoxUI_Language._TimerUI))
                    {
                        //timerSwitch = false; Timer = "00:00:00";
                        timerMod.Retime();
                        RetimeButtonClickEvent();
                    }
                    if (GUILayout.Button(timerMod.CurrentTimerTime))
                    {
                        //timerSwitch = !timerSwitch;
                        //if (timerSwitch && Timer == "00:00:00") { startTime = DateTime.Now; }

                        timerMod.TimeSwitch = !timerMod.TimeSwitch;         
                        TimerButtonClickEvent();
                    }
                    GUILayout.EndHorizontal();
                }

                //GUILayout.BeginHorizontal(new GUILayoutOption[0]);
                //{
                //    GUILayout.Label(toolBoxUI_Language._TriggerUI);
                //    if (GUILayout.Button(string.Format("{0}/{1}", (triggerMod.Index + 1).ToString(), triggerMod.Size)))
                //    {
                //        //triggerIndex = -1;
                //        //TriggerIndex2 = -1;
                //        TriggerButtonClickEvent();
                //    }
                //}
                //GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();

            GUI.DragWindow();
        }
                   
        GUIStyle style = new GUIStyle
        {
            normal = { textColor = Color.white },
            alignment = TextAnchor.MiddleLeft,
            active = { background = Texture2D.whiteTexture, textColor = Color.black },
            margin = { top = 5 },
            fontSize = 15
        };
        GUIStyle style1 = new GUIStyle
        {
            normal = { textColor = Color.white },
            alignment = TextAnchor.MiddleRight,
            active = { background = Texture2D.whiteTexture, textColor = Color.black },
            margin = { top = 5 },
            fontSize = 15
        };
    }
}
