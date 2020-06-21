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

    public class SceneModController : SingleInstance<SceneModController>
    {
        public override string Name { get; } = "SceneMod Controller";

        /// <summary>地图包路径</summary>
        public static string ScenePacksPath = @"Scenes\";

        public SceneMod CurrentScene { get; private set; }
        public List<SceneMod> Scenes { get; private set; }

        private GameObject floorBig;
        private GameObject levelBoundary;
        private GameObject spWorldBoundary;
        private GameObject mpWorldBoundary;
        private GameObject fogSphere;
        private GameObject mainCamera;
        private ColorfulFog activeColorfulFog;
        public bool worldBoundaryHidden = false;
        public bool floorBigDisabled = false;
        public bool fogDisabled = false;
        private int defaultFarClip = 1500;
        private int noFogFarClip = 1500000;

        //public Action<SceneFolder> ReadSceneEvent;
        public event Action<SceneMod> OnLoadSceneEvent;
        public event Action<SceneMod> OnClearSceneEvent;

        void Awake()
        {
            worldBoundaryHidden = false;
            floorBigDisabled = false;

            Scenes = ReadScenes(ScenePacksPath, true);

            ModConsole.RegisterCommand("csm", new CommandHandler((value) =>
            {
                Dictionary<string, Action<string[]>> commandOfAction = new Dictionary<string, Action<string[]>>
                {
                    { "CreateNewScene".ToLower(),   (args)=>{ if(value[1]!= null&&value[1]!=""){SceneModController.Instance.CreateNewScene(value[1], true);} } },
                    { "LoadScene".ToLower(),   (args)=>{ if(value[1]!= null&&value[1]!=""){SceneModController.Instance.LoadScene(value[1]);} } },
                    { "ClearScene".ToLower(),   (args)=>{ SceneModController.Instance.ClearScene();} },
                    { "RefreshScene".ToLower(),   (args)=>{ SceneModController.Instance.RefreshScenes();} },
                };

                if (commandOfAction.ContainsKey(value[0].ToLower()))
                {
                    commandOfAction[value[0].ToLower()].Invoke(value);
                }
                else
                {
                    Debug.Log(string.Format("Unknown command '{0}', type 'help' for list.", value[0]));
                }
            }),
            "<color=#FF6347>" +
            "Custom Scene Mod Commands\n" +
            "  Usage: csm CreateNewScene <SceneName> :  Create a new custom scene.\n" +
            "  Usage: csm LoadScene <SceneName> :  Load a exist's custom scene.\n" +
            "  Usage: csm ClearScene :  Clear custom scene.\n" +
            "  Usage: csm RefreshScene :  Refresh custom scenes.\n" +
            "</color>"
            );
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                Debug.Log(SceneManager.GetActiveScene().name);
            }
        }

        public List<SceneMod> ReadScenes(string scenesPath, bool data = false)
        {
            var scenes = new HashSet<SceneMod>();

            if (!ModIO.ExistsDirectory(scenesPath, data))
            {
                ModIO.CreateDirectory(scenesPath, data);
            }

            var paths = ModIO.GetDirectories(scenesPath, data);
            if (paths.Length > 0)
            {
                foreach (var path in paths)
                {
                    if (path.ToLower() != "scenes/disabled")
                    {
                        var scene = new SceneMod(path + @"\", data);
                        if (scene.Enabled) scenes.Add(scene);
                    }
                }
            }

            return scenes.ToList();
        }
        public void RefreshScenes()
        {
            Scenes.Clear();
            Scenes = ReadScenes(ScenePacksPath, true);
        }

        public void CreateNewScene(string path, bool data = false)
        {
            path = ScenePacksPath + path;
            if (!Scenes.Exists(match => match.Path == path))
            {
                new SceneMod(path, data);
                RefreshScenes();

                Debug.Log("Create Scene success...");
            }
            else
            {
                Debug.Log("Scene is existed...");
            }
        }

        public void LoadScene(SceneMod scene)
        {
            ClearScene();

            if (scene.isExistPropertiseFile && scene != null)
            {
                //SceneManager.LoadScene("MasterSceneMultiplayer", LoadSceneMode.Single);


                ToggleFloorBig();
                ToggleFog();



                CurrentScene = scene;
                CurrentScene.Load(transform.parent);
                OnLoadSceneEvent?.Invoke(CurrentScene);
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
        public void LoadScene(int index, bool data = false)
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
                OnClearSceneEvent?.Invoke(CurrentScene);
                CurrentScene.Clear();
                CurrentScene = null;
            }
        }

        #region 隐藏/显示地面 空气墙 雾

        /// <summary>
        /// 隐藏地面
        /// </summary>
        public void ToggleFloorBig()
        {
            if (floorBig == null) floorBig = GameObject.Find("FloorBig");
            try
            {
                floorBig.SetActive(floorBigDisabled);
                floorBigDisabled = !floorBigDisabled;
            }
            catch
            {
                Debug.Log("something is wrong setting floor big");
            }
        }

        /// <summary>
        /// 隐藏空气墙
        /// </summary>
        public void ToggleWorldBoundary()
        {
            if (levelBoundary == null) levelBoundary = GameObject.Find("WORLD BOUNDARIES");
            if (spWorldBoundary == null) spWorldBoundary = GameObject.Find("WORLD BOUNDARIES_LARGE");
            if (mpWorldBoundary == null) mpWorldBoundary = GameObject.Find("WORLD BOUNDARIES LARGE");

            try
            {
                levelBoundary.SetActive(worldBoundaryHidden);
                worldBoundaryHidden = !worldBoundaryHidden;
            }
            catch
            {
#if DEBUG
                Debug.Log(levelBoundary == null);
                Debug.Log("something is wrong setting level world boundary");
#endif
            }

            try
            {
                spWorldBoundary.SetActive(worldBoundaryHidden);
                worldBoundaryHidden = !worldBoundaryHidden;
            }
            catch
            {
#if DEBUG
                Debug.Log(spWorldBoundary == null);
                Debug.Log("something is wrong setting sp world boundary");
#endif
            }

            try
            {
                mpWorldBoundary.SetActive(worldBoundaryHidden);
                worldBoundaryHidden = !worldBoundaryHidden;
            }
            catch
            {
#if DEBUG
                Debug.Log(mpWorldBoundary == null);
                Debug.Log("something is wrong setting mp world boundary");
#endif
            }
        }

        /// <summary>
        /// 隐藏雾
        /// </summary>
        public void ToggleFog()
        {

            if (fogSphere == null) fogSphere = GameObject.Find("FOG SPHERE");
            if (mainCamera == null) mainCamera = GameObject.Find("Main Camera");
            if (activeColorfulFog == null && mainCamera != null)
            {
                List<ColorfulFog> colourfulFogs = new List<ColorfulFog>();
                mainCamera.GetComponents(colourfulFogs);
                foreach (var fog in colourfulFogs)
                {
                    if (fog.enabled) activeColorfulFog = fog;
                }
            }

            try
            {
                fogSphere.GetComponent<MeshRenderer>().enabled = fogDisabled;
            }
            catch
            {
#if DEBUG
                Debug.Log(fogSphere == null);
                Debug.Log("something is wrong with fog sphere");
#endif
            }

            try
            {
                activeColorfulFog.enabled = fogDisabled;
            }
            catch
            {
#if DEBUG
                Debug.Log(activeColorfulFog == null);
                Debug.Log("something is wrong with active fog renderer");
#endif
            }

            try
            {
                GameObject[] fogs = FindObjectsOfType<GameObject>();
                foreach (var fog in fogs)
                {
                    if (fog == null) continue;
                    if (fog.name.Contains("Fog"))
                    {
                        Debug.Log(fog.name);
                        MeshRenderer mesh = fog.GetComponent<MeshRenderer>();
                        if (mesh == null) continue;
                        mesh.enabled = fogDisabled;
                    }
                }
            }
            catch
            {
#if DEBUG
                Debug.Log("something is wrong with fog volume");
#endif
            }

            fogDisabled = !fogDisabled;
            Camera.main.farClipPlane = fogDisabled ? noFogFarClip : defaultFarClip;
        }
        #endregion
    }



}
