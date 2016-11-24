using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BesiegeCustomScene
{
    public class TriggerMod : MonoBehaviour
    {
        void Start()
        {
        }
        void OnDisable()
        {
            ClearTrigger();
        }
        void OnDestroy()
        {
            ClearTrigger();
        }
        ////////////////////////////
        private GameObject[] meshtriggers;
        private int TriggerSize = 0;
         string ScenePath = GeoTools.ScenePath;
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
                        #region Triggers
                        if (chara[0] == "Triggers")
                        {
                            if (chara[1] == "size")
                            {
                                this.TriggerSize = Convert.ToInt32(chara[2]);
                                LoadTrigger();
                            }
                        }
                        else if (chara[0] == "Trigger")
                        {
                            int i = Convert.ToInt32(chara[1]);
                            if (chara[2] == "mesh")
                            {
                                meshtriggers[i].GetComponent<MeshFilter>().mesh = GeoTools.MeshFromObj(chara[3]);
                            }
                            else if (chara[2] == "wmesh")
                            {
                                meshtriggers[i].GetComponent<MeshFilter>().mesh = GeoTools.WMeshFromObj(chara[3]);
                            }
                            else if (chara[2] == "scale")
                            {
                                meshtriggers[i].transform.localScale = new Vector3(
                               Convert.ToSingle(chara[3]),
                               Convert.ToSingle(chara[4]),
                               Convert.ToSingle(chara[5]));
                            }
                            else if (chara[2] == "location")
                            {
                                meshtriggers[i].transform.localPosition = new Vector3(
                                Convert.ToSingle(chara[3]),
                                Convert.ToSingle(chara[4]),
                                Convert.ToSingle(chara[5]));
                            }
                            else if (chara[2] == "rotation")
                            {
                                meshtriggers[i].transform.rotation = new Quaternion(
                                Convert.ToSingle(chara[3]),
                                Convert.ToSingle(chara[4]),
                                Convert.ToSingle(chara[5]),
                                Convert.ToSingle(chara[6]));
                            }
                            else if (chara[2] == "eulerangles")
                            {
                                meshtriggers[i].transform.Rotate(new Vector3(
                                Convert.ToSingle(chara[3]),
                                Convert.ToSingle(chara[4]),
                                Convert.ToSingle(chara[5])), Space.Self);
                            }
                            else if (chara[2] == "euleranglesworld")
                            {
                                meshtriggers[i].transform.Rotate(new Vector3(
                                Convert.ToSingle(chara[3]),
                                Convert.ToSingle(chara[4]),
                                Convert.ToSingle(chara[5])), Space.World);
                            }
                            else if (chara[2] == "fromtorotation")
                            {
                                meshtriggers[i].transform.rotation = Quaternion.FromToRotation(
                              new Vector3(Convert.ToSingle(chara[3]),
                                Convert.ToSingle(chara[4]),
                                Convert.ToSingle(chara[5])),
                                new Vector3(Convert.ToSingle(chara[6]),
                                Convert.ToSingle(chara[7]),
                                Convert.ToSingle(chara[8]))
                                );
                            }
                            else if (chara[2] == "shader")
                            {
                                meshtriggers[i].GetComponent<MeshRenderer>().material.shader = Shader.Find("chara[3]");
                            }
                            else if (chara[2] == "texture")
                            {
                                meshtriggers[i].GetComponent<MeshRenderer>().material.mainTexture = GeoTools.LoadTexture(chara[3]);
                            }
                            else if (chara[2] == "stexture")
                            {
                                meshtriggers[i].GetComponent<MeshRenderer>().sharedMaterial.mainTexture = GeoTools.LoadTexture(chara[3]);
                            }
                            else if (chara[2] == "color")
                            {
                                meshtriggers[i].GetComponent<MeshRenderer>().material.color = new Color(
                                Convert.ToSingle(chara[3]),
                                Convert.ToSingle(chara[4]),
                                Convert.ToSingle(chara[5]),
                                Convert.ToSingle(chara[6]));
                            }
                            else if (chara[2] == "meshcollider")
                            {
                                meshtriggers[i].GetComponent<MeshCollider>().sharedMesh = GeoTools.MeshFromObj(chara[3]);
                                meshtriggers[i].GetComponent<MeshCollider>().convex = true;
                                meshtriggers[i].GetComponent<MeshCollider>().isTrigger = true;
                            }
                            else if (chara[2] == "wmeshcollider")
                            {
                                meshtriggers[i].GetComponent<MeshCollider>().sharedMesh = GeoTools.WMeshFromObj(chara[3]);
                                meshtriggers[i].GetComponent<MeshCollider>().convex = true;
                                meshtriggers[i].GetComponent<MeshCollider>().isTrigger = true;
                            }
                            ///////////////////////////////////////////////
                        }
                        #endregion
                    }
                }
                srd.Close();
                Debug.Log("ReadMeshTrigger Completed!");
            }
            catch (Exception ex)
            {
                Debug.Log("ReadMeshTrigger Failed!");
                Debug.Log(ex.ToString());
                return;
            }
        }
        public void LoadTrigger()
        {
            try
            {
                ClearTrigger();
                if (TriggerSize > 100) TriggerSize = 100;
                if (TriggerSize < 0) TriggerSize = 0;
                if (TriggerSize > 0)
                {           
                    meshtriggers = new GameObject[TriggerSize];
                    for (int i = 0; i < meshtriggers.Length; i++)
                    {
                        meshtriggers[i] = GameObject.CreatePrimitive(PrimitiveType.Plane);
                        meshtriggers[i].GetComponent<MeshCollider>().sharedMesh.Clear();
                        meshtriggers[i].GetComponent<MeshFilter>().mesh.Clear();
                        meshtriggers[i].AddComponent<MTrigger>();
                        meshtriggers[i].name = "_meshtrigger" + i.ToString();
                    }
                }
            }
            catch (System.Exception ex)
            {
                Debug.Log("LoadTrigger Failed!");
                Debug.Log(ex.ToString());
            }
        }
        public void ClearTrigger()
        {
            if (meshtriggers == null) return;
            if (meshtriggers.Length <= 0) return;
            Debug.Log("ClearMeshTriggers");
            for (int i = 0; i < meshtriggers.Length; i++)
            {
                Destroy(meshtriggers[i]);
            }
        }
    }
}