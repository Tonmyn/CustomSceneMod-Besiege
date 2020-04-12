using UnityEngine;
using Modding;
using System.Collections.Generic;
using System;
using System.Collections;
using static CustomScene.GeoTools;
using System.Threading;


namespace CustomScene
{


    public class Mod : ModEntryPoint
    {
        
        public static GameObject ModObject;

        public static SceneController SceneController;
        public static BlockInformationMod blockInformationMod;
        public static TimerMod timerMod;
        public static Prop prop;

        public override void OnLoad()
        {
            string DisplayName = "Custom Scene Mod";

            ModObject = new GameObject(DisplayName);

            //prop = ModObject.AddComponent<Prop>();

            //ModObject.AddComponent<test>();

            GameObject customScene = new GameObject("CustomScene");
            SceneController = SceneController.Instance;
            SceneController.transform.SetParent(customScene.transform);
            customScene.AddComponent<UI.EnvironmentSettingUI>();
            customScene.transform.SetParent(ModObject.transform);

            //GameObject toolbox = new GameObject("ToolBox");
            //blockInformationMod = toolbox.AddComponent<BlockInformationMod>();
            //timerMod = toolbox.AddComponent<TimerMod>();
            //toolbox.AddComponent<UI.ToolBoxSettingUI>();
            //toolbox.transform.SetParent(ModObject.transform);

            //GameObject miniMap = new GameObject("Mini Map");
            //miniMap.AddComponent<UI.MiniMapSettingUI>();
            //miniMap.transform.SetParent(Mod.transform);

            UnityEngine.Object.DontDestroyOnLoad(ModObject);
     
        }
    }



   //public class test :MonoBehaviour
   // {


   //     testClass ts, ts1;

   //     SceneSettingFile sceneSettingFile;

   //     public class testClass: Modding.Serialization.Element
   //     {
       
   //         [Modding.Serialization.RequireToValidate]
   //         public string SceneName { get; set; }
   //         [Modding.Serialization.CanBeEmpty]
   //         public string Author { get; set; }
   //         [Modding.Serialization.CanBeEmpty]
   //         public testClass2 testClass2 { get; set; }


   //         public override string ToString()
   //         {
   //             string str = "";
   //             if (testClass2 != null)
   //                 str = string.Format("{0}-{1}\n{2}", SceneName, Author, testClass2.ToString());
   //             else
   //                 str = string.Format("{0}-{1}\n{2}", SceneName, Author, "null");

   //             return str;
   //         }
   //     }
     
   //     public class testClass2
   //     {
   //         public string matType;
   //         public string matName;
   //         //public Material material;

   //         public static Dictionary<string, Type> dic = new Dictionary<string, Type>
   //        {
   //            { "contents",typeof(string) },
   //            { "shader",typeof(Shader)},
   //             { "material",typeof(Material)}
   //        };

   //         public override string ToString()
   //         {
   //             string str = "";

   //             str = string.Format("{0}+{1}", matType, matName);

   //             return str;
   //         }

   //     }

   //     void Start()
   //     {
   //         ts = new testClass();
   //         ts.SceneName = "test map";
   //         ts.Author = "ltm";
   //         //ts.testClass2 = new testClass2();
   //         //ts.testClass2.matName = "mat name";
   //         //ts.testClass2.matType = "shader";
   //         //ts.testClass2.material = new Material(Shader.Find("diffuse"));

   //         sceneSettingFile = new SceneSettingFile()
   //         {
   //             AuthorName = "XultimateX",
   //             SceneDescription = "Ver.0.0.1",
   //             /* Texture = new Serializable.Shader() { name="test-shader", propertise = new Serializable.Shader.ShaderPropertise[] { new Serializable.Shader.ShaderPropertise() { Name ="mainTexture", DataType ="Texture", Value = "test-tex"}, new Serializable.Shader.ShaderPropertise() { Name = "color", DataType = "Color", Value = "1,1,1,1" } } } */
   //             MeshsPropertise = new TerrainPropertise() { Size = 1 },
   //             MeshPropertises = new MeshPropertise[] { new MeshPropertise() }
   //             //{
   //             //    collider = new Serializable.Collider() {  mesh = new Serializable.Mesh() { meshName="test-mesh"} },
   //             //     mesh = new Serializable.Mesh() {  meshName ="test-mesh"},
   //             //      name = "test-object",
   //             //       renderer = new Serializable.Renderer()
   //             //       {
   //             //           material = new Serializable.Material()
   //             //           {
   //             //                 texture = new Serializable.Texture() {name = "test-tex"}
   //             //           }
   //             //       }              
   //             //}
   //         };
   //     }

   //     void Update()
   //     {
   //         if (Input.GetKeyDown(KeyCode.S))
   //         {
   //             Debug.Log("SerializeXml");

   //             ModIO.SerializeXml(sceneSettingFile, "setting.xml", true);

   //         }

   //         if (Input.GetKeyDown(KeyCode.D))
   //         {
   //             Debug.Log("DeserializeXml");
   //             sceneSettingFile = ModIO.DeserializeXml<SceneSettingFile>("setting.xml", true,false);
                
   //             Debug.Log(sceneSettingFile.ToString());
   //         }
   //     }

   //     Texture2D[] texture = new Texture2D[50];

   //     void OnGUI()
   //     {
   //         if (GUI.Button(new Rect(10, 10, 200, 200), texture[0]))
   //         {
   //             StartCoroutine(load());
   //             //texture = null;
   //            //texture = ResourceLoader.LoadTexture(@"Scenes\平地\Textures\GroundTexture.png", new Vector2(1, 1), true);

   //             //Resources.UnloadUnusedAssets();
            
   //         }

   //         if (GUI.Button(new Rect(10, 300, 200, 200),"clear"))
   //         {
   //             foreach (var tex in texture)
   //             {
   //                 Destroy(tex);
   //             }
   //             //Resources.UnloadUnusedAssets();
   //             //Destroy(texture);
   //         }

   //     }

   //     IEnumerator load()
   //     {
   //         //for (int i = 0; i < 50; i++)
   //         //{
   //         //    var tex = ResourceLoader.LoadTexture("test"+i, @"Scenes\平地\Textures\GroundTexture", true);
   //         //    yield return new WaitUntil(() => tex.Loaded);
   //         //    texture[i] = tex;


   //         //    Debug.Log(i);
   //         //    yield return new WaitForSeconds(0.5f);
   //         //}

   //         for (int i = 0; i<50; i++)
   //         {
   //             texture[i] = ResourceLoader.LoadTexture(@"Scenes\平地\Textures\GroundTexture.png", new Vector2(1, 1), true);

   //             yield return 0;
   //         }


   //         //var tex = ResourceLoader.LoadTexture("test", @"Scenes\平地\Textures\GroundTexture", true);
   //         //yield return new WaitUntil(() => tex.Loaded);
   //         //texture = tex;

   //         //var tex = Resources.LoadAsync/*<Texture2D>*/(@"F:\Games\Besiege\v.0.8-beta\Besiege_Data\Mods\Data\CustomSceneMod_2c12a0fb-c022-4717-a9e8-f8b2bd9de433\Scenes\平地\Textures\GroundTexture.png");
   //         //yield return new WaitUntil(() => tex.isDone);

   //         //texture = tex.asset as Texture2D;
   //         //Debug.Log("done");
   //     }
   // }
}
