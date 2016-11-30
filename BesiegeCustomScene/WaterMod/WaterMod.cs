using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BesiegeCustomScene
{
    class WaterMod: MonoBehaviour
    {
        void Start()
        {
            WWW iteratorVariable0 = new WWW("file:///" + GeoTools.ShaderPath+"Water.unity3d.dll");
            iteratorVariable1 = iteratorVariable0.assetBundle;
           /* 
            string[] names = iteratorVariable1.GetAllAssetNames();
            for (int i = 0; i < names.Length; i++)
            {
                Debug.Log(names[i]);
            }
            */
           // GeoTools.PrintShader();
        }
        void OnDisable()
        {
            ClearWater();
        }
        void OnDestroy()
        {
            ClearWater();
        }
        void FixedUpdate()
        {
            if (waterTemp == null)
            {
                waterTemp = (GameObject)UnityEngine.Object.Instantiate(GameObject.Find("Water"));
                Debug.Log(waterTemp.GetComponent<MeshRenderer>().material.shader.name);      
                waterTemp.name = "waterTemp";
                 Debug.Log(waterTemp.GetComponent<MeshRenderer>().material.shaderKeywords.Length);
                for (int i = 0; i < waterTemp.GetComponent<MeshRenderer>().material.shaderKeywords.Length; i++)
                {
                    Debug.Log(waterTemp.GetComponent<MeshRenderer>().material.shaderKeywords[i]);
                }
                DontDestroyOnLoad(waterTemp);
                Debug.Log("Get Water Temp Successfully");
                waterTemp.SetActive(false);
            }
        }
        /// ////////////////////////
        private GameObject waterTemp=null;
        string ScenePath = GeoTools.ScenePath;
        private AssetBundle iteratorVariable1;
        private GameObject[] Mwater;
        private int WaterSize = 0;
        private Vector3 waterScale = new Vector3(0, 1, 0);
        private Vector3 MwaterScale = new Vector3(30, 1, 30);
        private Vector3 waterLocation = new Vector3(0, 0, 0);
        private Quaternion waterRotation = new Quaternion(0, 0, 0, 1);
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
                       #region Water
                            if (chara[0] == "Water")
                        {
                            if (chara[1] == "size")
                            {
                                this.WaterSize = Convert.ToInt32(chara[2]);
                                LoadWater();
                            }
                            else if (chara[1] == "scale")
                            {
                                waterScale = new Vector3(
                                Convert.ToSingle(chara[2]),
                                Convert.ToSingle(chara[3]),
                                Convert.ToSingle(chara[4]));
                            }
                            else if (chara[1] == "mscale")
                            {
                                MwaterScale = new Vector3(
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
                            else if (chara[1] == "roation")
                            {
                                waterRotation = new Quaternion(
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
                Debug.Log("ReadMeshWater Completed!");
            }
            catch (Exception ex)
            {
                Debug.Log("ReadMeshWater Failed!");
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
                if (waterScale.x < 0) waterScale.x = 0;
                if (waterScale.x > 9) waterScale.x = 9;
                if (waterScale.z < 0) waterScale.z = 0;
                if (waterScale.z > 9) waterScale.z = 9;
                Mwater = new GameObject[((int)waterScale.x * 2 + 1) * ((int)waterScale.z * 2 + 1)];
                Mwater[0] = (GameObject)Instantiate(iteratorVariable1.LoadAsset("water4example (advanced)",typeof(GameObject)), waterLocation, new Quaternion());             
                Mwater[0].name = "water0";
               //Debug.Log(Mwater[0].GetComponent<WaterBase>().sharedMaterial.name);
               GeoTools.ResetWaterMaterial(ref Mwater[0].GetComponent<WaterBase>().sharedMaterial);
                Mwater[0].transform.localScale = MwaterScale;
                int index = 1;
                for (float k = -waterScale.x; k <= waterScale.x; k++)
                {
                    for (float i = -waterScale.z; i <= waterScale.z; i++)
                    {
                        if ((k != 0) || (i != 0))
                        {
                            Mwater[index] = Instantiate(Mwater[0]);
                            Mwater[index].name = "water" + index.ToString();
                            Mwater[index].transform.position = new Vector3((float)(k * 50 * Mwater[0].transform.localScale.x) + waterLocation.x,
                                waterLocation.y, (float)(i * 50 * Mwater[0].transform.localScale.z) + waterLocation.z);
                            index++;
                        }
                    }
                }
                if (Mwater.Length == 1) Mwater[0].transform.rotation = this.waterRotation;
            }
            catch (Exception ex)
            {
                Debug.Log("LoadWater Failed");
                Debug.Log(ex.ToString());
            }
        }
        public void ClearWater()
        {
            if (Mwater == null) return;
            if (Mwater.Length <= 0) return;
            Debug.Log("ClearWater");
            for (int i = 0; i < Mwater.Length; i++)
            {
                try {
                    Destroy(GameObject.Find("water" + i.ToString() + "ReflectionMain Camera"));
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
                Debug.Log("LoadFloater Failed");
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
                Debug.Log("ClearFloater Failed");
            }
        }
    }
}
