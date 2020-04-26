using System;
using System.Collections;
using System.Collections.Generic;
//using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using Modding;
using Modding.Levels;

namespace CustomScene
{

    public class SceneController : SingleInstance<SceneController>
    {
        public override string Name { get; } = "Scene Controller";

        /// <summary>地图包路径</summary>
        public static string ScenePacksPath = "Scenes";
        /// <summary>地图包列表</summary>
        public List<SceneFolder> ScenePacks;
        ///// <summary>地图物体</summary>
        //public GameObject SceneObjects;
        public Scene CurrentScene { get; private set; }
        public List<Scene> Scenes { get; private set; }


        public bool worldBoundariesEnable = true;
        public bool FloorBigEnable = true;
        public bool fogEnable = true;

        //public Action<SceneFolder> ReadSceneEvent;
        public event Action<Scene> OnLoadSceneEvent;
        public event Action OnClearSceneEvent;

        void Awake()
        {

            ScenePacksPath = GeoTools.ScenePackPath;

            ScenePacks = ReadScenePacks(ScenePacksPath, GeoTools.isDataMode);

            //SceneObjects = new GameObject("Scene Objects");
            //SceneObjects.transform.SetParent(transform);

            Scenes = ReadScenes(ScenePacksPath, true);

            ModConsole.RegisterCommand("csm", new CommandHandler((value) =>
            {
                Dictionary<string, Action<string[]>> commandOfAction = new Dictionary<string, Action<string[]>>
                {
                    { "CreateNewScene".ToLower(),   (args)=>{ if(value[1]!= null&&value[1]!=""){SceneController.Instance.CreateNewScene(value[1], true);} } },
                    { "LoadScene".ToLower(),   (args)=>{ if(value[1]!= null&&value[1]!=""){SceneController.Instance.LoadScene(value[1]);} } },
                     { "ClearScene".ToLower(),   (args)=>{ SceneController.Instance.ClearScene();} },
                };

                if (commandOfAction.ContainsKey(value[0].ToLower()))
                {
                    commandOfAction[value[0].ToLower()].Invoke(value);
                }
                else
                {
                    Debug.Log(string.Format( "Unknown command '{0}', type 'help' for list.",value[0]));
                }
            }),
            "<color=#FF6347>" +
            "Custom Scene Mod Commands\n" +
            "  Usage: csm CreateNewScene <SceneName> :  Create a new custom scene.\n" +
            "  Usage: csm LoadScene <SceneName> :  Load a exist's custom scene.\n" +
            "  Usage: csm ClearScene :  Load a exist's custom scene.\n" +
            "</color>"
            );
        }

        //void Start()
        //{
        //    SceneManager.sceneLoaded += (UnityEngine.SceneManagement.Scene s, LoadSceneMode lsm) =>
        //    {
        //        worldBoundariesEnable = true;

        //        FloorBigEnable = true;

        //        fogEnable = true;

        //        if (!GeoTools.IsBuilding())
        //        {
        //            ClearEnvironment();
        //        }
        //    };
        //}

        //void OnDisable()
        //{
        //    //ClearEnvironment();
        //    CurrentScene.Clear();
        //}

        //void OnDestroy()
        //{
        //    //ClearEnvironment();
        //    CurrentScene.Clear();
        //}

        /// <summary>
        /// 读取指定路径下所有地图包
        /// </summary>
        /// <param name="scenesPacksPath">地图包路径</param>
        /// <returns></returns>
        public List<SceneFolder> ReadScenePacks(string scenesPacksPath, bool data = false)
        {
            List<SceneFolder> SPs = new List<SceneFolder>() { };
            List<string> scenePaths = new List<string>();

            if (!ModIO.ExistsDirectory(scenesPacksPath, data))
            {
                ModIO.CreateDirectory("Scenes", data);
            }

            scenePaths = ModIO.GetDirectories(scenesPacksPath, data).ToList();

            foreach (var scenePath in scenePaths)
            {
                SPs.Add(new SceneFolder(scenePath, data));
            }

            SPs = SPs.Distinct(new Compare()).ToList();
            return SPs;
        }

        public List<Scene> ReadScenes(string scenesPath, bool data = false)
        {
            var scenes = new HashSet<Scene>();

            if (!ModIO.ExistsDirectory(scenesPath, data))
            {
                ModIO.CreateDirectory(scenesPath, data);
            }

            var paths = ModIO.GetDirectories(scenesPath, data);
            if (paths.Length > 0)
            {
                Debug.Log(paths.Length);
                foreach (var path in paths)
                {
                    Debug.Log(path);
                    var scene = new Scene(path, data);
                    if (scene.Enabled) scenes.Add(scene);
                }
                Debug.Log(scenes.Count);
            }

            return scenes.ToList();
        }

