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

namespace BesiegeCustomScene
{

    public delegate void ReadSceneHandler(SceneFolder scenePack);
    public delegate void LoadSceneHandler();
    public delegate void ClearSceneHandler();

    public class CustomSceneMod : MonoBehaviour
    {
    
        /// <summary>地图包路径</summary>
        public static string ScenePacksPath;
        /// <summary>地图包列表</summary>
        public List<SceneFolder> ScenePacks;

        public bool WorldBoundariesEnable = true;
        public bool FloorBigEnable = true;
        public bool FogEnable = true;

        public event ReadSceneHandler ReadSceneEvent;
        public event LoadSceneHandler LoadSceneEvent;
        public event ClearSceneHandler ClearSceneEvent;

        List<string> sceneNames;

        //public List<GameObject> EnvironmentObjects;

        void Awake()
        {
            sceneNames = BesiegeCustomSceneMod.Mod.GetComponent<SettingsManager>().settingFile.settings.sceneNames;

            ScenePacksPath = GeoTools.ScenePackPath;

            ScenePacks = ReadScenePacks(ScenePacksPath);
        }

        void Start()
        {

            SceneManager.sceneLoaded += (Scene s, LoadSceneMode lsm) =>
            {
                WorldBoundariesEnable = true;

                FloorBigEnable = true;

                FogEnable = true;
            };
        }

        void OnDisable()
        {
            ClearEnvironment();
        }

        void OnDestroy()
        {
            ClearEnvironment();
        }

        /// <summary>
        /// 读取指定路径下所有地图包
        /// </summary>
        /// <param name="scenesPackPath">地图包路径</param>
        /// <returns></returns>
        public List<SceneFolder> ReadScenePacks(string scenesPackPath)
        {
            List<SceneFolder> SPs = new List<SceneFolder>() { };

            if (!ModIO.ExistsDirectory(scenesPackPath))
            {
                GeoTools.Log("Error! Scenes Path Directory not exists!");
                return SPs;
            }

            foreach (var sceneName in sceneNames)
            {
                SPs.Add(new SceneFolder(sceneName));
            }

            SPs = SPs.Distinct(new Compare()).ToList();
            return SPs;
        }

        public List<SceneFolder> ReloadScenePacks()
        {
            return ReadScenePacks(ScenePacksPath);
        }


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



        /// <summary>
        /// 加载地图包
        /// </summary>
        /// <param name="ScenePack">地图包</param>
        IEnumerator ILoadScenePack(SceneFolder scenePack)
        {
            if (SceneManager.GetActiveScene().name != "2")
            {
                SceneManager.LoadScene("2", LoadSceneMode.Single);//打开level  
            }
            yield return null;

            loadScenePack(scenePack);

        }

        /// <summary>
        /// 加载地图包 多人模式下
        /// </summary>
        /// <param name="ScenePack">地图包</param>
        IEnumerator ILoadScenePack_Multiplayer(SceneFolder scenePack)
        {

            yield return null;

            loadScenePack(scenePack);

        }

        void loadScenePack(SceneFolder scenePack)
        {
            hideFloorBig();

            ReadSceneEvent(scenePack);

            LoadSceneEvent();

        }

        /// <summary>
        /// 加载地图包
        /// </summary>
        /// <param name="ScenePack">地图包列表序号</param>
        public void LoadScenePack(int index)
        {
#if DEBUG
            GeoTools.Log("load scene pack");
#endif
            if (BesiegeNetworkManager.Instance == null)
            {
#if DEBUG
                GeoTools.Log("load scene pack not in multiplayers");
#endif
                StartCoroutine(ILoadScenePack(ScenePacks[index]));
                return;

            }


            if (BesiegeNetworkManager.Instance.isActiveAndEnabled && BesiegeNetworkManager.Instance.isConnected)
            {
#if DEBUG
                GeoTools.Log("load scene pack in multiplayers");
#endif
                StartCoroutine(ILoadScenePack_Multiplayer(ScenePacks[index]));
                return;
            }

        }

