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


        /// <summary>摄像头属性</summary>
        public class CameraPropertise
        {
            public int farClipPlane;
            public float focusLerpSmooth;
            public Vector3 fog;
            public bool SSAO;

            //public static SceneSetting None { get; } = new SceneSetting { farClipPlane = 3000, focusLerpSmooth = 100, fog = Vector3.one, SSAO = false };
        }

        public override void ReadEnvironment(ScenePack scenePack)
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
                        if (chara[0] == nameof(Camera))
                        {
                            if (chara[1] == nameof(cameraPropertise.farClipPlane))
                            {
                                //try
                                //{
                                //    GameObject.Find("Main Camera").GetComponent<Camera>().farClipPlane = Convert.ToInt32(chara[2]);
                                //}
                                //catch (Exception ex)
                                //{
                                //    GeoTools.Log("farClipPlane Error");
                                //    GeoTools.Log(ex.ToString());
                                //}
                                cameraPropertise.farClipPlane = Convert.ToInt32(chara[2]);
                            }
                            else if (chara[1] == nameof(cameraPropertise.focusLerpSmooth))
                            {
                                try
                                {
                                    if (chara[2] == "Infinity")
                                    {
                                        //GameObject.Find("Main Camera").GetComponent<MouseOrbit>().focusLerpSmooth = float.PositiveInfinity;
                                        cameraPropertise.focusLerpSmooth = float.PositiveInfinity;
                                    }
                                    else
                                    {
                                        //GameObject.Find("Main Camera").GetComponent<MouseOrbit>().focusLerpSmooth = Convert.ToSingle(chara[2]);
                                        cameraPropertise.focusLerpSmooth = Convert.ToSingle(chara[2]);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    GeoTools.Log("focusLerpSmooth Error");
                                    GeoTools.Log(ex.ToString());
                                }
                            }
                            //else if (chara[1] == nameof(cameraPropertise.fog))
                            //{
                            //    try
                            //    {
                            //        //GameObject.Find("Fog Volume").transform.localScale = new Vector3(0, 0, 0);
                            //        cameraPropertise.fog = Vector3.zero;
                            //    }
                            //    catch
                            //    {
                            //        try
                            //        {
                            //            GameObject.Find("Fog Volume Dark").transform.localScale = new Vector3(0, 0, 0);
                            //        }
                            //        catch
                            //        {
                            //            GeoTools.Log("fog error");
                            //        }
                            //    }
                            //}
                            else if (chara[1] == nameof(cameraPropertise.SSAO))
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
            throw new NotImplementedException();
        }

        public override void ClearEnvironment()
        {
            throw new NotImplementedException();
        }
    }
}
