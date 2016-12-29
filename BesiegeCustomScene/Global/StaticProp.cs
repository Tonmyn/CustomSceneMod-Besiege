using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BesiegeCustomScene
{
    public class Prop : MonoBehaviour
    {
        void Start()
        {
            Isstart = 0;
        }
        private int Isstart = 0;
        public GameObject cloudTemp = null;
        public List<GameObject> MaterialTemp = new List<GameObject>();
        public GameObject iceTemp = null;
        public GameObject snowTemp = null;
        public GameObject GetObjectInScene(string ObjectName)
        {
            try
            {
                GameObject ObjectTemp = (GameObject)Instantiate(GameObject.Find(ObjectName));
                ObjectTemp.name = ObjectName + "Temp";
                UnityEngine.Object.DontDestroyOnLoad(ObjectTemp);
                Debug.Log("Get " + ObjectName + "Temp Successfully");
                ObjectTemp.SetActive(false);
                return ObjectTemp;
            }
            catch (Exception ex)
            {
                Debug.Log("Error! Get " + ObjectName + "Temp Failed");
                Debug.Log(ex.ToString());
                return null;
            }
        }
        private string StartedScene = "";
        public static double t = 2;
        void FixedUpdate()
        {
            try
            {
                if (Isstart == 1) {
                    StartedScene = SceneManager.GetActiveScene().name;
                    GeoTools.OpenScene("TITLE SCREEN");
                    cloudTemp = GetObjectInScene("CLoud");
                    GameObject matTemp = GetObjectInScene("Water");
                    if (matTemp != null) this.MaterialTemp.Add(matTemp);
                }
                //if (Isstart == 30) { OpenScene("21"); Isstart++; }
                // if (Isstart == 40) { iceTemp = GetObjectInScene("LargeCrystal");Debug.Log(iceTemp.GetComponent<Renderer>().material.shader.name); }
                if (Isstart == 5 * t) { GeoTools.OpenScene("36"); }
                if (Isstart == 10 * t) { snowTemp = GetObjectInScene("Snow"); }
                if (Isstart == 20 * t) { GeoTools.OpenScene(StartedScene); }
                if (Isstart == 30 * t)
                {
                    GameObject WM = GameObject.CreatePrimitive(PrimitiveType.Plane);
                    WM.GetComponent<Renderer>().material.shader = Shader.Find("Standard");
                    WM.GetComponent<Renderer>().material.SetFloat("_Glossiness", 1);
                    WM.GetComponent<Renderer>().material.mainTexture = GeoTools.LoadTexture("WM1");
                    WM.name = "WMTemp";
                    UnityEngine.Object.DontDestroyOnLoad(WM);
                    Debug.Log("Get " + WM.name + "Temp Successfully");
                    WM.SetActive(false);
                    this.MaterialTemp.Add(WM);
                }
                if (Isstart == 60 * t)
                {
                  
                }
            }
            catch (Exception ex)
            {
                Debug.Log(ex.ToString());
            }
            if (Isstart <= 90 * t) Isstart++;
        }
    }
}
