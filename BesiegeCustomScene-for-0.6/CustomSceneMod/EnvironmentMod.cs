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

        void OnDisable()
        {
            ClearEnvironment();
        }

        void OnDestroy()
        {
            ClearEnvironment();
        }


        public abstract void ReadEnvironment(ScenePack scenePack);

        public abstract void LoadEnvironment();

        public abstract void ClearEnvironment();

    }
}
