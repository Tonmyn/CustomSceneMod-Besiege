using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace BesiegeCustomScene
{
    class MeshMod : MonoBehaviour
    {
        void Start()
        {

        }
        void OnDisable()
        {
            ClearMeshes();
        }
        void OnDestroy()
        {
            ClearMeshes();
        }
        /// <summary>
        /// /////////////////////////
        /// </summary>
        string ScenePath = GeoTools.ScenePath;
        private GameObject[] meshes;
        private int MeshSize = 0;
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
                        #region Meshes
                        if (chara[0] == "Meshes")
                        {
                            if (chara[1] == "size")
                            {
                                this.MeshSize = Convert.ToInt32(chara[2]);
                                LoadMesh();
                            }
                        }
                        else if (chara[0] == "MeshLargeObj")
                        {
                            string[] argus = chara[1].Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                            if (argus[0] == "mesh")
                            {
                                string[] argus2 = chara[2].Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                                string[] argus3 = chara[3].Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                                string[] argus4 = chara[4].Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                                Vector3 position = new Vector3(Convert.ToInt32(argus3[1]), Convert.ToInt32(argus3[2]), Convert.ToInt32(argus3[3]));
                                Vector3 scale = new Vector3(Convert.ToInt32(argus4[1]), Convert.ToInt32(argus4[2]), Convert.ToInt32(argus4[3]));
                                List<Mesh> meshList = GeoTools.MeshFromLargeObj(argus[1], Convert.ToInt32(argus[2]));
                                if (meshList.Count > 0)
                                {
                                    for (int k = 0; k < meshList.Count; k++)
                                    {
                                        for (int j = 0; j < meshes.Length; j++)
                                        {
                                            Mesh mesh1 = meshes[j].GetComponent<MeshFilter>().mesh;
                                            if (mesh1 == null || mesh1.vertices.Length <= 0)
                                            {
                                                meshes[j].GetComponent<MeshFilter>().mesh = meshList[k];
                                                meshes[j].transform.position = position;
                                                meshes[j].transform.localScale = scale;
                                                if(argus2.Length>=3)  meshes[j].GetComponent<MeshRenderer>().material.shader = Shader.Find(argus2[2]);                         
                                               // if (argus2.Length >= 4) meshes[j].GetComponent<MeshRenderer>().material.SetFloat("_Mode", Convert.ToSingle(argus2[3]));
                                               // Debug.Log("meshes" + j.ToString() + " RenderingMode: " + meshes[j].GetComponent<MeshRenderer>().material.GetFloat("_Mode"));
                                                meshes[j].GetComponent<MeshRenderer>().material.mainTexture = GeoTools.LoadTexture(argus2[1]);
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            else if (argus[0] == "meshcollider")
                            {
                                string[] argus2 = chara[2].Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                                string[] argus3 = chara[3].Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                                Vector3 position = new Vector3(Convert.ToInt32(argus2[1]), Convert.ToInt32(argus2[2]), Convert.ToInt32(argus2[3]));
                                Vector3 scale = new Vector3(Convert.ToInt32(argus3[1]), Convert.ToInt32(argus3[2]), Convert.ToInt32(argus3[3]));
                                List<Mesh> meshList = GeoTools.MeshFromLargeObj(argus[1], Convert.ToInt32(argus[2]));
                                if (meshList.Count > 0)
                                {
                                    for (int k = 0; k < meshList.Count; k++)
                                    {
                                        for (int j = 0; j < meshes.Length; j++)
                                        {
                                            Mesh mesh1 = meshes[j].GetComponent<MeshCollider>().sharedMesh;
                                            if (mesh1 == null || mesh1.vertices.Length <= 0)
                                            {
                                                meshes[j].GetComponent<MeshCollider>().sharedMesh = meshList[k];
                                                meshes[j].transform.position = position;
                                                meshes[j].transform.localScale = scale;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else if (chara[0] == "Mesh")
                        {
                            int i = Convert.ToInt32(chara[1]);
                            if (chara[2] == "mesh")
                            {
                                meshes[i].GetComponent<MeshFilter>().mesh = GeoTools.MeshFromObj(chara[3]);
                            }
                            else if (chara[2] == "wmesh")
                            {
                                meshes[i].GetComponent<MeshFilter>().mesh = GeoTools.WMeshFromObj(chara[3]);
                            }
                            else if (chara[2] == "emesh")
                            {
                                meshes[i].GetComponent<MeshFilter>().mesh = GeoTools.EMeshFromObj(chara[3]);
                            }
                            else if (chara[2] == "heightmapmesh")
                            {
                                Mesh mesh = GeoTools.LoadHeightMap(
                                Convert.ToSingle(chara[3]),
                                Convert.ToSingle(chara[4]),
                                Convert.ToInt32(chara[5]),
                                Convert.ToInt32(chara[6]),
                                Convert.ToInt32(chara[7]),
                                Convert.ToSingle(chara[8]),
                                chara[9]);
                                meshes[i].GetComponent<MeshFilter>().mesh = mesh;
                                meshes[i].GetComponent<MeshCollider>().sharedMesh = mesh;
                            }
                            else if (chara[2] == "plannarmesh")
                            {
                                Mesh mesh = GeoTools.MeshFromPoints(
                                Convert.ToInt32(chara[3]),
                                Convert.ToInt32(chara[4]),
                                Convert.ToSingle(chara[5]),
                                Convert.ToSingle(chara[6]));
                                meshes[i].GetComponent<MeshFilter>().mesh = mesh;
                                meshes[i].GetComponent<MeshCollider>().sharedMesh = mesh;
                            }
                            else if (chara[2] == "plannarmeshcollider")
                            {
                                Mesh mesh = GeoTools.MeshFromPoints(
                                Convert.ToInt32(chara[3]),
                                Convert.ToInt32(chara[4]),
                                Convert.ToSingle(chara[5]),
                                Convert.ToSingle(chara[6]));
                                // meshes[i].GetComponent<MeshFilter>().mesh = mesh;
                                meshes[i].GetComponent<MeshCollider>().sharedMesh = mesh;
                            }
                            else if (chara[2] == "heightmapmeshcollider")
                            {
                                Mesh mesh = GeoTools.LoadHeightMap(
                                Convert.ToSingle(chara[3]),
                                Convert.ToSingle(chara[4]),
                                Convert.ToInt32(chara[5]),
                                Convert.ToInt32(chara[6]),
                                Convert.ToInt32(chara[7]),
                                Convert.ToSingle(chara[8]),
                                chara[9]);
                                // meshes[i].GetComponent<MeshFilter>().mesh = mesh;
                                meshes[i].GetComponent<MeshCollider>().sharedMesh = mesh;
                            }
                            else if (chara[2] == "plannarmeshrenderer")
                            {
                                Mesh mesh = GeoTools.MeshFromPoints(
                                Convert.ToInt32(chara[3]),
                                Convert.ToInt32(chara[4]),
                                Convert.ToSingle(chara[5]),
                                Convert.ToSingle(chara[6]));
                                meshes[i].GetComponent<MeshFilter>().mesh = mesh;
                                //  meshes[i].GetComponent<MeshCollider>().sharedMesh = mesh;
                            }
                            else if (chara[2] == "heightmapmeshrenderer")
                            {
                                Mesh mesh = GeoTools.LoadHeightMap(
                                Convert.ToSingle(chara[3]),
                                Convert.ToSingle(chara[4]),
                                Convert.ToInt32(chara[5]),
                                Convert.ToInt32(chara[6]),
                                Convert.ToInt32(chara[7]),
                                Convert.ToSingle(chara[8]),
                                chara[9]);
                                meshes[i].GetComponent<MeshFilter>().mesh = mesh;
                                //meshes[i].GetComponent<MeshCollider>().sharedMesh = mesh;
                            }

                            else if (chara[2] == "color")
                            {
                                meshes[i].GetComponent<MeshRenderer>().material.color = new Color(
                                Convert.ToSingle(chara[3]),
                                Convert.ToSingle(chara[4]),
                                Convert.ToSingle(chara[5]),
                                Convert.ToSingle(chara[6]));
                            }
                            else if (chara[2] == "shadow")
                            {
                                if (Convert.ToInt32(chara[3]) == 0)
                                { meshes[i].GetComponent<MeshRenderer>().receiveShadows = false; }
                                else
                                { meshes[i].GetComponent<MeshRenderer>().receiveShadows = true; }
                            }
                            else if (chara[2] == "texture")
                            {
                                meshes[i].GetComponent<MeshRenderer>().material.mainTexture = GeoTools.LoadTexture(chara[3]);
                                //  Debug.Log(meshes[i].GetComponent<MeshRenderer>().material.GetFloat("_Glossiness"));
                                meshes[i].GetComponent<MeshRenderer>().material.SetFloat("_Glossiness", 0);
                            }
                            else if (chara[2] == "stexture")
                            {
                                meshes[i].GetComponent<MeshRenderer>().sharedMaterial.mainTexture = GeoTools.LoadTexture(chara[3]);
                            }
                            else if (chara[2] == "materialcopy")
                            {
                                try
                                {
                                    meshes[i].GetComponent<MeshRenderer>().material = new Material(GameObject.Find(chara[3]).GetComponent<Renderer>().material);
                                    Debug.Log(meshes[i].GetComponent<MeshRenderer>().material.shader.name);
                                }
                                catch (Exception ex)
                                {
                                    Debug.Log("MaterialCopy Failed");
                                    Debug.Log(ex.ToString());
                                }
                            }
                            else if (chara[2] == "shader")
                            {
                                meshes[i].GetComponent<MeshRenderer>().material.shader = Shader.Find(chara[3]);
                            }
                            else if (chara[2] == "settexture")
                            {
                                meshes[i].GetComponent<MeshRenderer>().material.SetTexture(chara[3], GeoTools.LoadTexture(chara[4]));
                            }
                            else if (chara[2] == "setcolor")
                            {
                                meshes[i].GetComponent<MeshRenderer>().material.SetColor(chara[3], new Color(
                                Convert.ToSingle(chara[4]),
                                Convert.ToSingle(chara[5]),
                                Convert.ToSingle(chara[6]),
                                Convert.ToSingle(chara[7])));
                            }
                            else if (chara[2] == "setvector")
                            {
                                meshes[i].GetComponent<MeshRenderer>().material.SetVector(chara[3], new Vector4(
                                Convert.ToSingle(chara[4]),
                                Convert.ToSingle(chara[5]),
                                Convert.ToSingle(chara[6]),
                                Convert.ToSingle(chara[7])));
                            }
                            else if (chara[2] == "setfloat")
                            {
                                meshes[i].GetComponent<MeshRenderer>().material.SetFloat(chara[3], Convert.ToSingle(chara[4]));
                            }
                            else if (chara[2] == "meshcollider")
                            {
                                meshes[i].GetComponent<MeshCollider>().sharedMesh = GeoTools.MeshFromObj(chara[3]);

                            }
                            else if (chara[2] == "wmeshcollider")
                            {
                                meshes[i].GetComponent<MeshCollider>().sharedMesh = GeoTools.WMeshFromObj(chara[3]);

                            }
                            else if (chara[2] == "dynamicFriction")
                            {
                                float t = Convert.ToSingle(chara[3]);
                                if (t < 0) t = 0;
                                if (t > 1) t = 1;
                                meshes[i].GetComponent<MeshCollider>().material.dynamicFriction = t;
                            }
                            else if (chara[2] == "staticFriction")
                            {
                                float t = Convert.ToSingle(chara[3]);
                                if (t < 0) t = 0;
                                if (t > 1) t = 1;
                                meshes[i].GetComponent<MeshCollider>().material.staticFriction = t;
                            }
                            else if (chara[2] == "bounciness")
                            {
                                float t = Convert.ToSingle(chara[3]);
                                if (t < 0) t = 0;
                                if (t > 1) t = 1;
                                meshes[i].GetComponent<MeshCollider>().material.bounciness = t;
                            }
                            else if (chara[2] == "frictionCombine ")
                            {
                                if (chara[3] == "Average" || chara[3] == "average")
                                { meshes[i].GetComponent<MeshCollider>().material.frictionCombine = PhysicMaterialCombine.Average; }
                                else if (chara[3] == "Multiply" || chara[3] == "multiply")
                                { meshes[i].GetComponent<MeshCollider>().material.frictionCombine = PhysicMaterialCombine.Multiply; }
                                else if (chara[3] == "Minimum" || chara[3] == "minimum")
                                { meshes[i].GetComponent<MeshCollider>().material.frictionCombine = PhysicMaterialCombine.Minimum; }
                                else if (chara[3] == "Maximum" || chara[3] == "maximum")
                                { meshes[i].GetComponent<MeshCollider>().material.frictionCombine = PhysicMaterialCombine.Maximum; }
                            }
                            else if (chara[2] == "bounceCombine  ")
                            {
                                if (chara[3] == "Average" || chara[3] == "average")
                                { meshes[i].GetComponent<MeshCollider>().material.bounceCombine = PhysicMaterialCombine.Average; }
                                else if (chara[3] == "Multiply" || chara[3] == "multiply")
                                { meshes[i].GetComponent<MeshCollider>().material.bounceCombine = PhysicMaterialCombine.Multiply; }
                                else if (chara[3] == "Minimum" || chara[3] == "minimum")
                                { meshes[i].GetComponent<MeshCollider>().material.bounceCombine = PhysicMaterialCombine.Minimum; }
                                else if (chara[3] == "Maximum" || chara[3] == "maximum")
                                { meshes[i].GetComponent<MeshCollider>().material.bounceCombine = PhysicMaterialCombine.Maximum; }
                            }
                            else if (chara[2] == "location")
                            {
                                meshes[i].transform.localPosition = new Vector3(
                                Convert.ToSingle(chara[3]),
                                Convert.ToSingle(chara[4]),
                                Convert.ToSingle(chara[5]));
                                //Debug.Log("meshes" + i.ToString() + ".loaction:" + meshes[i].transform.localPosition.ToString());
                            }
                            else if (chara[2] == "scale")
                            {
                                meshes[i].transform.localScale = new Vector3(
                                Convert.ToSingle(chara[3]),
                                Convert.ToSingle(chara[4]),
                                Convert.ToSingle(chara[5]));
                                //Debug.Log("meshes" + i.ToString() + ".scale:" + meshes[i].transform.localScale.ToString());
                            }
                            else if (chara[2] == "rotation")
                            {
                                meshes[i].transform.rotation = new Quaternion(
                                Convert.ToSingle(chara[3]),
                                Convert.ToSingle(chara[4]),
                                Convert.ToSingle(chara[5]),
                                Convert.ToSingle(chara[6]));
                            }
                            else if (chara[2] == "eulerangles")
                            {
                                meshes[i].transform.Rotate(new Vector3(
                                Convert.ToSingle(chara[3]),
                                Convert.ToSingle(chara[4]),
                                Convert.ToSingle(chara[5])), Space.Self);
                            }
                            else if (chara[2] == "euleranglesworld")
                            {
                                meshes[i].transform.Rotate(new Vector3(
                                Convert.ToSingle(chara[3]),
                                Convert.ToSingle(chara[4]),
                                Convert.ToSingle(chara[5])), Space.World);
                            }
                            else if (chara[2] == "fromtorotation")
                            {
                                meshes[i].transform.rotation = Quaternion.FromToRotation(
                              new Vector3(Convert.ToSingle(chara[3]),
                                Convert.ToSingle(chara[4]),
                                Convert.ToSingle(chara[5])),
                                new Vector3(Convert.ToSingle(chara[6]),
                                Convert.ToSingle(chara[7]),
                                Convert.ToSingle(chara[8]))
                                );
                            }
                        }
                        #endregion
                    }
                }
                srd.Close();
                Debug.Log("ReadMeshObj Completed!");
            }
            catch (Exception ex)
            {
                Debug.Log("ReadMeshObj Failed!");
                Debug.Log(ex.ToString());
                return;
            }
        }
        public void ClearMeshes()
        {
            if (meshes == null) return;
            if (meshes.Length <= 0) return;
            Debug.Log("ClearMeshes");
            for (int i = 0; i < meshes.Length; i++)
            {
                UnityEngine.Object.Destroy(meshes[i]);
            }    
        }
        public void LoadMesh()
        {
            try
            {
                ClearMeshes();
                if (MeshSize > 100) MeshSize = 100;
                if (MeshSize < 0) { MeshSize = 0; return; }
                if (MeshSize > 0)
                {
                    meshes = new GameObject[MeshSize];
                    for (int i = 0; i < meshes.Length; i++)
                    {
                        meshes[i] = GameObject.CreatePrimitive(PrimitiveType.Plane);
                        meshes[i].GetComponent<MeshCollider>().sharedMesh.Clear();
                        meshes[i].GetComponent<MeshFilter>().mesh.Clear();
                        meshes[i].name = "_mesh" + i.ToString();
                    }
                }
            }
            catch (System.Exception ex)
            {
                Debug.Log("LoadMesh Failed!");
                Debug.Log(ex.ToString());
            }
        }

    }
}
