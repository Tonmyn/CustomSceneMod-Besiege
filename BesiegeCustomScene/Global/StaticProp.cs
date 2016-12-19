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
        int Isstart = 0;
        public GameObject cloudTemp = null;
        public List<GameObject> MaterialTemp = new List<GameObject>();
        public GameObject iceTemp = null;
        public GameObject snowTemp = null;
      
        public void OpenScene(string Scene)
        {
            if (SceneManager.GetActiveScene().name != Scene)
            {
                SceneManager.LoadScene(Scene, LoadSceneMode.Single);//打开level
            }
        }
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
        string StartedScene = "";
        static double t = 1;
        void FixedUpdate()
        {
            if (Isstart == 0) { StartedScene = SceneManager.GetActiveScene().name; }
            if (Isstart == 10 * t) { OpenScene("TITLE SCREEN"); }
            if (Isstart == 20 * t) { cloudTemp = GetObjectInScene("CLoud"); }
            if (Isstart == 30 * t)
            {
                GameObject matTemp = GetObjectInScene("Water");
                if (matTemp != null) this.MaterialTemp.Add(matTemp);
            }
            //if (Isstart == 30) { OpenScene("21"); Isstart++; }
            // if (Isstart == 40) { iceTemp = GetObjectInScene("LargeCrystal");Debug.Log(iceTemp.GetComponent<Renderer>().material.shader.name); }
            if (Isstart == 40 * t) { OpenScene("36"); Isstart++; }
            if (Isstart == 50 * t) { snowTemp = GetObjectInScene("Snow"); }
            if (Isstart == 60 * t) { OpenScene(StartedScene); }
            if (Isstart <= 100 * t) Isstart++;
        }     
    }
}
