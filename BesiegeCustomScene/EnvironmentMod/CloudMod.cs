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
        void Start()
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
            
            if (clouds != null)
            {
                if (clouds[0] != null)
                {             
                        for (int i = 0; i < clouds.Length; i++)
                        {
                            clouds[i].transform.RotateAround(this.transform.localPosition, axis[i], Time.deltaTime * 0.3f);
                            clouds[i].GetComponent<ParticleSystem>().startSize = UnityEngine.Random.Range(30, 200);
                        }          
                }
            }
        }
        string ScenePath = GeoTools.ScenePath;
        private GameObject[] clouds;
        private Vector3[] axis;
        private int CloudSize = 0;
        private int cloudstep = 1200;
     //   private int step = 0;
        private Color CloudsColor = new Color(0.92f, 0.92f, 0.92f, 0.5f);
        private Vector3 cloudScale = new Vector3(1000, 200, 1000);
        public void ReadScene(string SceneName)
        {
            CloudsColor = new Color(0.9f, 0.9f, 0.9f, 0.6f);
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
                        #region Cloud
                        if (chara[0] == "Cloud")
                        {

                            if (chara[1] == "size")
                            {
                                this.CloudSize = Convert.ToInt32(chara[2]);
                                LoadCloud();
                            }
                            if (chara[1] == "step")
                            {
                                this.cloudstep = Convert.ToInt32(chara[2]);             
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
                Debug.Log("Error! ReadCloud Failed!");
                Debug.Log(ex.ToString());
                return;
            }
        }
        public void LoadCloud()
        {
            try
            {
                ClearCloud();
                if (this.gameObject.GetComponent<Prop>().CloudTemp == null) return;
                if (CloudSize < 0) CloudSize = 0;
                if (CloudSize > 1000) CloudSize = 1000;
                if (CloudSize == 0) { return; }
                else
                {
                    clouds = new GameObject[CloudSize];
                    axis = new Vector3[CloudSize];
                    for (int i = 0; i < clouds.Length; i++)
                    {
                        clouds[i] = (GameObject)Instantiate(this.gameObject.GetComponent<Prop>().CloudTemp, new Vector3(
                            UnityEngine.Random.Range(-cloudScale.x + transform.localPosition.x, cloudScale.x + transform.localPosition.x),
                            UnityEngine.Random.Range(transform.localPosition.y, cloudScale.y + transform.localPosition.y),
                            UnityEngine.Random.Range(-cloudScale.z + transform.localPosition.z, cloudScale.z + transform.localPosition.z)),
                            new Quaternion(0, 0, 0, 0));
                        clouds[i].transform.SetParent(this.transform);
                        clouds[i].transform.localScale = new Vector3(100, 100, 100);
                        clouds[i].SetActive(true);
                        ParticleSystem ps = clouds[i].GetComponent<ParticleSystem>();
                        ps.startColor = CloudsColor;
                        axis[i] = new Vector3(UnityEngine.Random.Range(-0.1f, 0.1f), 1f, UnityEngine.Random.Range(-0.1f, 0.1f));
                    }
                   // Debug.Log("LoadCloud Successfully");
                }
            }
            catch (Exception ex)
            {
                Debug.Log("Error! LoadCloud Failed");
                Debug.Log(ex.ToString());
                ClearCloud();
            }
        }
        public void ClearCloud()
        {
           // step = 0;
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
