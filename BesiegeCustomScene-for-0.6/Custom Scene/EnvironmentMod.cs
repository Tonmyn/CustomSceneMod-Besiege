using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Modding;

namespace BesiegeCustomScene
{
    abstract public class EnvironmentMod : MonoBehaviour
    {
        public CustomSceneMod customSceneMod;

        public DataLoader dataLoader;

        void OnDisable()
        {
            ClearEnvironment();
        }

        void OnDestroy()
        {
            ClearEnvironment();
        }

        void Start()
        {
            //transform.SetParent(BesiegeCustomSceneMod.Mod.GetComponent<UI.SceneSettingUI>().gameObject.transform);

            customSceneMod = transform.parent.GetComponent<CustomSceneMod>();
            customSceneMod.ReadSceneEvent += ReadEnvironment;
            customSceneMod.LoadSceneEvent += LoadEnvironment;
            customSceneMod.ClearSceneEvent += ClearEnvironment;

            //GameObject go = new GameObject();
            //go.AddComponent(this.GetType());
            //go.transform.SetParent(customSceneMod.transform);

            

            dataLoader = new DataLoader();
        }

        public abstract void ReadEnvironment(SceneFolder scenePack);

        public abstract void LoadEnvironment();

        public abstract void ClearEnvironment();

    }
}
