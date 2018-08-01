using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BesiegeCustomScene
{
    class CameraMod : EnvironmentMod
    {
        

        /// <summary>摄像头设置参数</summary>
        public CameraPropertise cameraPropertise;

        /// <summary>
        /// 默认的摄像头参数  储存原先的摄像头参数
        /// </summary>
        public CameraPropertise defultePropertise;

        /// <summary>摄像头属性</summary>
        public class CameraPropertise
        {
            public float farClipPlane = 4500f;
            public float focusLerpSmooth = 8f;
            //public bool fog;
            public bool SSAO;

            //public static SceneSetting None { get; } = new SceneSetting { farClipPlane = 3000, focusLerpSmooth = 100, fog = Vector3.one, SSAO = false };
        }

        public override void ReadEnvironment(SceneFolder scenePack)
        {
            ClearEnvironment();

            try
            {
                foreach (var str in scenePack.SettingFileDatas)
                {
                    string[] chara = str.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                    if (chara.Length > 2)
                    {
                        #region Camera
                        if (chara[0].ToLower() == nameof(Camera).ToLower())
                        {

                            cameraPropertise = cameraPropertise ?? new CameraPropertise();

                            if (chara[1].ToLower() == nameof(cameraPropertise.farClipPlane).ToLower())
                            {
                                cameraPropertise.farClipPlane = Convert.ToInt32(chara[2]);
                            }
                            else if (chara[1].ToLower() == nameof(cameraPropertise.focusLerpSmooth).ToLower())
                            {

                                if (chara[2].ToLower() == "infinity")
                                {
                                    cameraPropertise.focusLerpSmooth = float.PositiveInfinity;
                                }
                                else
                                {
                                    cameraPropertise.focusLerpSmooth = Convert.ToInt32(chara[2]);
                                }

                            }                   
                            else if (chara[1].ToLower() == nameof(cameraPropertise.SSAO).ToLower())
                            {
                                //if (chara[2] == "OFF")
                                //{
                                //    GeoTools.Log("SSAO OFF");
                                //    OptionsMaster.SSAO = true;
                                //    FindObjectOfType<ToggleAO>().Set();
                                //}
                                //else if (chara[2] == "ON")
                                //{
                                //    GeoTools.Log("SSAO ON");
                                //    OptionsMaster.SSAO = false;
                                //    FindObjectOfType<ToggleAO>().Set();
                                //}
                                //cameraPropertise.SSAO = dataLoader.readBool(chara, 1);
                            }

                        }
                        #endregion
                    }
                }

                GeoTools.Log("Read Camera Completed!");
            }
            catch (Exception ex)
            {
                GeoTools.Log("Read Camera Failed!");
                GeoTools.Log(ex.Message);
                return;
            }


        }


        public override void LoadEnvironment()
        {
            if (cameraPropertise == null) return;

            defultePropertise = new CameraPropertise();

            GameObject mainCamera = GameObject.Find("Main Camera");

            Camera camera = mainCamera.GetComponent<Camera>();
            defultePropertise.farClipPlane = camera.farClipPlane;
            camera.farClipPlane = cameraPropertise.farClipPlane;

            MouseOrbit mouseOrbit = mainCamera.GetComponent<MouseOrbit>();
            defultePropertise.focusLerpSmooth = mouseOrbit.focusLerpSmooth;
            mouseOrbit.focusLerpSmooth = cameraPropertise.focusLerpSmooth;
        }

        public override void ClearEnvironment()
        {
            if (cameraPropertise == null) return;
            if (defultePropertise == null) return;
            
            GameObject mainCamera = GameObject.Find("Main Camera");

            Camera camera = mainCamera.GetComponent<Camera>();
            camera.farClipPlane = defultePropertise.farClipPlane;

            MouseOrbit mouseOrbit = mainCamera.GetComponent<MouseOrbit>();
            mouseOrbit.focusLerpSmooth = defultePropertise.focusLerpSmooth;

            cameraPropertise = defultePropertise = null;
        }
    }
}
