﻿using System;
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

    public class CustomSceneMod : MonoBehaviour
    {

        /// <summary>地图包路径</summary>
        public static string ScenePacksPath;
        /// <summary>地图包列表</summary>
        public List<SceneFolder> ScenePacks;

        public bool worldBoundariesEnable = true;
        public bool FloorBigEnable = true;
        public bool fogEnable = true;

        public Action<SceneFolder> ReadSceneEvent;
        public Action LoadSceneEvent;
        public Action ClearSceneEvent;

        void Awake()
        {

            ScenePacksPath = GeoTools.ScenePackPath;

            ScenePacks = ReadScenePacks(ScenePacksPath, GeoTools.isDataMode);
        }

        void Start()
        {
            SceneManager.sceneLoaded += (Scene s, LoadSceneMode lsm) =>
            {
                worldBoundariesEnable = true;

                FloorBigEnable = true;

                fogEnable = true;

                if (!GeoTools.isBuilding())
                {
                    ClearEnvironment();
                }
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
        /// <param name="scenesPacksPath">地图包路径</param>
        /// <returns></returns>
        public List<SceneFolder> ReadScenePacks(string scenesPacksPath, bool data = false)
        {
            List<SceneFolder> SPs = new List<SceneFolder>() { };
            List<string> scenePaths = new List<string>();

            if (!ModIO.ExistsDirectory("Scenes", data))
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

        public void ReloadScenePacks()
        {
            ReadScenePacks(ScenePacksPath, true);
        }

        public void OpenScenesDirectory()
        {
            GeoTools.OpenDirctory(ScenePacksPath, GeoTools.isDataMode);
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

            LoadScenePack(scenePack);

        }

        /// <summary>
        /// 加载地图包 多人模式下
        /// </summary>
        /// <param name="ScenePack">地图包</param>
        IEnumerator ILoadScenePack_Multiplayer(SceneFolder scenePack)
        {

            yield return null;

            LoadScenePack(scenePack);

        }

        void LoadScenePack(SceneFolder scenePack)
        {
            HideFloorBigy();

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

            HideFloorBigy();

        }

        private void HideFloorBigy()
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
            catch { }
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

                SetWorldBoundaries(WorldBoundaries_Large);

            }
            catch (Exception e)
            {
                GeoTools.Log(e.Message);
                worldBoundariesEnable = !worldBoundariesEnable;
            }

            try
            {
                //多人模式下
                GameObject worldBoundaries = GameObject.Find("WORLD BOUNDARIES");
                //Bounds worldBoundaries = new Bounds();
                //if (WorldBoundaries == null)
                //{
                //    Debug.LogError("Can't find level bounds!");
                //}
                //Collider[] componentsInChildren = WorldBoundaries.GetComponentsInChildren<Collider>(true);
                //for (int i = 0; i < componentsInChildren.Length; i++)
                //{
                //    Bounds bound = componentsInChildren[i].bounds;
                //    if (i != 0)
                //    {
                //        worldBoundaries.Encapsulate(bound);
                //    }
                //    else
                //    {
                //        worldBoundaries = bound;
                //    }
                //}
                //worldBoundaries.Expand(worldBoundaries.extents * 2f * 100f);

                //NetworkCompression.SetWorldBounds(worldBoundaries);
                SetWorldBoundaries(worldBoundaries);

            }
            catch (Exception e)
            {
                GeoTools.Log(e.Message);
                worldBoundariesEnable = !worldBoundariesEnable;
            }

            void SetWorldBoundaries(GameObject WorldBoundaries)
            {
                worldBoundariesEnable = !worldBoundariesEnable;

                foreach (BoxCollider BC in WorldBoundaries.GetComponentsInChildren<BoxCollider>())
                {
                    BC.isTrigger = !worldBoundariesEnable;
                }

                foreach (Renderer MR in WorldBoundaries.GetComponentsInChildren<Renderer>())
                {
                    MR.enabled = worldBoundariesEnable;
                }

            }


        }

        /// <summary>
        /// 隐藏雾
        /// </summary>
        public void HideFog()
        {
            fogEnable = !fogEnable;

            try
            {
                if (SceneManager.GetActiveScene().name == "BARREN EXPANSE" || (SceneManager.GetActiveScene().name == "MasterSceneMultiplayer" && (Level.GetCurrentLevel().Setup.Name == null || Level.GetCurrentLevel().Setup.Name == "")))
                {

                    GameObject mainCamera = GameObject.Find("Main Camera");
                    mainCamera.GetComponent<ColorfulFog>().enabled = fogEnable;
                }
            }
            catch
            { }

            try
            {
                GameObject fogSPHERE = GameObject.Find("FOG SPHERE");
                fogSPHERE.GetComponent<MeshRenderer>().enabled = fogEnable;
            }
            catch
            { }

            try
            {
                GameObject.Find("Fog Volume").GetComponent<MeshRenderer>().enabled = fogEnable;
            }
            catch (Exception e)
            {
                GeoTools.Log(e.Message);
            }

        }
        #endregion
    }



}
