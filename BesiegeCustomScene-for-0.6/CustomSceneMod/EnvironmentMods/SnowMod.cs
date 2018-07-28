using System;
using System.Collections.Generic;
//using System.IO;
using System.Text;
using UnityEngine;

namespace BesiegeCustomScene
{
    public class SnowMod : EnvironmentMod
    {

        //void OnDisable()
        //{
        //    ClearSnow();
        //}
        //void OnDestroy()
        //{
        //    ClearSnow();
        //}

        List<GameObject> snowObjects;
        int snowSize = 0;

        SnowPropertise[] snowPropertises;
        class SnowPropertise
        {

            public Vector3 snowPosition = Vector3.zero;
            public Vector3 snowScale = Vector3.one;
            public Color snowColor = Color.white;
            public float snowStartSize = 0f;
            public float snowStartSpeed = 0f;
            public int snowMaxParticles = 0;

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
                        #region Snow
                        if (chara[0] == "MSnow" || chara[0] == "Msnow")
                        {

                            if (chara[1] == "size")
                            {
                                snowSize = Convert.ToInt32(chara[2]);
                                //snowObjects.AddRange(new GameObject[10]);
                                snowPropertises = new SnowPropertise[snowSize];
                                //LoadSnow();
                            }
                        }
                        else if (chara[0] == "Snow")
                        {
                            int i = Convert.ToInt32(chara[1]);

                            if (chara[2] == "scale")
                            {
                                snowPropertises[i].snowScale = new Vector3(
                                Convert.ToSingle(chara[3]),
                                Convert.ToSingle(chara[4]),
                                Convert.ToSingle(chara[5]));
                            }
                            else if (chara[2] == "location")
                            {
                                snowPropertises[i].snowPosition = new Vector3(
                                Convert.ToSingle(chara[3]),
                                Convert.ToSingle(chara[4]),
                                Convert.ToSingle(chara[5]));
                            }
                            else if (chara[2] == "color" || chara[2] == "startColor")
                            {
                                snowPropertises[i].snowColor = new Color(
                                Convert.ToSingle(chara[3]),
                                Convert.ToSingle(chara[4]),
                                Convert.ToSingle(chara[5]),
                                Convert.ToSingle(chara[6]));
                            }
                            else if (chara[2] == "Size" || chara[2] == "startSize")
                            {
                                snowPropertises[i].snowStartSize = Convert.ToSingle(chara[3]);
                            }
                            else if (chara[2] == "Speed" || chara[2] == "startSpeed")
                            {
                                snowPropertises[i].snowStartSpeed = Convert.ToSingle(chara[3]);
                            }
                            else if (chara[2] == "maxParticles")
                            {
                                snowPropertises[i].snowMaxParticles = Convert.ToInt32(chara[3]);
                            }
                        }
                        #endregion
                    }
                }
                GeoTools.Log("Read Snow Completed!");
            }
            catch (Exception ex)
            {
                GeoTools.Log("Error! Read Snow Failed!");
                GeoTools.Log(ex.Message);
                ClearEnvironment();
                return;
            }
        }
        public override void LoadEnvironment()
        {
            try
            {       
                if (this.gameObject.GetComponent<Prop>().SnowTemp == null) return;

                snowSize = Mathf.Clamp(snowSize, 0, 1000);

                if (snowSize == 0)
                {
                    return;
                }
                else
                {
                    snowObjects = new List<GameObject>();

                    for (int i = 0; i < snowSize; i++)
                    {
                        snowObjects.Add(createSnowObject(snowPropertises[i]));
                    }

#if DEBUG
                    GeoTools.Log("Load Snow Successfully");
#endif
                }

            }
            catch (Exception ex)
            {
                GeoTools.Log("Error! Load Snow Failed");
                GeoTools.Log(ex.Message);
                ClearEnvironment();
            }
        }
        public override void ClearEnvironment()
        {
            if (snowObjects == null) return;
            if (snowObjects.Count <= 0) return;
#if DEBUG
            GeoTools.Log("Clear Snow");
#endif
            for (int i = 0; i < snowObjects.Count; i++)
            {
                Destroy(snowObjects[i]);
            }

            snowObjects = null;
            snowSize = 0;
        }

        GameObject createSnowObject(SnowPropertise snowPropertise )
        {
            GameObject go = (GameObject)Instantiate(gameObject.GetComponent<Prop>().SnowTemp);
            go.name = "Snow Object";
            go.SetActive(true);
            go.transform.SetParent(transform);
            go.transform.localScale = snowPropertise.snowScale;
            go.transform.localPosition = snowPropertise.snowPosition;

            ParticleSystem ps = go.GetComponent<ParticleSystem>();
            ps.startColor = snowPropertise.snowColor;
            ps.startSize = snowPropertise.snowStartSize;
            ps.startSpeed = snowPropertise.snowStartSpeed;
            ps.maxParticles = snowPropertise.snowMaxParticles;

            return go;
        }

    }
}
