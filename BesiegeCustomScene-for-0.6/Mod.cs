using PluginManager.Plugin;
using UnityEngine;

namespace BesiegeCustomScene
{

    [OnGameInit]
    public class Mod : MonoBehaviour
    {
        public GameObject customSceneMod;

        string DisplayName = "Besiege Custom Scene";

        string Version = "1.10.9";

        private void Start()   
        {

            //添加MOD更新推送功能
            //new GameObject("Mod更新组件").AddComponent<Updater>().SetUrl("XultimateX", Name);

            customSceneMod = new GameObject();
            customSceneMod.name = string.Format("{0} {1}", DisplayName, Version);

            customSceneMod.AddComponent<SettingsManager>();
            customSceneMod.AddComponent<LanguageManager>();
            customSceneMod.AddComponent<UI.SceneSettingUI>();
            customSceneMod.AddComponent<UI.ToolBoxSettingUI>();
            
            ////SceneMod.AddComponent<SceneUI>();
            ////SceneMod.AddComponent<TimeUI>();
            //customSceneMod.AddComponent<MeshMod>();
            //customSceneMod.AddComponent<CubeMod>();
            ////customSceneMod.AddComponent<TriggerMod>();
            //customSceneMod.AddComponent<SnowMod>();
            //customSceneMod.AddComponent<CloudMod>();
            //customSceneMod.AddComponent<WaterMod>();
            ////customSceneMod.AddComponent<SkyMod>();
            //customSceneMod.AddComponent<Prop>();




            UnityEngine.Object.DontDestroyOnLoad(customSceneMod);

            

        }
    }
}
