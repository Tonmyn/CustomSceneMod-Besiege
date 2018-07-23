using System;
using System.Collections.Generic;
using UnityEngine;

namespace BesiegeCustomScene.UI
{
    class SceneSettingUI : MonoBehaviour
    {
        /// <summary>地图包列表</summary>
        //public List<SceneMod.ScenePack> scenePacks;

        public CustomSceneMod sceneMod;

        public bool ShowGUI = true;

        public KeyCode DisplaySceneSettingKey = KeyCode.F9;

        public delegate void SceneButtonClickHandler(int index);
        /// <summary>地形按钮点击事件  按键序号</summary>
        public event SceneButtonClickHandler OnSceneButtonClick;
        /// <summary>去雾按钮点击事件</summary>
        public event VoidDel OnFogButtonClick;
        /// <summary>去地面按钮点击事件</summary>
        public event VoidDel OnFloorButtonClick;
        /// <summary>去空气墙按钮点击事件</summary>
        public event VoidDel OnWorldBoundsButtonClick;
        /// <summary>重新加载地形按钮点击事件</summary>
        public event VoidDel OnReloadScenesButtonClick;

        int windowID = GeoTools.GetWindowID();
        Vector2 scrollVector = Vector2.zero;
        Rect windowRect = new Rect(Screen.width * 0.05f, Screen.height * 0.5f, 280, 300f);
        /// <summary>地图按钮高度</summary>
        int buttonHeight = 20;
        /// <summary>地图滑动区域</summary>
        Rect sceneButtonsRect;

        SceneUI_Language sceneUI_Language;

        public struct SceneUI_Language
        {
            public string _SceneWindowUI;
            public string _FogUI;
            public string _WorldBoundsUI;
            public string _FloorGridUI;
            public string _SceneList;

            public static SceneUI_Language DefaultLanguage { get; } = new SceneUI_Language
            {
                _SceneWindowUI = "Scene Setting",
                _FogUI = "Remove Fog",
                _WorldBoundsUI = "Remove World Bounds",
                _FloorGridUI = "Remove Floor",
                _SceneList = "Scene List:",
            };
        }



        void Awake()
        {
            initSceneMod();

            LanguageManager.LanguageFile currentLanuage;
            currentLanuage = GetComponent<LanguageManager>().Get_CurretLanguageFile();

            float height = (sceneMod.ScenePacks.Count - 1) * (buttonHeight + 5) + 5;
            sceneButtonsRect = new Rect(0, 0, 200, height);

            sceneUI_Language = SceneUI_Language.DefaultLanguage;
            if (currentLanuage != null)
            {
                Dictionary<int, string> translation = currentLanuage.dic_Translation;
                sceneUI_Language._SceneWindowUI = translation[100];
                sceneUI_Language._FogUI = translation[101];
                sceneUI_Language._WorldBoundsUI = translation[102];
                sceneUI_Language._FloorGridUI = translation[103];
                sceneUI_Language._SceneList = translation[104];
            }

        }

        void initSceneMod()
        {
            sceneMod = new GameObject("Custom Scene Mod").AddComponent<CustomSceneMod>();

            sceneMod.transform.SetParent(transform);

            //sceneMod = gameObject.GetComponent<CustomSceneMod>() ?? gameObject.AddComponent<CustomSceneMod>();

            OnSceneButtonClick += sceneMod.LoadScenePack;

            OnFogButtonClick += sceneMod.HideFog;

            OnFloorButtonClick += sceneMod.HideFloorBig;

            OnWorldBoundsButtonClick += sceneMod.HideWorldBoundaries;
        }

        private void Update()
        {
            if (Input.GetKeyDown(DisplaySceneSettingKey) && Input.GetKey(KeyCode.LeftControl))
            {
                ShowGUI = !ShowGUI;
            }
        }

        private void OnGUI()
        {
            
            if (!StatMaster.levelSimulating && ShowGUI && GeoTools.isBuilding() && !StatMaster.inMenu)
            {
                windowRect = GUI.Window(windowID, windowRect, new GUI.WindowFunction(SceneWindow), sceneUI_Language._SceneWindowUI);
            }
                    
        }

        void SceneWindow(int ID)
        {

            GUILayout.BeginHorizontal();
            {
                if (GUI.Button(new Rect(10, 20, 80, 20), sceneUI_Language._FogUI)) { OnFogButtonClick(); }
                if (GUI.Button(new Rect(100, 20, 80, 20), sceneUI_Language._WorldBoundsUI)) { OnWorldBoundsButtonClick(); }
                if (GUI.Button(new Rect(190, 20, 80, 20), sceneUI_Language._FloorGridUI)) { OnFloorButtonClick(); }
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginVertical();
            {
                GUI.Label(new Rect(10, 50, 280, 20), sceneUI_Language._SceneList);
                GUI.Box(new Rect(10, 70, 260, 220), "");

                scrollVector = GUI.BeginScrollView(new Rect(15, 75, 250, 210), scrollVector, sceneButtonsRect);
                {
                    GUILayout.BeginArea(new Rect(0, 0, 250, sceneButtonsRect.height));
                    {
                        for (int i = 0; i < sceneMod.ScenePacks.Count; i++)
                        {
                            if (GUILayout.Button(sceneMod.ScenePacks[i].Name, GUILayout.Width(230), GUILayout.Height(20)))
                            {
                                //LoadScene(i);
                                OnSceneButtonClick(i);
                            }
                        }
                    }
                    GUILayout.EndArea();
                }
                GUI.EndScrollView();
            }
            GUILayout.EndVertical();                 

            GUI.DragWindow();
        }

        GUIStyle windowStyle = new GUIStyle()
        {
            fontStyle = FontStyle.Bold,
            fontSize = 20,
            normal = { textColor = Color.red },
            alignment = TextAnchor.UpperLeft,

        };
    
    }
}
