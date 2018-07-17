using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BesiegeCustomScene.UI
{
    class ToolBoxSettingUI : MonoBehaviour
    {
       

        struct TimerUI_Language
        {
            public string _TimerWindowUI;
            public string _PositionUI;
            public string _VelocityUI;
            public string _DistanceUI;
            public string _AccelerationUI;
            public string _OverloadUI;
            public string _TimeUI;
            public string _TimerUI;
            public string _TriggerUI;

            public static TimerUI_Language DefaultLanguage { get; } = new TimerUI_Language
            {
                _TimerWindowUI = "Timer",
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

        Rect windowRect = new Rect(15f, 100f, 150f, 150f);

        TimerUI_Language timerUI_Language;



        event Click VelocityButtonClickEvent;
        event Click RetimeButtonClickEvent;
        event Click TimerButtonClickEvent;
        event Click TriggerButtonClickEvent;

        int triggerIndex;
        int triggerSize;

        TimerMod timerMod;

        BlockInformationMod blockInformationMod;

        void Start()
        {
            initLanguage();
            initEvent();

            timerMod = gameObject.AddComponent<TimerMod>();
            blockInformationMod = gameObject.AddComponent<BlockInformationMod>();
        }

        void initLanguage()
        {
            LanguageManager.LanguageFile currentLanuage = GetComponent<LanguageManager>().Get_CurretLanguageFile();

            timerUI_Language = TimerUI_Language.DefaultLanguage;
            if (currentLanuage != null)
            {
                Dictionary<int, string> translation = currentLanuage.dic_Translation;
                timerUI_Language._TimerWindowUI = translation[200];
                timerUI_Language._PositionUI = translation[204];
                timerUI_Language._VelocityUI = translation[205];
                timerUI_Language._AccelerationUI = translation[201];
                timerUI_Language._OverloadUI = translation[202];
                timerUI_Language._DistanceUI = translation[206];
                timerUI_Language._TimeUI = translation[203];
                timerUI_Language._TimerUI = translation[207];
                timerUI_Language._TriggerUI = translation[208];
            }
        }
        void initEvent()
        {
            VelocityButtonClickEvent += () => { };
            RetimeButtonClickEvent += () => { };
            TimerButtonClickEvent += () => { };
            TriggerButtonClickEvent += () => { };
        }

        void OnGUI()
        {
            if (ShowGUI && GeoTools.isBuilding())
            {
                windowRect = GUI.Window(windowID, windowRect, new GUI.WindowFunction(TimerWindow),"");
            }
        }

        void TimerWindow(int windowID)
        {
          

            GUILayout.BeginVertical(new GUILayoutOption[0]);
            {
                GUILayout.BeginHorizontal(new GUILayoutOption[0]);
                {
                    GUILayout.Label(timerUI_Language._PositionUI);
                    GUILayout.Label(blockInformationMod.Position);
                }
                GUILayout.EndHorizontal();

                //GUILayout.BeginHorizontal(new GUILayoutOption[0]);
                //{
                //    GUILayout.Label(timerUI_Language._VelocityUI);
                //    GUILayout.Label(targetRigidbody.velocity.magnitude.ToString());
                //}
                //GUILayout.EndHorizontal();

                //GUILayout.BeginHorizontal(new GUILayoutOption[0]);
                //{
                //    GUILayout.Label(timerUI_Language._DistanceUI);
                //    GUILayout.Label(Distance.ToString());
                //}
                //GUILayout.EndHorizontal();


                //GUILayout.BeginHorizontal(new GUILayoutOption[0]);
                //{
                //    GUILayout.Label(timerUI_Language._OverloadUI + "(g)");
                //    GUILayout.Label(Overload.ToString());
                //}
                //GUILayout.EndHorizontal();

                //GUILayout.BeginHorizontal(new GUILayoutOption[0]);
                //{
                //    GUILayout.Label(timerUI_Language._TimeUI);
                //    GUILayout.Label(Time);
                //}
                //GUILayout.EndHorizontal();


                GUILayout.BeginHorizontal(new GUILayoutOption[0]);
                {
                    if (GUILayout.Button(timerUI_Language._TimerUI))
                    {
                        //timerSwitch = false; Timer = "00:00:00";
                        RetimeButtonClickEvent();
                    }
                    //if (GUILayout.Button("[" + Timer + "]"))
                    //{
                    //    //timerSwitch = !timerSwitch;
                    //    //if (timerSwitch && Timer == "00:00:00") { startTime = DateTime.Now; }
                    //    TimerButtonClickEvent();
                    //}
                    GUILayout.EndHorizontal();
                }

                GUILayout.BeginHorizontal(new GUILayoutOption[0]);
                {
                    GUILayout.Label(timerUI_Language._TriggerUI);
                    if (GUILayout.Button("[" + (triggerIndex + 1).ToString() + "/" + triggerSize.ToString() + "]"))
                    {
                        //triggerIndex = -1;
                        //TriggerIndex2 = -1;
                        TriggerButtonClickEvent();
                    }
                }
                GUILayout.EndHorizontal();
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
