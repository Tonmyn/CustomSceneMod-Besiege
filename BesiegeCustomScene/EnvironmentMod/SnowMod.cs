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
        string ScenePath = GeoTools.ScenePath;
        private GameObject[] Snow;
        private int SnowSize = 0;
        private Color SnowColor = new Color(1f, 1f, 1f, 1);
        private Vector3 SnowScale = new Vector3(0, 1, 0);//repeat times
        public Vector3 mscale;
        public Vector3 mlocation;
        public void ReadScene(string SceneName)
        {
            try
            {
                //Debug.Log(Application.dataPath);
                if (!File.Exists(ScenePath + SceneName + ".txt"))
                {
                    Debug.Log("Error! Scene File not exists!");
                    return;
                }
                StreamReader srd = File.OpenText(ScenePath + SceneName + ".txt");
                while (srd.Peek() != -1)
                {
                    string str = srd.ReadLine();
                    string[] chara = str.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    if (chara.Length > 2)
                    {
                        #region Snow
                        if (chara[0] == "Snow")
                        {

                            if (chara[1] == "size")
                            {
                                this.SnowSize = Convert.ToInt32(chara[2]);
                                LoadCloud();
                            }
                            else if (chara[1] == "snowScale"|| chara[1] == "SnowScale")
                            {
                                this.SnowScale = new Vector3(
                                Convert.ToSingle(chara[2]),
                                Convert.ToSingle(chara[3]),
                                Convert.ToSingle(chara[4]));
                            }
                            else if (chara[1] == "scale")
                            {
                                mscale = new Vector3(
                                Convert.ToSingle(chara[2]),
                                Convert.ToSingle(chara[3]),
                                Convert.ToSingle(chara[4]));
                            }
                            else if (chara[1] == "location")
                            {
                                mlocation = new Vector3(
                                Convert.ToSingle(chara[2]),
                                Convert.ToSingle(chara[3]),
                                Convert.ToSingle(chara[4]));
                            }
                            else if (chara[1] == "color")
                            {
                                this.SnowColor = new Color(
                                Convert.ToSingle(chara[2]),
                                Convert.ToSingle(chara[3]),
                                Convert.ToSingle(chara[4]),
                                Convert.ToSingle(chara[5]));
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
        public void LoadCloud()
        {
            try
            {
                ClearSnow();
                if (this.gameObject.GetComponent<Prop>().snowTemp == null) return;
                if (SnowSize <= 0) return;
                Snow = new GameObject[((int)SnowScale.x * 2 + 1) * ((int)SnowScale.z * 2 + 1)];

                Snow[0] = (GameObject)UnityEngine.Object.Instantiate(this.gameObject.GetComponent<Prop>().snowTemp);
                Snow[0].transform.localPosition = this.mlocation;
                Snow[0].transform.localScale = this.mscale;
                Snow[0].SetActive(true);
                if (!SnowColor.Equals(new Color(1, 1, 1, 1))) Snow[0].GetComponent<ParticleSystem>().startColor = SnowColor;
                Snow[0].GetComponent<ParticleSystem>().startSize = 0.5f;
                // Snow[index].GetComponent<ParticleSystem>().startLifetime = 6;
                // Snow[index].GetComponent<ParticleSystem>().startSpeed = 1.6f;
                Snow[0].GetComponent<ParticleSystem>().maxParticles = 100000;
                Snow[0].name = "snow0";
                int index = 1;
                for (float k = -SnowScale.x; k <= SnowScale.x; k++)
                {
                    for (float i = -SnowScale.z; i <= SnowScale.z; i++)
                    {
                        if ((k != 0) || (i != 0))
                        {
                            Snow[index] = (GameObject)Instantiate(Snow[0]);
                            Snow[index].transform.localPosition = new Vector3((float)(k * 30 * Snow[0].transform.localScale.x) + mlocation.x,
                                mlocation.y, (float)(i * 30 * Snow[0].transform.localScale.y) + mlocation.z);
                            Snow[index].name = "snow" + index.ToString();
                            index++;
                        }
                    }
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
            SnowColor = new Color(1f, 1f, 1f, 1);
            if (Snow == null) return;
            if (Snow.Length <= 0) return;
            Debug.Log("ClearSnow");
            for (int i = 0; i < Snow.Length; i++)
            {
                Destroy(Snow[i]);
            }
        }

    }
}