        //        public void ReloadScenePacks()
        //        {
        //            ReadScenePacks(ScenePacksPath, true);
        //        }

        //        public void OpenScenesDirectory()
        //        {
        //            GeoTools.OpenDirctory(ScenePacksPath, GeoTools.isDataMode);
        //        }
        public void RefreshScenes()
        {
            Scenes.Clear();
            Scenes = ReadScenes(ScenePacksPath, true);
        }

        public void CreateNewScene(string path ,bool data = false)
        {
            path = ScenePacksPath + @"\" + path; 
            if (!Scenes.Exists(match =>  match.Path == path ))
            {
                new Scene(path, data);
                RefreshScenes();

                Debug.Log("Create Scene success...");
            }
            else
            {
                Debug.Log("Scene is existed...");    
            }
        }

        public void LoadScene(Scene scene)
        {
            ClearScene();

            if (scene.isExist && scene!= null)
            {
                CurrentScene = scene;
                CurrentScene.Load(transform.parent);
            }
            else
            {
                RefreshScenes();
                Debug.Log("Scene is not exist...");
            }
        }
        public void LoadScene(string name)
        {
            var scene = Scenes.Find(match => match.Propertise.Name == name);

            LoadScene(scene);
        }
        public void LoadScene(int index,bool data = false)
        {
            if (index + 1 <= Scenes.Count)
            {
                LoadScene(Scenes[index]);
            }
            else
            {
                Debug.Log("index is wrong");
                RefreshScenes();
            }
        }

        public void ClearScene()
        {
            if (CurrentScene != null)
            {
                CurrentScene.Clear();
            }
        }

        /// <summary>List去重类</summary>
        class Compare : IEqualityComparer<SceneFolder>
        {
            public bool Equals(SceneFolder x, SceneFolder y)
            {
                return x.Name == y.Name;//可以自定义去重规则，此处将地图包名字相同的就作为重复记录，不管地图的路径是什么
            }

            public int GetHashCode(SceneFolder obj)
            {
                return obj.Name.GetHashCode();
            }
        }



        //        /// <summary>
        //        /// 加载地图包
        //        /// </summary>
        //        /// <param name="ScenePack">地图包</param>
        //        IEnumerator ILoadScenePack(SceneFolder scenePack)
        //        {
        //            if (SceneManager.GetActiveScene().name != "2")
        //            {
        //                SceneManager.LoadScene("2", LoadSceneMode.Single);//打开level  
        //            }
        //            yield return null;

        //            LoadScenePack(scenePack);

        //        }

        //        /// <summary>
        //        /// 加载地图包 多人模式下
        //        /// </summary>
        //        /// <param name="ScenePack">地图包</param>
        //        IEnumerator ILoadScenePack_Multiplayer(SceneFolder scenePack)
        //        {

        //            yield return null;

        //            LoadScenePack(scenePack);

        //        }

        //        void LoadScenePack(SceneFolder scenePack)
        //        {
        //            //HideFloorBigy();

        //            //ReadSceneEvent(scenePack);

        //            //LoadSceneEvent();

        //        }

        //        /// <summary>
        //        /// 加载地图包
        //        /// </summary>
        //        /// <param name="ScenePack">地图包列表序号</param>
        //        public void LoadScenePack(int index)
        //        {
        //#if DEBUG
        //            GeoTools.Log("load scene pack");
        //#endif
        //            if (BesiegeNetworkManager.Instance == null)
        //            {
        //#if DEBUG
        //                GeoTools.Log("load scene pack not in multiplayers");
        //#endif
        //                StartCoroutine(ILoadScenePack(ScenePacks[index]));
        //                return;

        //            }


        //            if (BesiegeNetworkManager.Instance.isActiveAndEnabled && BesiegeNetworkManager.Instance.isConnected)
        //            {
        //#if DEBUG
        //                GeoTools.Log("load scene pack in multiplayers");
        //#endif
        //                StartCoroutine(ILoadScenePack_Multiplayer(ScenePacks[index]));
        //                return;
        //            }

        //        }

        //        //清除地图
        //        public void ClearEnvironment()
        //        {
        //            ClearSceneEvent();

        //            Resources.UnloadUnusedAssets();
        //        }



        //        #region 隐藏/显示地面 空气墙 雾

        //        /// <summary>
        //        /// 隐藏地面
        //        /// </summary>
        //        public void HideFloorBig()
        //        {

        //            if (!FloorBigEnable)
        //            {
        //                UnhideFloorBig();
        //                return;
        //            }

        //            HideFloorBigy();

        //        }

        //        private void HideFloorBigy()
        //        {

        //            GameObject floorBig = GameObject.Find("FloorBig");

