using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BesiegeCustomScene
{
    class CloudMod : MonoBehaviour
    {
        void start()
        {

        }
        void OnDisable()
        {
            ClearCloud();
        }
        void OnDestroy()
        {
            ClearCloud();
        }
        void FixedUpdate()
        {
            if (cloudTemp == null)
            {
                cloudTemp = (GameObject)UnityEngine.Object.Instantiate(GameObject.Find("CLoud"));
                cloudTemp.SetActive(false);
                DontDestroyOnLoad(cloudTemp);
                Debug.Log("Get Cloud Temp Successfully");
            }
            if (clouds != null)
            {
                if (clouds[0] != null)
                {
                    for (int i = 0; i < clouds.Length; i++)
                    {
                        clouds[i].transform.RotateAround(this.transform.localPosition, axis[i], Time.deltaTime);
                        clouds[i].GetComponent<ParticleSystem>().startSize = UnityEngine.Random.Range(30, 200);
                    }
                }
            }
        }
        string ScenePath = GeoTools.ScenePath;
        private GameObject[] clouds;
        private Vector3[] axis;
        private int CloudSize = 0;
        private GameObject cloudTemp;
        private Color CloudsColor = new Color(1f, 1f, 1f, 1);
        private Vector3 cloudScale = new Vector3(1000, 200, 1000);
        public void ReadScene(string SceneName)
        {
            try
            {
                Debug.Log(Application.dataPath);
                if (!File.Exists(ScenePath + SceneName + ".txt"))
                {
                    Debug.Log("Scene File not exists!");
                    return;
                }
                StreamReader srd = File.OpenText(ScenePath + SceneName + ".txt");
                while (srd.Peek() != -1)
                {
                    string str = srd.ReadLine();
                    string[] chara = str.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    if (chara.Length > 2)
                    {
                        #region Cloud
                        if (chara[0] == "Cloud")
                        {

                            if (chara[1] == "size")
                            {
                                this.CloudSize = Convert.ToInt32(chara[2]);
                                LoadCloud();
                            }
                            else if (chara[1] == "floorScale" || chara[1] == "cloudScale")
                            {
                                cloudScale = new Vector3(
                                Convert.ToSingle(chara[2]),
                                Convert.ToSingle(chara[3]),
                                Convert.ToSingle(chara[4]));
                            }
                            else if (chara[1] == "location")
                            {
                                this.transform.localPosition = new Vector3(
                                Convert.ToSingle(chara[2]),
                                Convert.ToSingle(chara[3]),
                                Convert.ToSingle(chara[4]));
                            }
                            else if (chara[1] == "color")
                            {
                                this.CloudsColor = new Color(
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
                Debug.Log("ReadCloud Completed!");
            }
            catch (Exception ex)
            {
                Debug.Log("ReadCloud Failed!");
                Debug.Log(ex.ToString());
                return;
            }
        }
        public void LoadCloud()
        {
            try
            {
                ClearCloud();
                if (cloudTemp == null) return;
                if (CloudSize < 0) CloudSize = 0;
                if (CloudSize > 1000) CloudSize = 1000;
                if (CloudSize == 0) { return; }
                else
                {
                    clouds = new GameObject[CloudSize];
                    axis = new Vector3[CloudSize];
                    for (int i = 0; i < clouds.Length; i++)
                    {
                        clouds[i] = (GameObject)UnityEngine.Object.Instantiate(cloudTemp, new Vector3(
                            UnityEngine.Random.Range(-cloudScale.x + transform.localPosition.x, cloudScale.x + transform.localPosition.x),
                            UnityEngine.Random.Range(transform.localPosition.y, cloudScale.y + transform.localPosition.y),
                            UnityEngine.Random.Range(-cloudScale.z + transform.localPosition.z, cloudScale.z + transform.localPosition.z)),
                            new Quaternion(0, 0, 0, 0));
                        clouds[i].transform.SetParent(this.transform);
                        clouds[i].transform.localScale = new Vector3(15, 15, 15);
                        clouds[i].SetActive(true);
                        clouds[i].GetComponent<ParticleSystem>().startColor = CloudsColor;
                        clouds[i].GetComponent<ParticleSystem>().startSize = 30;
                        clouds[i].GetComponent<ParticleSystem>().startLifetime = 6;
                        clouds[i].GetComponent<ParticleSystem>().startSpeed = 1.6f;
                        clouds[i].GetComponent<ParticleSystem>().maxParticles = 18;
                        axis[i] = new Vector3(UnityEngine.Random.Range(-0.1f, 0.1f), 1f, UnityEngine.Random.Range(-0.1f, 0.1f));
                    }
                    Debug.Log("LoadCloud Successfully");
                }
            }
            catch (Exception ex)
            {
                Debug.Log("LoadCloud Failed");
                Debug.Log(ex.ToString());
                ClearCloud();
            }
        }
        public void ClearCloud()
        {
            CloudsColor = new Color(1f, 1f, 1f, 1);
            if (clouds == null) return;
            if (clouds.Length <= 0) return;
            Debug.Log("ClearCloud");
            for (int i = 0; i < clouds.Length; i++)
            {
                Destroy(clouds[i]);
            }
        }

    }
}
