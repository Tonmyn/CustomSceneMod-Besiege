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
        public override string BesiegeVersion { get { return "v0.4"; } }
        public override string Author { get { return "zian1"; } }
        public override Version Version { get { return new Version("9.6"); } }
        public override bool CanBeUnloaded { get { return true; } }
        public GameObject temp;
        public override void OnLoad()
        {         
            temp = new GameObject(); temp.name = "BesiegeCustomScene_v9";
            temp.AddComponent<SceneUI>();
            temp.AddComponent<TimeUI>();
            temp.AddComponent<MeshMod>();
            temp.AddComponent<TriggerUI>();
            temp.AddComponent<SnowMod>();
            temp.AddComponent<CloudMod>();
            temp.AddComponent<WaterMod>();
            temp.AddComponent<Prop>();
            UnityEngine.Object.DontDestroyOnLoad(temp);
        }
        public override void OnUnload()
        {
            UnityEngine.Object.Destroy(temp);
        }        
    }  
}
