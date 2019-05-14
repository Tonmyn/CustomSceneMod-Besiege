using System.Collections.Generic;
using UnityEngine;
using Modding;

namespace CustomScene.UI
{
    class ToolBoxSettingUI : MonoBehaviour
    {
        bool ShowGUI;
        //readonly KeyCode DisplayToolBoxKey = KeyCode.F8;
        ModKey DisplayToolBoxKey = ModKeys.GetKey("Tool Box SettingUI-key");
        readonly int windowID = GeoTools.GetWindowID();

        Rect windowRect = new Rect(15f, 100f, 180f, 200f);

        event Click VelocityButtonClickEvent;
        event Click RetimeButtonClickEvent;
        event Click TimerButtonClickEvent;

        TimerMod timerMod;
        BlockInformationMod blockInformationMod;

        void Start()
        {
            ShowGUI = true;
            InitEvent();

            GameObject go = new GameObject("Timer Mod");
            go.transform.SetParent(transform);
            timerMod = go.GetComponent<TimerMod>();

            go = new GameObject("Block Information Mod");
            go.transform.SetParent(transform);
            blockInformationMod = go.GetComponent<BlockInformationMod>();

        }

        void InitEvent()
        {
            VelocityButtonClickEvent += () => 
            {
#if DEBUG
                ConsoleController.ShowMessage("velocity button click event ");
#endif
            };
            RetimeButtonClickEvent += () => 
            {
#if DEBUG
                ConsoleController.ShowMessage("retime button click event ");
#endif
            };
            TimerButtonClickEvent += () =>
            {
#if DEBUG
                ConsoleController.ShowMessage("timer button click event ");
#endif
            };
        }

        private void Update()
        {
            if (DisplayToolBoxKey.IsPressed)
            {
                ShowGUI = !ShowGUI;
            }
        }

        void OnGUI()
        {
            if (ShowGUI && GeoTools.IsBuilding())
            {
                windowRect = GUI.Window(windowID, windowRect, new GUI.WindowFunction(TimerWindow), LanguageManager.BlockInformationTitle);
            }
        }

        void TimerWindow(int windowID)
        {
          

            GUILayout.BeginVertical(new GUILayoutOption[0]);
            {
                GUILayout.BeginHorizontal(new GUILayoutOption[0]);
                {
                    GUILayout.Label(LanguageManager.PositionLabel);
                    GUILayout.Label(blockInformationMod.Position.ToString());
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal(new GUILayoutOption[0]);
                {
                    if (GUILayout.Button(LanguageManager.VelocityLabel))
                    {
                        blockInformationMod.ChangedVelocityUnit();
                        VelocityButtonClickEvent();
                    }

                    GUILayout.Label(string.Format("{0} {1}", blockInformationMod.Velocity.magnitude.ToString("#0.00"), blockInformationMod.velocityUnit));
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal(new GUILayoutOption[0]);
                {
                    GUILayout.Label(LanguageManager.AccelerationLabel);
                    GUILayout.Label(blockInformationMod.Acceleration.ToString("#0.00"));
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal(new GUILayoutOption[0]);
                {
                    GUILayout.Label(LanguageManager.DistanceLabel);
                    GUILayout.Label(string.Format("{0:N2}", blockInformationMod.Distance / 1000f));
                }
                GUILayout.EndHorizontal();


                GUILayout.BeginHorizontal(new GUILayoutOption[0]);
                {
                    GUILayout.Label(LanguageManager.GForceLabel);
                    GUILayout.Label(string.Format("{0}(g)", blockInformationMod.Overload.ToString("#0.00")));
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal(new GUILayoutOption[0]);
                {
                    GUILayout.Label(LanguageManager.TimeLabel);
                    GUILayout.Label(timerMod.CurrentSystemTime);
                }
                GUILayout.EndHorizontal();


                GUILayout.BeginHorizontal(new GUILayoutOption[0]);
                {
                    if (GUILayout.Button(LanguageManager.TimerLabel))
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
                //GUILayout.Label(LanguageManager.Trigger);
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

        readonly GUIStyle style = new GUIStyle
        {
            normal = { textColor = Color.white },
            alignment = TextAnchor.MiddleLeft,
            active = { background = Texture2D.whiteTexture, textColor = Color.black },
            margin = { top = 5 },
            fontSize = 15
        };
        readonly GUIStyle style1 = new GUIStyle
        {
            normal = { textColor = Color.white },
            alignment = TextAnchor.MiddleRight,
            active = { background = Texture2D.whiteTexture, textColor = Color.black },
            margin = { top = 5 },
            fontSize = 15
        };
    }
}
