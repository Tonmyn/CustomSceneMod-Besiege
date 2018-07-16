using PluginManager.Plugin;
using UnityEngine;

namespace BesiegeCustomScene
{

    [OnGameInit]
    public class CustomSceneMod : MonoBehaviour
    {
        public GameObject SceneMod;

        string DisplayName = "Besiege Custom Scene";

        string Version = "1.10.8";

        public CustomSceneMod() { }

        private void Start()   
        {

            //添加MOD更新推送功能
            //new GameObject("Mod更新组件").AddComponent<Updater>().SetUrl("XultimateX", Name);

            SceneMod = new GameObject();
            SceneMod.name = string.Format("{0} {1}", DisplayName, Version);
            SceneMod.AddComponent<SettingsManager>();
            SceneMod.AddComponent<LanguageManager>();
            //SceneMod.AddComponent<SceneMod>();
            ////SceneMod.AddComponent<SceneUI>();
            ////SceneMod.AddComponent<TimeUI>();
 
            //SceneMod.AddComponent<Prop>();

            SceneMod.AddComponent<ToolBoxSettingUI>();

            UnityEngine.Object.DontDestroyOnLoad(SceneMod);

            

        }
    }
}
