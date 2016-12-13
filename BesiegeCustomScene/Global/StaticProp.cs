using System;
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
        public  GameObject cloudTemp = null;
        public  GameObject waterTemp = null;
        public GameObject iceTemp = null;
        public void GetLevelInfo()
        {
            Scene scene1 = SceneManager.GetActiveScene();
            Debug.Log("ActiveScene : " + scene1.rootCount.ToString() + "=>" + scene1.name);
            Debug.Log("SceneCount : " + SceneManager.sceneCountInBuildSettings.ToString());

        }
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
            catch(Exception ex)
            {
                Debug.Log("Error! Get " + ObjectName + "Temp Failed");
                Debug.Log(ex.ToString());
                return null;
            }

        }
        string StartedScene = "";
        void FixedUpdate()
        {
            if (Isstart == 0) { StartedScene = SceneManager.GetActiveScene().name; }
            if (Isstart == 10) { OpenScene("TITLE SCREEN");}
            if (Isstart == 20) { cloudTemp = GetObjectInScene("CLoud"); }
            //if (Isstart == 30) { OpenScene("21"); Isstart++; }
           // if (Isstart == 40) { iceTemp = GetObjectInScene("LargeCrystal");Debug.Log(iceTemp.GetComponent<Renderer>().material.shader.name); }
            if (Isstart == 30) { OpenScene(StartedScene); }
            if(Isstart<=100)    Isstart++;
        }
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q) && Input.GetKey(KeyCode.LeftControl))
            {
                GetLevelInfo();
            }
           
        }
    }
}
