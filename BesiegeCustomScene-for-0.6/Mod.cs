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

            Mod = new GameObject();
            Mod.name = string.Format("{0} {1}", DisplayName, Version);

            Mod.AddComponent<SettingsManager>();
            Mod.AddComponent<LanguageManager>();
            Mod.AddComponent<Prop>();

            GameObject customSceneMod = new GameObject("Custom Scene Mod");
            customSceneMod.AddComponent<UI.SceneSettingUI>();
            customSceneMod.transform.SetParent(Mod.transform);

            GameObject toolboxmod = new GameObject("Tool Box Mod");
            toolboxmod.AddComponent<UI.ToolBoxSettingUI>();
            toolboxmod.transform.SetParent(Mod.transform);

            //UnityEngine.Object.DontDestroyOnLoad(Mod);


        }
    }

   
}
