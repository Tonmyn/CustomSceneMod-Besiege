using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BesiegeCustomScene
{
    class SnowMod : MonoBehaviour
    {
        void Start()
        {
        }
        void OnDisable()
        {
            ClearSnow();
        }
        void OnDestroy()
        {
            ClearSnow();
        }
        private GameObject[] MSnow;
        private int SnowSize = 0;
        public void ReadScene(string SceneName)
        {
            try
            {
                //Debug.Log(Application.dataPath);
                if (!File.Exists(GeoTools.ScenePath + SceneName + ".txt"))
                {
                    Debug.Log("Error! Scene File not exists!");
                    return;
                }
                StreamReader srd = File.OpenText(GeoTools.ScenePath + SceneName + ".txt");
                while (srd.Peek() != -1)
                {
                    string str = srd.ReadLine();
                    string[] chara = str.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    if (chara.Length > 2)
                    {
                        #region Snow
                        if (chara[0] == "MSnow" || chara[0] == "Msnow")
                        {

                            if (chara[1] == "size")
                            {
                                this.SnowSize = Convert.ToInt32(chara[2]);
                                LoadSnow();
                            }
                        }
                        else if (chara[0] == "Snow")
                        {
                            int i = Convert.ToInt32(chara[1]);

                            if (chara[2] == "scale")
                            {
                                MSnow[i].transform.localScale = new Vector3(
                                Convert.ToSingle(chara[3]),
                                Convert.ToSingle(chara[4]),
                                Convert.ToSingle(chara[5]));
                            }
                            else if (chara[2] == "location")
                            {
                                MSnow[i].transform.localPosition = new Vector3(
                                Convert.ToSingle(chara[3]),
                                Convert.ToSingle(chara[4]),
                                Convert.ToSingle(chara[5]));
                            }
                            else if (chara[2] == "color" || chara[2] == "startColor")
                            {
                                MSnow[i].GetComponent<ParticleSystem>().startColor = new Color(
                                Convert.ToSingle(chara[3]),
                                Convert.ToSingle(chara[4]),
                                Convert.ToSingle(chara[5]),
                                Convert.ToSingle(chara[6]));
                            }
                            else if (chara[2] == "Size" || chara[2] == "startSize")
                            {
                                MSnow[i].GetComponent<ParticleSystem>().startSize = Convert.ToSingle(chara[3]);
                            }
                            else if (chara[2] == "Speed" || chara[2] == "startSpeed")
                            {
                                MSnow[i].GetComponent<ParticleSystem>().startSpeed = Convert.ToSingle(chara[3]);
                            }
                            else if (chara[2] == "maxParticles")
                            {
                                MSnow[i].GetComponent<ParticleSystem>().maxParticles = Convert.ToInt32(chara[3]);
                            }
                        }
                        #endregion
                    }
                }
                srd.Close();
                Debug.Log("ReadSnow Completed!");
            }
            catch (Exception ex)
            {
                Debug.Log("Error! ReadSnow Failed!");
                Debug.Log(ex.ToString());
                return;
            }
        }
        public void LoadSnow()
        {
            try
            {
                ClearSnow();
                if (this.gameObject.GetComponent<Prop>().snowTemp == null) return;
                if (SnowSize <= 0) return;
                MSnow = new GameObject[SnowSize];
                for (int i = 0; i <= MSnow.Length; i++)
                {
                    MSnow[i] = (GameObject)Instantiate(gameObject.GetComponent<Prop>().snowTemp);
                    MSnow[i].name = "snow" + i.ToString();
                    MSnow[i].SetActive(true);
                    MSnow[i].transform.localScale = new Vector3(1, 1, 1);
                    MSnow[i].transform.localPosition = new Vector3(0, 0, 0);
                }

            }
            catch (Exception ex)
            {
                Debug.Log("Error! LoadSnow Failed");
                Debug.Log(ex.ToString());
                ClearSnow();
            }
        }
        public void ClearSnow()
        {

            if (MSnow == null) return;
            if (MSnow.Length <= 0) return;
            Debug.Log("ClearSnow");
            for (int i = 0; i < MSnow.Length; i++)
            {
                Destroy(MSnow[i]);
            }
        }

    }
}
