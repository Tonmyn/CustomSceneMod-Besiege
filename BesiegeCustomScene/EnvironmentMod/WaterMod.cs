using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

namespace BesiegeCustomScene
{
    class WaterMod : MonoBehaviour
    {

        void Start()
        {

        }
        void OnDisable()
        {
            ClearWater();
        }
        void OnDestroy()
        {
            ClearWater();
        }
        /// ////////////////////////      
        string ScenePath = GeoTools.ScenePath;
        private GameObject[] Mwater;
        private int WaterSize = 0;
        private Vector3 waterScale = new Vector3(1, 1, 1);
        private Vector3 waterRepeat = new Vector3(0, 1, 0);
        private Vector3 waterLocation = new Vector3(0, 0, 0);
        public void ReadScene(string SceneName)
        {
            WaterSize = 0;
            waterScale = new Vector3(1, 1, 1);
            waterRepeat = new Vector3(0, 1, 0);
            waterLocation = new Vector3(0, 0, 0);
            try
            {
                //  Debug.Log(Application.dataPath);
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
                        #region Water
                        if (chara[0] == "Water")
                        {
                            if (chara[1] == "size")
                            {
                                this.WaterSize = Convert.ToInt32(chara[2]);
                                LoadWater();
                            }
                            else if (chara[1] == "repeat")
                            {
                                waterRepeat = new Vector3(
                                Convert.ToSingle(chara[2]),
                                Convert.ToSingle(chara[3]),
                                Convert.ToSingle(chara[4]));
                            }
                            else if (chara[1] == "mscale" || chara[1] == "scale")
                            {
                                waterScale = new Vector3(
                                Convert.ToSingle(chara[2]),
                                Convert.ToSingle(chara[3]),
                                Convert.ToSingle(chara[4]));
                            }
                            else if (chara[1] == "location")
                            {
                                waterLocation = new Vector3(
                                Convert.ToSingle(chara[2]),
                                Convert.ToSingle(chara[3]),
                                Convert.ToSingle(chara[4]));
                            }
                        }
                        #endregion
                    }
                }
                srd.Close();
                Debug.Log("ReadMeshWater Completed!");
            }
            catch (Exception ex)
            {
                Debug.Log("Error! ReadMeshWater Failed!");
                Debug.Log(ex.ToString());
                return;
            }
        }
        public void LoadWater()
        {
            try
            {
                ClearWater();
                if (WaterSize <= 0) return;
                if (waterRepeat.x < 0) waterRepeat.x = 0;
                if (waterRepeat.x > 9) waterRepeat.x = 9;
                if (waterRepeat.z < 0) waterRepeat.z = 0;
                if (waterRepeat.z > 9) waterRepeat.z = 9;
                Mwater = new GameObject[((int)waterRepeat.x * 2 + 1) * ((int)waterRepeat.z * 2 + 1)];

                //gameObject.GetComponent<Prop>().WaterTemp.SetActive(true);
                int index = 0;
                for (float k = -waterRepeat.x; k <= waterRepeat.x; k++)
                {
                    for (float i = -waterRepeat.z; i <= waterRepeat.z; i++)
                    {
                        Mwater[index] = Instantiate(gameObject.GetComponent<Prop>().TileTemp);
                        Mwater[index].name = "water" + index.ToString();
                        Mwater[index].transform.position = new Vector3((float)(
                            k * 50 * waterScale.x) + waterLocation.x, waterLocation.y, (float)(i * 50 * waterScale.z) + waterLocation.z);
                        Mwater[index].transform.localScale=waterScale;
                        Mwater[index].SetActive(true);
                        index++;
                    }
                }

               // Mwater[0].AddComponent<PlanarReflection>();
              //  Mwater[0].GetComponent<PlanarReflection>().m_ReflectionCamera = Camera.current;
            }
            catch (Exception ex)
            {
                Debug.Log("LoadWater Failed");
                Debug.Log(ex.ToString());
            }
        }
        public void ClearWater()
        {
            Debug.Log("ClearFloater");
            ClearFloater();
            if (Mwater == null) return;
            if (Mwater.Length <= 0) return;
            Debug.Log("ClearWater");
            gameObject.GetComponent<Prop>().WaterTemp.SetActive(false);
            for (int i = 0; i < Mwater.Length; i++)
            {
                try
                {
                    Destroy(GameObject.Find("WaterTempReflectionMain Camera"));
                }
                catch { }
                Destroy(Mwater[i]);
            }
        }
        public void LoadFloater()
        {
            try
            {
                MyBlockInfo[] infoArray = UnityEngine.Object.FindObjectsOfType<MyBlockInfo>();
                foreach (MyBlockInfo info in infoArray)
                {
                    if (info.gameObject.GetComponent<Floater>() == null)
                    {
                        info.gameObject.AddComponent<Floater>();
                    }
                }
            }
            catch
            {
                Debug.Log("Error! LoadFloater Failed");
            }
        }
        public void ClearFloater()
        {
            try
            {
                MyBlockInfo[] infoArray = UnityEngine.Object.FindObjectsOfType<MyBlockInfo>();
                foreach (MyBlockInfo info in infoArray)
                {
                    if (info.gameObject.GetComponent<Floater>() != null)
                    {
                        Destroy(info.gameObject.GetComponent<Floater>());
                    }
                }
            }
            catch
            {
                Debug.Log("Error! ClearFloater Failed");
            }
        }
    }
}
