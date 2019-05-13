using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Modding;

namespace CustomScene
{
    abstract public class EnvironmentMod<T> /*: MonoBehaviour*/ where T: EnvironmentPropertise
    {
        //public CustomSceneMod customSceneMod;

        //public DataLoader dataLoader;

        public abstract T Propertise { get; }

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

            //customSceneMod = transform.parent.GetComponent<CustomSceneMod>();
            //customSceneMod.ReadSceneEvent += ReadEnvironment;
            //customSceneMod.LoadSceneEvent += LoadEnvironment;
            //customSceneMod.ClearSceneEvent += ClearEnvironment;          

            ////dataLoader = new DataLoader();
        }

        public abstract void ReadEnvironment(SceneFolder scenePack);

        public abstract void LoadEnvironment();

        public abstract void ClearEnvironment();

    }

    public class EnvironmentPropertise
    {


    }
}
