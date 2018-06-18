using System.Collections.Generic;
using UnityEngine;

namespace BesiegeCustomScene.UI
{
    class SceneSettingUI : MonoBehaviour
    {
        /// <summary>地图包列表</summary>
        public List<SceneMod.ScenePack> scenePacks;

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
                _FogUI = "Fog",
                _WorldBoundsUI = "World Bounds",
                _FloorGridUI = "Floor Grid",
                _SceneList = "Scene List:",
            };
        }

        

        private void Start()
        {
            sceneButtonsRect = new Rect(0, 0, 200, (scenePacks.Count - 1) * (buttonHeight + 5) + 5);
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
                windowRect = GUI.Window(windowID, windowRect, new GUI.WindowFunction(SceneWindow), "地图设置");
            }
            

            //// Wrap everything in the designated GUI Area
            //// 在指定的区域中包裹控件
            //GUILayout.BeginArea(new Rect(0, 0, 200, 200));

            //// Begin the singular Horizontal Group
            //// 开始单行的水平组
            //GUILayout.BeginHorizontal();

            //// Place a Button normally
            //// 放置一个重复按钮
            //if (GUILayout.RepeatButton("Increase max\nSlider Value"))
            //{
            //    maxSliderValue += 3.0 * Time.deltaTime;
            //}

            //// Arrange two more Controls vertically beside the Button
            //// 在前一个按钮旁边垂直排列两个或更多控件
            //GUILayout.BeginVertical();
            //GUILayout.Box("Slider Value: " + Mathf.Round(sliderValue));
            //sliderValue = GUILayout.HorizontalSlider(sliderValue, 0.0f, (float)maxSliderValue);

            //// End the Groups and Area
            //GUILayout.EndVertical();
            //GUILayout.EndHorizontal();
            //GUILayout.EndArea();

            //// Make a background box 创建一个背景盒子
            //GUI.Box(new Rect(10, 10, 100, 90), "Loader Menu");

            //// Make the first button. If it is pressed, Application.Loadlevel (1) will be executed
            //// 创建第一个按钮，如果按下这个按钮，将执行
            //if (GUI.Button(new Rect(20, 40, 80, 20), "Level 1"))
            //{
            //    Application.LoadLevel(1);
            //}

            //// Make the second button. 创建第二个按钮
            //if (GUI.Button(new Rect(20, 70, 80, 20), "Level 2"))
            //{
            //    Application.LoadLevel(2);
            //}
        }

        void SceneWindow(int ID)
        {

            GUILayout.BeginHorizontal();
            if (GUI.Button(new Rect(10, 20, 80, 20), "去雾")) { OnFogButtonClick(); }
            if (GUI.Button(new Rect(100, 20, 80, 20), "去空气墙")) { OnWorldBoundsButtonClick(); }
            if (GUI.Button(new Rect(190, 20, 80, 20), "去地板")) { OnFloorButtonClick(); }
            GUILayout.EndHorizontal();           
            
            GUILayout.BeginVertical();
            GUI.Label(new Rect(10,50,280,20), "地图列表:");
            GUI.Box(new Rect(10,70,260,220),"");
            scrollVector = GUI.BeginScrollView(new Rect(15, 75, 250, 210), scrollVector, sceneButtonsRect);
            GUILayout.BeginArea(new Rect(0, 0, 250, sceneButtonsRect.height));
            for (int i = 0; i < scenePacks.Count; i++)
            {
                if (GUILayout.Button(scenePacks[i].Name, GUILayout.Width(230), GUILayout.Height(20)))
                {
                    //LoadScene(i);
                    OnSceneButtonClick(i); 
                }
            }
            GUILayout.EndArea();
            GUI.EndScrollView();
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

        /// <summary>加载自定义地形</summary>
        void LoadScene(int i)
        {
            GetComponent<SceneMod>().LoadScenePack(i);
        }

        /// <summary>隐藏雾</summary>
        void HideFog()
        {

        }

        /// <summary>隐藏地面</summary>
        void HideFloorGrid()
        {
            GetComponent<SceneMod>().HideFloorBig();
        }

        /// <summary>隐藏空气墙</summary>
        void HideWorldBounds()
        {
            GetComponent<SceneMod>().HideWorldBoundaries();
        }
    
    }
}