        //清除地图
        public void ClearEnvironment()
        {
            ClearSceneEvent();

            Resources.UnloadUnusedAssets();
        }



        #region 隐藏/显示地面 空气墙 雾
 
        /// <summary>
        /// 隐藏地面
        /// </summary>
        public void HideFloorBig()
        {

            if (!FloorBigEnable)
            {
                UnhideFloorBig();
                return;
            }

            hideFloorBig();

        }

        private void hideFloorBig()
        {

            GameObject floorBig = GameObject.Find("FloorBig");

            try
            {
                floorBig.GetComponent<BoxCollider>().enabled = false;
                floorBig.GetComponent<MeshRenderer>().enabled = false;
                foreach (var v in floorBig.GetComponentsInChildren<Renderer>())
                {
                    v.enabled = false;
                }

                FloorBigEnable = false;
            }
            catch{ }
        }

        /// <summary>
        /// 还原FloorBig
        /// </summary>
        private void UnhideFloorBig()
        {

            GameObject floorBig = GameObject.Find("FloorBig");

            try
            {
                floorBig.GetComponent<BoxCollider>().enabled = true;
                floorBig.GetComponent<MeshRenderer>().enabled = true;
                foreach (var v in floorBig.GetComponentsInChildren<Renderer>())
                {
                    v.enabled = true;
                }
                FloorBigEnable = true;
            }
            catch { }

        }

        /// <summary>
        /// 隐藏空气墙
        /// </summary>
        public void HideWorldBoundaries()
        {

            try
            {

                //单人模式下
                GameObject WorldBoundaries_Large = GameObject.Find("WORLD BOUNDARIES_LARGE");

                Set_WorldBoundaries(WorldBoundaries_Large);

            }
            catch (Exception e)
            {
                GeoTools.Log(e.Message);
                WorldBoundariesEnable = !WorldBoundariesEnable;
            }

            try
            {
                //多人模式下
                GameObject WorldBoundaries = GameObject.Find("WORLD BOUNDARIES");

                Set_WorldBoundaries(WorldBoundaries);

            }
            catch(Exception e)
            {
                GeoTools.Log(e.Message);
                WorldBoundariesEnable = !WorldBoundariesEnable;
            }

            void Set_WorldBoundaries(GameObject WorldBoundaries)
            {
                WorldBoundariesEnable = !WorldBoundariesEnable;

                foreach (BoxCollider BC in WorldBoundaries.GetComponentsInChildren<BoxCollider>())
                {
                    BC.isTrigger = !WorldBoundariesEnable;
                }

                foreach (Renderer MR in WorldBoundaries.GetComponentsInChildren<Renderer>())
                {
                    MR.enabled = WorldBoundariesEnable;
                }
                
            }
         

        }

        /// <summary>
        /// 隐藏雾
        /// </summary>
        public void HideFog()
        {
            FogEnable = !FogEnable;

            try
            {
                if (/*Level.GetCurrentLevel().Setup.Name == "BARREN EXPANSE" ||*/ SceneManager.GetActiveScene().name == "BARREN EXPANSE")
                {

                    GameObject mainCamera = GameObject.Find("Main Camera");
                    mainCamera.GetComponent<ColorfulFog>().enabled = FogEnable;
                }
                else if (SceneManager.GetActiveScene().name == "MasterSceneMultiplayer")
                {
                    if (Level.GetCurrentLevel().Setup.Name == null)
                    {
                        GameObject mainCamera = GameObject.Find("Main Camera");
                        mainCamera.GetComponent<ColorfulFog>().enabled = FogEnable;
                    }
                }
            }
            catch
            { }

            try
            {
                GameObject fogSPHERE = GameObject.Find("FOG SPHERE");
                fogSPHERE.GetComponent<MeshRenderer>().enabled = FogEnable;
            }
            catch
            { }

            try
            {
                GameObject.Find("Fog Volume").GetComponent<MeshRenderer>().enabled = FogEnable;
            }
            catch (Exception e)
            {
                GeoTools.Log(e.Message);
            }

        }
        #endregion
    }

   

}
