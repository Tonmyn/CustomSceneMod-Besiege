using System;
using System.Collections.Generic;
using UnityEngine;
using Modding;
using System.Reflection;

namespace CustomScene.UI
{
    class SceneSettingUI : MonoBehaviour
    {
        /// <summary>地图包列表</summary>
        public CustomSceneMod sceneMod;

        public bool showGUI = true;

        //public KeyCode DisplaySceneSettingKey = KeyCode.F9;

        public ModKey DisplaySceneSettingKey = ModKeys.GetKey("Scene SettingUI-key");

        /// <summary>地形按钮点击事件  按键序号</summary> 
        public Action<int> OnSceneButtonClick;
        /// <summary>去雾按钮点击事件</summary>
        public Action OnFogButtonClick;
        /// <summary>去地面按钮点击事件</summary>
        public Action OnFloorButtonClick;
        /// <summary>去空气墙按钮点击事件</summary>
        public Action OnWorldBoundsButtonClick;
        /// <summary>重新加载地形按钮点击事件</summary>
        public Action OnReloadScenesButtonClick;
        /// <summary>打开地图包文件夹按钮点击事件</summary>
        public Action OnOpenScenePacksDirectoryButtonClick;

        readonly int windowID = GeoTools.GetWindowID();

        Vector2 scrollVector = Vector2.zero;
        Rect windowRect = new Rect(Screen.width * 0.05f, Screen.height * 0.5f, 280, 300f+25f);

        /// <summary>地图按钮高度</summary>
        readonly int buttonHeight = 20;

        /// <summary>地图滑动区域</summary>
        Rect sceneButtonsRect;

        void Awake()
        {
            InitSceneMod();

            float height = (sceneMod.ScenePacks.Count - 1 + 1) * (buttonHeight + 5) + 5 - 5;

            sceneButtonsRect = new Rect(0, 0, 200, height);
        }

        void InitSceneMod()
        {
            //sceneMod = gameObject.AddComponent<CustomSceneMod>();

            //OnSceneButtonClick += sceneMod.LoadScenePack;

            //OnFogButtonClick += sceneMod.HideFog;

            //OnFloorButtonClick += sceneMod.HideFloorBig;

            //OnWorldBoundsButtonClick += sceneMod.HideWorldBoundaries;

            //OnReloadScenesButtonClick += sceneMod.ReloadScenePacks;

            //OnOpenScenePacksDirectoryButtonClick += sceneMod.OpenScenesDirectory;

            //GameObject go; 
            //go = new GameObject("Camera Mod");
            //go.AddComponent<CameraMod>().transform.SetParent(transform);
            //go = new GameObject("Mesh Mod");
            //go.AddComponent<MeshMod>().transform.SetParent(transform);
            ////go = new GameObject("Snow Mod");
            ////go.AddComponent<SnowMod>().transform.SetParent(transform);
            ////go = new GameObject("Cloud Mod");
            ////go.AddComponent<CloudMod>().transform.SetParent(transform);
            //go = new GameObject("Water Mod");
            //go.AddComponent<WaterMod>().transform.SetParent(transform);
            //go = new GameObject("Sky Mod");
            //go.AddComponent<SkyMod>().transform.SetParent(transform);
        }

        private void Update()
        {
            if (DisplaySceneSettingKey.IsPressed)
            {
                showGUI = !showGUI;
            }
        }

        private void OnGUI()
        {
            if (!StatMaster.levelSimulating && showGUI && GeoTools.IsBuilding() && !StatMaster.inMenu)
            {
                windowRect = GUI.Window(windowID, windowRect, new GUI.WindowFunction(SceneWindow), LanguageManager.SceneWindowTitle);
            }

        }

        void SceneWindow(int ID)
        {
            GUILayout.BeginHorizontal();
            {
                if (GUI.Button(new Rect(10, 20, 80, 20), LanguageManager.FogButtonLabel)) { OnFogButtonClick(); }
                if (GUI.Button(new Rect(100, 20, 80, 20), LanguageManager.WorldBoundsButtonLabel)) { OnWorldBoundsButtonClick(); }
                if (GUI.Button(new Rect(190, 20, 80, 20), LanguageManager.FloorGridButtonLabel)) { OnFloorButtonClick(); }
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            {
                if (GUI.Button(new Rect(10, 20+25, 120, 20), LanguageManager.ReloadButonLabel)) { OnReloadScenesButtonClick(); }
                if (GUI.Button(new Rect(100+50, 20+25, 120, 20), LanguageManager.ScenesDirectoryButtonLabel)) { OnOpenScenePacksDirectoryButtonClick(); }
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginVertical();
            {
                GUI.Label(new Rect(10, 50+25, 280, 20), LanguageManager.SceneListLabel);
                GUI.Box(new Rect(10, 70+25, 260, 220), "");

                scrollVector = GUI.BeginScrollView(new Rect(15, 75+25, 250, 210), scrollVector, sceneButtonsRect);
                {
                    GUILayout.BeginArea(new Rect(0, 0, 250, sceneButtonsRect.height));
                    {
                        for (int i = 0; i < sceneMod.GetComponent<CustomSceneMod>().ScenePacks.Count; i++)
                        {
                            if (GUILayout.Button(sceneMod.GetComponent<CustomSceneMod>().ScenePacks[i].Name, GUILayout.Width(230), GUILayout.Height(20)))
                            {
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
