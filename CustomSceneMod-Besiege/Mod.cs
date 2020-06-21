using UnityEngine;
using Modding;
using System.Collections.Generic;
using System;
using System.Collections;
using static CustomScene.GeoTools;
using System.Threading;


namespace CustomScene
{


    public class Mod : ModEntryPoint
    {
        
        public static GameObject ModObject;

        public static SceneModController SceneController;
        public static BlockInformationMod blockInformationMod;
        public static TimerMod timerMod;
        public static Prop prop;

        public override void OnLoad()
        {
            string DisplayName = "Custom Scene Mod";

            ModObject = new GameObject(DisplayName);

            //prop = ModObject.AddComponent<Prop>();

            //ModObject.AddComponent<test>();

            GameObject customScene = new GameObject("CustomScene");
            SceneController = SceneModController.Instance;
            SceneController.transform.SetParent(customScene.transform);
            customScene.AddComponent<UI.EnvironmentSettingUI>();
            customScene.transform.SetParent(ModObject.transform);

            //GameObject toolbox = new GameObject("ToolBox");
            //blockInformationMod = toolbox.AddComponent<BlockInformationMod>();
            //timerMod = toolbox.AddComponent<TimerMod>();
            //toolbox.AddComponent<UI.ToolBoxSettingUI>();
            //toolbox.transform.SetParent(ModObject.transform);

            //GameObject miniMap = new GameObject("Mini Map");
            //miniMap.AddComponent<UI.MiniMapSettingUI>();
            //miniMap.transform.SetParent(Mod.transform);

            UnityEngine.Object.DontDestroyOnLoad(ModObject);
     
        }
    }
}
