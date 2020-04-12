//using System;
//using System.Collections.Generic;
////using System.IO;
//using System.Text;
//using UnityEngine;

//namespace CustomScene
//{
//    public class SnowMod : EnvironmentMod<SnowPropertise>
//    {

//        List<GameObject> snowObjects;
//        int snowSize = 0;

//        SnowPropertise snowPropertises;

//        public override CustomScene.SnowPropertise Propertise => throw new NotImplementedException();

//        class SnowPropertise
//        {
//            public Vector3 snowPosition = Vector3.zero;
//            public Vector3 snowScale = Vector3.one;
//            public Color snowColor = Color.white;
//            public float snowStartSize = 0f;
//            public float snowStartSpeed = 0f;
//            public int snowMaxParticles = 0;
//        }

//        public override void Read(SceneFolder scenePack)
//        {
//            Clear();

//            try
//            {


//                foreach (var str in scenePack.SettingFileDatas)
//                {
//                    string[] chara = str.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
//                    if (chara.Length >= 2)
//                    {
//                        #region Snow
//                        if (chara[0].ToLower() == "msnow")
//                        {
//                            if (chara[1].ToLower() == "size")
//                            {
//                                snowSize = Convert.ToInt32(chara[2]);
//                                if (snowSize <= 0)
//                                {
//                                    break;
//                                }
//                                snowPropertises = new SnowPropertise();
//                            }
//                        }
//                        else if (chara[0] == "Snow")
//                        {
//                            int i = Convert.ToInt32(chara[1]);

//                            if (chara[2] == "scale")
//                            {
//                                snowPropertises.snowScale = new Vector3(
//                                Convert.ToSingle(chara[3]),
//                                Convert.ToSingle(chara[4]),
//                                Convert.ToSingle(chara[5]));
//                            }
//                            else if (chara[2] == "location")
//                            {
//                                snowPropertises.snowPosition = new Vector3(
//                                Convert.ToSingle(chara[3]),
//                                Convert.ToSingle(chara[4]),
//                                Convert.ToSingle(chara[5]));
//                            }
//                            else if (chara[2] == "color" || chara[2] == "startColor")
//                            {
//                                snowPropertises.snowColor = new Color(
//                                Convert.ToSingle(chara[3]),
//                                Convert.ToSingle(chara[4]),
//                                Convert.ToSingle(chara[5]),
//                                Convert.ToSingle(chara[6]));
//                            }
//                            else if (chara[2] == "Size" || chara[2] == "startSize")
//                            {
//                                snowPropertises.snowStartSize = Convert.ToSingle(chara[3]);
//                            }
//                            else if (chara[2] == "Speed" || chara[2] == "startSpeed")
//                            {
//                                snowPropertises.snowStartSpeed = Convert.ToSingle(chara[3]);
//                            }
//                            else if (chara[2] == "maxParticles")
//                            {
//                                snowPropertises.snowMaxParticles = Convert.ToInt32(chara[3]);
//                            }
//                        }
//                        #endregion
//                    }
//                }
//                GeoTools.Log("Read Snow Completed!");
//            }
//            catch (Exception ex)
//            {
//                GeoTools.Log("Error! Read Snow Failed!");
//                GeoTools.Log(ex.Message);
//                Clear();
//                return;
//            }
//        }
//        public override void Load()
//        {
//            try
//            {
//                if (Mod.prop.SnowTemp == null)
//                {
//                    GeoTools.Log("snow temp null");
//                    return;
//                }

//                snowSize = Mathf.Clamp(snowSize, 0, 1000);

//                if (snowSize <= 0)
//                {

//                    return;
//                }
//                else
//                {
//                    snowObjects = new List<GameObject>();

//                    for (int i = 0; i < snowSize; i++)
//                    {
//                        snowObjects.Add(CreateSnowObject(snowPropertises));
//                    }

//#if DEBUG
//                    GeoTools.Log("Load Snow Successfully");
//#endif
//                }

//            }
//            catch (Exception ex)
//            {
//                GeoTools.Log("Error! Load Snow Failed");
//                GeoTools.Log(ex.Message);
//                Clear();
//            }
//        }
//        public override void Clear()
//        {
//            if (snowObjects == null) return;
//            if (snowObjects.Count <= 0) return;
//#if DEBUG
//            GeoTools.Log("Clear Snow");
//#endif
//            for (int i = 0; i < snowObjects.Count; i++)
//            {
//                UnityEngine.Object.Destroy(snowObjects[i]);
//            }

//            snowObjects = null;
//            snowSize = 0;
//        }

//        GameObject CreateSnowObject(SnowPropertise snowPropertise )
//        {
//            GameObject go = (GameObject)UnityEngine.Object.Instantiate(Mod.prop.SnowTemp);
//            go.name = "Snow Object";
//            go.SetActive(true);
//            go.transform.SetParent(Mod.SceneController.transform);
//            go.transform.localScale = snowPropertise.snowScale;
//            go.transform.localPosition = snowPropertise.snowPosition;

//            ParticleSystem ps = go.GetComponent<ParticleSystem>();
//            ps.startColor = snowPropertise.snowColor;
//            ps.startSize = snowPropertise.snowStartSize;
//            ps.startSpeed = snowPropertise.snowStartSpeed;
//            ps.maxParticles = snowPropertise.snowMaxParticles;

//            return go;
//        }

//    }

//    public class SnowPropertise : EnvironmentPropertise
//    {
//        public Vector3 snowPosition = Vector3.zero;
//        public Vector3 snowScale = Vector3.one;
//        public Color snowColor = Color.white;
//        public float snowStartSize = 0f;
//        public float snowStartSpeed = 0f;
//        public int snowMaxParticles = 0;
//    }
//}
