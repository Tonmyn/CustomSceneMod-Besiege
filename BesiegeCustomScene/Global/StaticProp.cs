using UnityEngine;
using UnityEngine.SceneManagement;

namespace BesiegeCustomScene
{
    public class Prop : MonoBehaviour
    {
        void Start()
        {
            Isstart = false;
        }
        bool Isstart = false;
        public  GameObject cloudTemp = null;
        public  GameObject waterTemp = null;
        public void GetLevelInfo()
        {
            Scene scene1 = SceneManager.GetActiveScene();
            Debug.Log("ActiveScene : " + scene1.rootCount.ToString() + "=>" + scene1.name);
            Debug.Log("SceneCount : " + SceneManager.sceneCountInBuildSettings.ToString());

        }
        public GameObject GetObjectInScene(string ObjectName, string Scene)
        {
            try
            {
                if (SceneManager.GetActiveScene().name != Scene)
            {
                SceneManager.LoadScene(Scene, LoadSceneMode.Single);//打开level
            }      
                GameObject ObjectTemp = (GameObject)UnityEngine.Object.Instantiate(GameObject.Find(ObjectName));
                ObjectTemp.name = ObjectName + "Temp";
                UnityEngine.Object.DontDestroyOnLoad(ObjectTemp);
                Debug.Log("Get " + ObjectName + "Temp Successfully");
                ObjectTemp.SetActive(false);
                return ObjectTemp;
            }
            catch
            {
                Debug.Log("Error! Get " + ObjectName + "Temp Failed");
                return null;
            }

        }
        void FixedUpdate()
        {
            if (!Isstart)
            {
                Isstart = true;
                GetLevelInfo();
                string scene1 = SceneManager.GetActiveScene().name;
                if (cloudTemp == null)
                {
                    cloudTemp=GetObjectInScene("CLoud", "TITLE SCREEN");
                }
                if (waterTemp == null)
                {
                    waterTemp = GetObjectInScene("Water", "TITLE SCREEN");
                }
                if(scene1 != SceneManager.GetActiveScene().name)SceneManager.LoadScene(scene1, LoadSceneMode.Single);
            }
        }
    }
}
