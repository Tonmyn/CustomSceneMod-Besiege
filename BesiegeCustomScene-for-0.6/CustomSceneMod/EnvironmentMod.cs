using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Modding;

namespace BesiegeCustomScene
{
    abstract class EnvironmentMod : MonoBehaviour
    {

        void OnDisable()
        {
            ClearEnvironment();
        }

        void OnDestroy()
        {
            ClearEnvironment();
        }

        public abstract void ReadEnvironment(CustomSceneMod.ScenePack scenePack);

        public abstract void LoadEnvironment();

        public abstract void ClearEnvironment();

    }
}
