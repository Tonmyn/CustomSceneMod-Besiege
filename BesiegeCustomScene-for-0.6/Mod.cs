using UnityEngine;
using Modding;

namespace BesiegeCustomScene
{


    public class BesiegeCustomSceneMod : ModEntryPoint
    {

        public override void OnLoad()
        {

            GameObject customSceneMod;

            string DisplayName = "Besiege Custom Scene";

            string Version = "1.10.9";

            //添加MOD更新推送功能
            //new GameObject("Mod更新组件").AddComponent<Updater>().SetUrl("XultimateX", Name);

            customSceneMod = new GameObject();
            customSceneMod.name = string.Format("{0} {1}", DisplayName, Version);

            customSceneMod.AddComponent<SettingsManager>();
            customSceneMod.AddComponent<LanguageManager>();
            customSceneMod.AddComponent<Prop>();
            customSceneMod.AddComponent<UI.SceneSettingUI>();
            customSceneMod.AddComponent<UI.ToolBoxSettingUI>();

            //customSceneMod.AddComponent<TestScript>();


            //UnityEngine.Object.DontDestroyOnLoad(customSceneMod);

            //GameObject go = new GameObject("test object");
            //go.AddComponent<test_script>();

            BesiegeConsoleController.ShowMessage("on load form BesiegeCustomSceneMod");

        }
    }

   
}
