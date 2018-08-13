using UnityEngine;
using Modding;

namespace BesiegeCustomScene
{


    public class BesiegeCustomSceneMod : ModEntryPoint
    {
        
        public static GameObject Mod;

        public override void OnLoad()
        {
            string DisplayName = "Besiege Custom Scene";

            string Version = "1.10.9";

            //添加MOD更新推送功能
            //new GameObject("Mod更新组件").AddComponent<Updater>().SetUrl("XultimateX", Name);

            Mod = new GameObject
            {
                name = string.Format("{0} {1}", DisplayName, Version)
            };

            Mod.AddComponent<SettingsManager>();
            Mod.AddComponent<Prop>();

            GameObject customScene = new GameObject("CustomScene");
            customScene.AddComponent<UI.SceneSettingUI>();
            customScene.transform.SetParent(Mod.transform);

            GameObject toolbox = new GameObject("ToolBox");
            toolbox.AddComponent<UI.ToolBoxSettingUI>();
            toolbox.transform.SetParent(Mod.transform);

            //GameObject miniMap = new GameObject("Mini Map");
            //miniMap.AddComponent<UI.MiniMapSettingUI>();
            //miniMap.transform.SetParent(Mod.transform);

            Object.DontDestroyOnLoad(Mod);
        }
    }

   
}
