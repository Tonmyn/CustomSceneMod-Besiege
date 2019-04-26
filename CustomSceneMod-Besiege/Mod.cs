using UnityEngine;
using Modding;
using System.Collections.Generic;
using System;

namespace BesiegeCustomScene
{


    public class BesiegeCustomSceneMod : ModEntryPoint
    {
        
        public static GameObject Mod;

        public override void OnLoad()
        {
            string DisplayName = "Besiege Custom Scene";

            string Version = "2.0.0";

            Mod = new GameObject
            {
                name = string.Format("{0} {1}", DisplayName, Version)
            };

            Mod.AddComponent<Prop>();Mod.AddComponent<test>();

            GameObject customScene = new GameObject("CustomScene");
            customScene.AddComponent<UI.SceneSettingUI>();
            customScene.transform.SetParent(Mod.transform);

            GameObject toolbox = new GameObject("ToolBox");
            toolbox.AddComponent<UI.ToolBoxSettingUI>();
            toolbox.transform.SetParent(Mod.transform);

            //GameObject miniMap = new GameObject("Mini Map");
            //miniMap.AddComponent<UI.MiniMapSettingUI>();
            //miniMap.transform.SetParent(Mod.transform);

            UnityEngine.Object.DontDestroyOnLoad(Mod);

        }
    }

   public class test :MonoBehaviour
    {


        testClass ts, ts1;

        public class testClass: Modding.Serialization.Element
        {
       
            [Modding.Serialization.RequireToValidate]
            public string SceneName { get; set; }
            [Modding.Serialization.CanBeEmpty]
            public string Author { get; set; }
            [Modding.Serialization.CanBeEmpty]
            public testClass2 testClass2 { get; set; }


            public override string ToString()
            {
                string str = "";
                if (testClass2 != null)
                    str = string.Format("{0}-{1}\n{2}", SceneName, Author, testClass2.ToString());
                else
                    str = string.Format("{0}-{1}\n{2}", SceneName, Author, "null");

                return str;
            }
        }
     
        public class testClass2
        {
            public string matType;
            public string matName;
            //public Material material;

            public static Dictionary<string, Type> dic = new Dictionary<string, Type>
           {
               { "contents",typeof(string) },
               { "shader",typeof(Shader)},
                { "material",typeof(Material)}
           };

            public override string ToString()
            {
                string str = "";

                str = string.Format("{0}+{1}", matType, matName);

                return str;
            }

        }

        void Start()
        {
            ts = new testClass();
            ts.SceneName = "test map";
            ts.Author = "ltm";
            //ts.testClass2 = new testClass2();
            //ts.testClass2.matName = "mat name";
            //ts.testClass2.matType = "shader";
            //ts.testClass2.material = new Material(Shader.Find("diffuse"));
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                Debug.Log("SerializeXml");

                ModIO.SerializeXml(ts, "testfile.xml", true);

            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                Debug.Log("DeserializeXml");
                ts1 = ModIO.DeserializeXml<testClass>("testfile.xml", true,false);
                
                Debug.Log(ts1.ToString());
            }
        }

    }
}
