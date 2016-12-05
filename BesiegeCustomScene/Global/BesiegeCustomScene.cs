using spaar;
using System;
using System.Collections;
using System.IO;
using UnityEngine;

namespace BesiegeCustomScene
{

    public class BesiegeModLoader : spaar.ModLoader.Mod
    {
        public override string Name { get { return "BesiegeCustomScene"; } }
        public override string DisplayName { get { return "BesiegeCustomScene"; } }
        public override string BesiegeVersion { get { return "0.4"; } }
        public override string Author { get { return "zian1"; } }
        public override Version Version { get { return new Version("9.00"); } }
        public override bool CanBeUnloaded { get { return true; } }
        public GameObject temp;
        public override void OnLoad()
        {         
            temp = new GameObject(); temp.name = "BesiegeCustomScene_9_0";
            temp.AddComponent<SceneUI>();
            temp.AddComponent<TimeUI>();
            temp.AddComponent<MeshMod>();
            temp.AddComponent<TriggerUI>();
            temp.AddComponent<WaterMod>();
            temp.AddComponent<CloudMod>();
            UnityEngine.Object.DontDestroyOnLoad(temp);
        }
        public override void OnUnload()
        {
            UnityEngine.Object.Destroy(temp);
        }        
    }  
}