        //            try
        //            {
        //                floorBig.GetComponent<BoxCollider>().enabled = false;
        //                floorBig.GetComponent<MeshRenderer>().enabled = false;
        //                foreach (var v in floorBig.GetComponentsInChildren<Renderer>())
        //                {
        //                    v.enabled = false;
        //                }

        //                FloorBigEnable = false;
        //            }
        //            catch { }
        //        }

        //        /// <summary>
        //        /// 还原FloorBig
        //        /// </summary>
        //        private void UnhideFloorBig()
        //        {

        //            GameObject floorBig = GameObject.Find("FloorBig");

        //            try
        //            {
        //                floorBig.GetComponent<BoxCollider>().enabled = true;
        //                floorBig.GetComponent<MeshRenderer>().enabled = true;
        //                foreach (var v in floorBig.GetComponentsInChildren<Renderer>())
        //                {
        //                    v.enabled = true;
        //                }
        //                FloorBigEnable = true;
        //            }
        //            catch { }

        //        }

        //        /// <summary>
        //        /// 隐藏空气墙
        //        /// </summary>
        //        public void HideWorldBoundaries()
        //        {
        //            try
        //            {

        //                //单人模式下
        //                GameObject WorldBoundaries_Large = GameObject.Find("WORLD BOUNDARIES_LARGE");

        //                SetWorldBoundaries(WorldBoundaries_Large);

        //            }
        //            catch (Exception e)
        //            {
        //                GeoTools.Log(e.Message);
        //                worldBoundariesEnable = !worldBoundariesEnable;
        //            }

        //            try
        //            {
        //                //多人模式下
        //                GameObject worldBoundaries = GameObject.Find("WORLD BOUNDARIES");
        //                //Bounds worldBoundaries = new Bounds();
        //                //if (WorldBoundaries == null)
        //                //{
        //                //    Debug.LogError("Can't find level bounds!");
        //                //}
        //                //Collider[] componentsInChildren = WorldBoundaries.GetComponentsInChildren<Collider>(true);
        //                //for (int i = 0; i < componentsInChildren.Length; i++)
        //                //{
        //                //    Bounds bound = componentsInChildren[i].bounds;
        //                //    if (i != 0)
        //                //    {
        //                //        worldBoundaries.Encapsulate(bound);
        //                //    }
        //                //    else
        //                //    {
        //                //        worldBoundaries = bound;
        //                //    }
        //                //}
        //                //worldBoundaries.Expand(worldBoundaries.extents * 2f * 100f);

        //                //NetworkCompression.SetWorldBounds(worldBoundaries);
        //                SetWorldBoundaries(worldBoundaries);

        //            }
        //            catch (Exception e)
        //            {
        //                GeoTools.Log(e.Message);
        //                worldBoundariesEnable = !worldBoundariesEnable;
        //            }

        //            void SetWorldBoundaries(GameObject WorldBoundaries)
        //            {
        //                worldBoundariesEnable = !worldBoundariesEnable;

        //                foreach (BoxCollider BC in WorldBoundaries.GetComponentsInChildren<BoxCollider>())
        //                {
        //                    BC.isTrigger = !worldBoundariesEnable;
        //                }

        //                foreach (Renderer MR in WorldBoundaries.GetComponentsInChildren<Renderer>())
        //                {
        //                    MR.enabled = worldBoundariesEnable;
        //                }

        //            }


        //        }

        //        /// <summary>
        //        /// 隐藏雾
        //        /// </summary>
        //        public void HideFog()
        //        {
        //            fogEnable = !fogEnable;

        //            Camera.main.farClipPlane = fogEnable ? 1500 : 150000;

        //            try
        //            {
        //                if (SceneManager.GetActiveScene().name == "BARREN EXPANSE" || (SceneManager.GetActiveScene().name == "MasterSceneMultiplayer" && (Level.GetCurrentLevel().Setup.Name == null || Level.GetCurrentLevel().Setup.Name == "")))
        //                {

        //                    GameObject mainCamera = GameObject.Find("Main Camera");
        //                    mainCamera.GetComponent<ColorfulFog>().enabled = fogEnable;
        //                }
        //            }
        //            catch
        //            { }

        //            try
        //            {
        //                GameObject fogSPHERE = GameObject.Find("FOG SPHERE");
        //                fogSPHERE.GetComponent<MeshRenderer>().enabled = fogEnable;
        //            }
        //            catch
        //            { }

        //            try
        //            {
        //                GameObject.Find("Fog Volume").GetComponent<MeshRenderer>().enabled = fogEnable;
        //            }
        //            catch (Exception e)
        //            {
        //                GeoTools.Log(e.Message);
        //            }

        //        }
        //        #endregion
    }



}
