using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Rendering;

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

        [Obsolete]
        string ScenePath = GeoTools.ScenePath;
        
        private GameObject[] meshes;
        private GameObject Meshs;
        private GameObject materialTemp;

        private int MeshSize = 0;
        [Obsolete]
        public void ReadScene(string SceneName)
        {
            try
            {
                //GeoTools.Log(Application.dataPath);
                if (!File.Exists(ScenePath + SceneName + ".txt"))
                {
                    GeoTools.Log("Error! Scene File not exists!");
                    return;
                }
                StreamReader srd = File.OpenText(ScenePath + SceneName + ".txt");
                while (srd.Peek() != -1)
                {
                    string str = srd.ReadLine();
                    string[] chara = str.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    if (chara.Length > 2)
                    {
                        #region Mesheses
                        if (chara[0] == "Meshes")
                        {
                            if (chara[1] == "size" || chara[1] == "Size")
                            {
                                this.MeshSize = Convert.ToInt32(chara[2]);
                                LoadMesh();
                            }
                            else if (chara[1] == "noshadow" || chara[1] == "noShadow" || chara[1] == "NoShadow")
                            {
                                this.MeshSize = Convert.ToInt32(chara[2]);
                                LoadMeshWithOutCastShadow();//对于带烘焙贴图的和模型过大的这项必须取消
                            }
                        }
                        #endregion 
                        #region Meshseries
                        else if (chara[0] == "Meshseries")
                        {
                            int start = Convert.ToInt32(chara[1]);
                            int end = Convert.ToInt32(chara[2]);
                            if (chara[3] == "largeobj")
                            {
                                int index = 0;
                                // Meshseries,start,end,largeobjcollider,Name,faceCount
                                List<Mesh> _meshes = GeoTools.MeshFromLargeObj(chara[4], Convert.ToInt32(chara[5]));
                                for (int i = start; i <= end; i++)
                                {
                                    meshes[i].GetComponent<MeshFilter>().mesh = _meshes[index];
                                    index++;
                                }
                            }
                            else if (chara[3] == "largeobjcollider")
                            {
                                int index = 0;
                                List<Mesh> _meshes = GeoTools.MeshFromLargeObj(chara[4], Convert.ToInt32(chara[5]));
                                for (int i = start; i <= end; i++)
                                {
                                    meshes[i].GetComponent<MeshCollider>().sharedMesh = _meshes[index];
                                    index++;
                                }
                            }
                            else if (chara[3] == "heightmapmesh")
                            {
                                int index = 0;
                                int _width = Convert.ToInt32(chara[4]);
                                int _height = Convert.ToInt32(chara[5]);
                                Vector3 scale = new Vector3(
                                    Convert.ToInt32(chara[6]),
                                    Convert.ToInt32(chara[7]),
                                    Convert.ToInt32(chara[8]));
                                Vector2 texturescale = new Vector3(
                                    Convert.ToInt32(chara[9]),
                                    Convert.ToInt32(chara[10]));
                                List<Mesh> _meshes = GeoTools.LoadHeightMap(_width, _height, scale, texturescale, chara[11]);
                                for (int i = start; i <= end; i++)
                                {
                                    meshes[i].GetComponent<MeshFilter>().mesh = _meshes[index];
                                    meshes[i].GetComponent<MeshCollider>().sharedMesh = _meshes[index];
                                    index++;
                                }
                            }
                            else if (chara[3] == "color")
                            {
                                for (int i = start; i <= end; i++)
                                {
                                    meshes[i].GetComponent<MeshRenderer>().material.color = new Color(
                                 Convert.ToSingle(chara[4]),
                                 Convert.ToSingle(chara[5]),
                                 Convert.ToSingle(chara[6]),
                                 Convert.ToSingle(chara[7]));
                                }
                            }
                            else if (chara[3] == "shader")
                            {
                                for (int i = start; i <= end; i++)
                                {
                                    meshes[i].GetComponent<MeshRenderer>().material.shader = Shader.Find(chara[4]);
                                }
                            }
                            else if (chara[3] == "setfloat")
                            {
                                for (int i = start; i <= end; i++)
                                {
                                    meshes[i].GetComponent<MeshRenderer>().material.SetFloat(chara[4], Convert.ToSingle(chara[5]));
                                }
                            }
                            else if (chara[3] == "settexture")
                            {
                                for (int i = start; i <= end; i++)
                                {
                                    meshes[i].GetComponent<MeshRenderer>().material.SetTexture(chara[4], GeoTools.LoadTexture(chara[5]));
                                }
                            }
                            else if (chara[3] == "setcolor")
                            {
                                for (int i = start; i <= end; i++)
                                {
                                    meshes[i].GetComponent<MeshRenderer>().material.SetColor(chara[4], new Color(
                            Convert.ToSingle(chara[5]),
                            Convert.ToSingle(chara[6]),
                            Convert.ToSingle(chara[7]),
                            Convert.ToSingle(chara[8])));
                                }
                            }
                            else if (chara[3] == "setvector")
                            {
                                for (int i = start; i <= end; i++)
                                {
                                    meshes[i].GetComponent<MeshRenderer>().material.SetVector(chara[4], new Vector4(
                        Convert.ToSingle(chara[5]),
                        Convert.ToSingle(chara[6]),
                        Convert.ToSingle(chara[7]),
                        Convert.ToSingle(chara[8])));
                                }
                            }
                            else if (chara[3] == "texture")
                            {
                                for (int i = start; i <= end; i++)
                                {
                                    meshes[i].GetComponent<MeshRenderer>().material.mainTexture = GeoTools.LoadTexture(chara[4]);
                                }
                            }
                            else if (chara[3] == "materialcopy")
                            {
                                try
                                {
                                    for (int i = start; i <= end; i++)
                                    {
                                        meshes[i].GetComponent<MeshRenderer>().material = new Material(GameObject.Find(chara[4]).GetComponent<Renderer>().material);
                                        // GeoTools.Log(meshes[i].GetComponent<MeshRenderer>().material.shader.name);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    GeoTools.Log("Error! MaterialCopy Failed");
                                    GeoTools.Log(ex.ToString());
                                }
                            }
                            else if (chara[3] == "materialPropcopy")
                            {
                                try
                                {
                                    for (int i = start; i <= end; i++)
                                    {
                                        int index = Convert.ToInt32(chara[4]);
                                        meshes[i].GetComponent<MeshRenderer>().material =
                                            gameObject.GetComponent<Prop>().MaterialTemp[index].GetComponent<Renderer>().material;
                                        // GeoTools.Log(meshes[i].GetComponent<MeshRenderer>().material.shader.name);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    GeoTools.Log("Error! MaterialPropCopy Failed");
                                    GeoTools.Log(ex.ToString());
                                }
                            }
                            else if (chara[3] == "castshadow" || chara[2] == "shadow")
                            {
                                for (int i = start; i <= end; i++)
                                {
                                    if (Convert.ToInt32(chara[4]) == 0)
                                    {
                                        meshes[i].GetComponent<Renderer>().shadowCastingMode = ShadowCastingMode.Off;
                                    }
                                    else
                                    {
                                        meshes[i].GetComponent<Renderer>().shadowCastingMode = ShadowCastingMode.On;
                                    }
                                }

                            }
                            else if (chara[3] == "receiveshadow")
                            {
                                for (int i = start; i <= end; i++)
                                {
                                    if (Convert.ToInt32(chara[4]) == 0)
                                    {
                                        meshes[i].GetComponent<Renderer>().receiveShadows = false;
                                    }
                                    else
                                    {
                                        meshes[i].GetComponent<Renderer>().receiveShadows = true;
                                    }
                                }
                            }
                            else if (chara[3] == "location")
                            {
                                for (int i = start; i <= end; i++)
                                {
                                    meshes[i].transform.localPosition = new Vector3(
                                Convert.ToSingle(chara[4]),
                                Convert.ToSingle(chara[5]),
                                Convert.ToSingle(chara[6]));
                                }
                            }
                            else if (chara[3] == "scale")
                            {
                                for (int i = start; i <= end; i++)
                                {
                                    meshes[i].transform.localScale = new Vector3(
                                Convert.ToSingle(chara[4]),
                                Convert.ToSingle(chara[5]),
                                Convert.ToSingle(chara[6]));
                                }
                            }
                        }
                        #endregion
                        #region Mesh
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
                            else if (chara[2] == "receiveshadow")
                            {
                                if (Convert.ToInt32(chara[3]) == 0)
                                {
                                    meshes[i].GetComponent<Renderer>().receiveShadows = false;
                                }
                                else
                                {
                                    meshes[i].GetComponent<Renderer>().receiveShadows = true;
                                }
                            }
                            else if (chara[2] == "castshadow" || chara[2] == "shadow")
                            {
                                if (Convert.ToInt32(chara[3]) == 0)
                                {
                                    meshes[i].GetComponent<Renderer>().shadowCastingMode = ShadowCastingMode.Off;
                                }
                                else
                                {
                                    meshes[i].GetComponent<Renderer>().shadowCastingMode = ShadowCastingMode.On;
                                }
                            }
                            else if (chara[2] == "texture")
                            {
                                meshes[i].GetComponent<MeshRenderer>().material.mainTexture = GeoTools.LoadTexture(chara[3]);
                                //meshes[i].GetComponent<MeshRenderer>().material.shader = Shader.Find("Standard"); 
                                //meshes[i].GetComponent<MeshRenderer>().material.SetFloat("_Glossiness", 1);//1是smoothness最高
                            }
                            else if (chara[2] == "etexture")
                            {
                                meshes[i].GetComponent<MeshRenderer>().material.mainTexture = GeoTools.ELoadTexture(chara[3]);
                                //meshes[i].GetComponent<MeshRenderer>().material.shader = Shader.Find("Standard"); 
                                //meshes[i].GetComponent<MeshRenderer>().material.SetFloat("_Glossiness", 1);//1是smoothness最高
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
                                    //  GeoTools.Log(meshes[i].GetComponent<MeshRenderer>().material.shader.name);
                                }
                                catch (Exception ex)
                                {
                                    GeoTools.Log("Error! MaterialCopy Failed");
                                    GeoTools.Log(ex.ToString());
                                }
                            }
                            else if (chara[2] == "smaterialcopy")
                            {
                                try
                                {
                                    meshes[i].GetComponent<MeshRenderer>().sharedMaterial = new Material(GameObject.Find(chara[3]).GetComponent<Renderer>().sharedMaterial);
                                }
                                catch (Exception ex)
                                {
                                    GeoTools.Log("Error! MaterialCopy Failed");
                                    GeoTools.Log(ex.ToString());
                                }
                            }
                            else if (chara[2] == "materialPropcopy")
                            {
                                try
                                {
                                    int index = Convert.ToInt32(chara[3]);
                                    meshes[i].GetComponent<MeshRenderer>().material =
                                        gameObject.GetComponent<Prop>().MaterialTemp[index].GetComponent<Renderer>().material;
                                    //     GeoTools.Log(meshes[i].GetComponent<MeshRenderer>().material.shader.name);
                                }
                                catch (Exception ex)
                                {
                                    GeoTools.Log("Error! MaterialPropCopy Failed");
                                    GeoTools.Log(ex.ToString());
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
                            else if (chara[2] == "emeshcollider")
                            {
                                meshes[i].GetComponent<MeshCollider>().sharedMesh = GeoTools.EMeshFromObj(chara[3]);
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
                                //GeoTools.Log("meshes" + i.ToString() + ".loaction:" + meshes[i].transform.localPosition.ToString());
                            }
                            else if (chara[2] == "scale")
                            {
                                meshes[i].transform.localScale = new Vector3(
                                Convert.ToSingle(chara[3]),
                                Convert.ToSingle(chara[4]),
                                Convert.ToSingle(chara[5]));
                                //GeoTools.Log("meshes" + i.ToString() + ".scale:" + meshes[i].transform.localScale.ToString());
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
                //   for (int i = 0; i < this.meshes.Length; i++){GeoTools.MeshFilt(ref this.meshes[i]);}
                GeoTools.Log("ReadMeshObj Completed!");
            }
            catch (Exception ex)
            {
                GeoTools.Log("Error! ReadMeshObj Failed!");
                GeoTools.Log(ex.ToString());
                return;
            }
        }

        public void ReadScene(SceneMod.ScenePack sp)
        {
            
            try
            {
                ClearMeshes();

                if (!File.Exists(sp.SettingFilePath))
                {
                    GeoTools.Log("Error! Scene File not exists!");
                    return;
                }             

                FileStream fs = new FileStream(sp.SettingFilePath, FileMode.Open);

                //打开数据文件
                StreamReader srd = new StreamReader(fs, Encoding.Default);

                while (srd.Peek() != -1)
                {

                    string[] chara = srd.ReadLine().Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);


                    if (chara.Length >= 2)
                    {
                        
                        #region Mesheses
                        if (chara[0] == "Meshes")
                        {
                            if (chara[1].ToLower() == "size")
                            {
                                this.MeshSize = Convert.ToInt32(chara[2]);
                                LoadMesh();
                            }
                            else if (chara[1].ToLower() == "noshadow")
                            {
                                this.MeshSize = Convert.ToInt32(chara[2]);
                                LoadMeshWithOutCastShadow();//对于带烘焙贴图的和模型过大的这项必须取消
                            }

                            else if (chara[1].ToLower() == "materialtemp")
                            {

                                try
                                {
                                    materialTemp = GameObject.CreatePrimitive(PrimitiveType.Plane);
                                    materialTemp.GetComponent<Renderer>().material.shader = Shader.Find("Standard");
                                    materialTemp.GetComponent<Renderer>().material.SetFloat("_Glossiness", 1);
                                    materialTemp.GetComponent<Renderer>().material.mainTexture = GeoTools.LoadTexture(chara[2], sp);
                                    //DontDestroyOnLoad(materialTemp);
                                    materialTemp.SetActive(false);
                                    //meshes[i].GetComponent<MeshRenderer>().material = materialTemp.GetComponent<Renderer>().material;
#if DEBUG
                                    GeoTools.Log("Get " + materialTemp.name + " Successfully");
#endif

                                }
                                catch (Exception ex)
                                {
                                    GeoTools.Log("Error! MaterialTemp Failed");
                                    GeoTools.Log(ex.ToString());
                                }

                            }
                        }
                        #endregion
                        #region Meshseries
                        else if (chara[0] == "Meshseries")
                        {
                            int start = Convert.ToInt32(chara[1]);
                            int end = Convert.ToInt32(chara[2]);
                            if (chara[3] == "largeobj")
                            {
                                int index = 0;
                                // Meshseries,start,end,largeobjcollider,Name,faceCount
                                List<Mesh> _meshes = GeoTools.MeshFromLargeObj(chara[4], Convert.ToInt32(chara[5]),sp);
                                for (int i = start; i <= end; i++)
                                {
                                    meshes[i].GetComponent<MeshFilter>().mesh = _meshes[index];
                                    index++;
                                }
                            }
                            else if (chara[3] == "largeobjcollider")
                            {
                                int index = 0;
                                List<Mesh> _meshes = GeoTools.MeshFromLargeObj(chara[4], Convert.ToInt32(chara[5]),sp);
                                for (int i = start; i <= end; i++)
                                {
                                    meshes[i].GetComponent<MeshCollider>().sharedMesh = _meshes[index];
                                    index++;
                                }
                            }
                            else if (chara[3] == "heightmapmesh")
                            {
                                int index = 0;
                                int _width = Convert.ToInt32(chara[4]);
                                int _height = Convert.ToInt32(chara[5]);
                                Vector3 scale = new Vector3(
                                    Convert.ToInt32(chara[6]),
                                    Convert.ToInt32(chara[7]),
                                    Convert.ToInt32(chara[8]));
                                Vector2 texturescale = new Vector3(
                                    Convert.ToInt32(chara[9]),
                                    Convert.ToInt32(chara[10]));
                                List<Mesh> _meshes = GeoTools.LoadHeightMap(_width, _height, scale, texturescale, chara[11],sp);
                                for (int i = start; i <= end; i++)
                                {
                                    meshes[i].GetComponent<MeshFilter>().mesh = _meshes[index];
                                    meshes[i].GetComponent<MeshCollider>().sharedMesh = _meshes[index];
                                    index++;
                                }
                            }
                            else if (chara[3] == "color")
                            {
                                for (int i = start; i <= end; i++)
                                {
                                    meshes[i].GetComponent<MeshRenderer>().material.color = new Color(
                                 Convert.ToSingle(chara[4]),
                                 Convert.ToSingle(chara[5]),
                                 Convert.ToSingle(chara[6]),
                                 Convert.ToSingle(chara[7]));
                                }
                            }
                            else if (chara[3] == "shader")
                            {
                                for (int i = start; i <= end; i++)
                                {
                                    meshes[i].GetComponent<MeshRenderer>().material.shader = Shader.Find(chara[4]);
                                }
                            }
                            else if (chara[3] == "setfloat")
                            {
                                for (int i = start; i <= end; i++)
                                {
                                    meshes[i].GetComponent<MeshRenderer>().material.SetFloat(chara[4], Convert.ToSingle(chara[5]));
                                }
                            }
                            else if (chara[3] == "settexture")
                            {
                                for (int i = start; i <= end; i++)
                                {
                                    meshes[i].GetComponent<MeshRenderer>().material.SetTexture(chara[4], GeoTools.LoadTexture(chara[5]));
                                }
                            }
                            else if (chara[3] == "setcolor")
                            {
                                for (int i = start; i <= end; i++)
                                {
                                    meshes[i].GetComponent<MeshRenderer>().material.SetColor(chara[4], new Color(
                            Convert.ToSingle(chara[5]),
                            Convert.ToSingle(chara[6]),
                            Convert.ToSingle(chara[7]),
                            Convert.ToSingle(chara[8])));
                                }
                            }
                            else if (chara[3] == "setvector")
                            {
                                for (int i = start; i <= end; i++)
                                {
                                    meshes[i].GetComponent<MeshRenderer>().material.SetVector(chara[4], new Vector4(
                        Convert.ToSingle(chara[5]),
                        Convert.ToSingle(chara[6]),
                        Convert.ToSingle(chara[7]),
                        Convert.ToSingle(chara[8])));
                                }
                            }
                            else if (chara[3] == "texture")
                            {
                                for (int i = start; i <= end; i++)
                                {
                                    meshes[i].GetComponent<MeshRenderer>().material.mainTexture = GeoTools.LoadTexture(chara[4],sp);
                                }
                            }
                            else if (chara[3] == "materialcopy")
                            {
                                try
                                {
                                    for (int i = start; i <= end; i++)
                                    {
                                        meshes[i].GetComponent<MeshRenderer>().material = new Material(GameObject.Find(chara[4]).GetComponent<Renderer>().material);
                                        // GeoTools.Log(meshes[i].GetComponent<MeshRenderer>().material.shader.name);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    GeoTools.Log("Error! MaterialCopy Failed");
                                    GeoTools.Log(ex.ToString());
                                }
                            }
                            else if (chara[3] == "materialPropcopy")
                            {
                                try
                                {
                                    for (int i = start; i <= end; i++)
                                    {
                                        int index = Convert.ToInt32(chara[4]);
                                        //meshes[i].GetComponent<MeshRenderer>().material =
                                        //    gameObject.GetComponent<Prop>().MaterialTemp[index].GetComponent<Renderer>().material;
                                        // GeoTools.Log(meshes[i].GetComponent<MeshRenderer>().material.shader.name);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    GeoTools.Log("Error! MaterialPropCopy Failed");
                                    GeoTools.Log(ex.ToString());
                                }

                            }
                            else if (chara[3] == "castshadow" || chara[2] == "shadow")
                            {
                                for (int i = start; i <= end; i++)
                                {
                                    if (Convert.ToInt32(chara[4]) == 0)
                                    {
                                        meshes[i].GetComponent<Renderer>().shadowCastingMode = ShadowCastingMode.Off;
                                    }
                                    else
                                    {
                                        meshes[i].GetComponent<Renderer>().shadowCastingMode = ShadowCastingMode.On;
                                    }
                                }

                            }
                            else if (chara[3] == "receiveshadow")
                            {
                                for (int i = start; i <= end; i++)
                                {
                                    if (Convert.ToInt32(chara[4]) == 0)
                                    {
                                        meshes[i].GetComponent<Renderer>().receiveShadows = false;
                                    }
                                    else
                                    {
                                        meshes[i].GetComponent<Renderer>().receiveShadows = true;
                                    }
                                }
                            }
                            else if (chara[3] == "location")
                            {
                                for (int i = start; i <= end; i++)
                                {
                                    meshes[i].transform.localPosition = new Vector3(
                                Convert.ToSingle(chara[4]),
                                Convert.ToSingle(chara[5]),
                                Convert.ToSingle(chara[6]));
                                }
                            }
                            else if (chara[3] == "scale")
                            {
                                for (int i = start; i <= end; i++)
                                {
                                    meshes[i].transform.localScale = new Vector3(
                                Convert.ToSingle(chara[4]),
                                Convert.ToSingle(chara[5]),
                                Convert.ToSingle(chara[6]));
                                }
                            }
                        }
                        #endregion
                        #region Mesh
                        else if (chara[0] == "Mesh")
                        {
                            int i = Convert.ToInt32(chara[1]);
                            if (chara[2] == "mesh")
                            {
                                meshes[i].GetComponent<MeshFilter>().mesh = GeoTools.MeshFromObj(chara[3], sp);
                            }
                            else if (chara[2] == "wmesh")
                            {
                                meshes[i].GetComponent<MeshFilter>().mesh = GeoTools.WMeshFromObj(chara[3], sp);
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
                                chara[9],
                                sp);
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
                                chara[9],
                                sp);
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
                                chara[9],
                                sp);
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
                            else if (chara[2] == "receiveshadow")
                            {
                                if (Convert.ToInt32(chara[3]) == 0)
                                {
                                    meshes[i].GetComponent<Renderer>().receiveShadows = false;
                                }
                                else
                                {
                                    meshes[i].GetComponent<Renderer>().receiveShadows = true;
                                }
                            }
                            else if (chara[2] == "castshadow" || chara[2] == "shadow")
                            {
                                if (Convert.ToInt32(chara[3]) == 0)
                                {
                                    meshes[i].GetComponent<Renderer>().shadowCastingMode = ShadowCastingMode.Off;
                                }
                                else
                                {
                                    meshes[i].GetComponent<Renderer>().shadowCastingMode = ShadowCastingMode.On;
                                }
                            }
                            else if (chara[2] == "texture")
                            {
                                meshes[i].GetComponent<MeshRenderer>().material.mainTexture = GeoTools.LoadTexture(chara[3], sp);
                                //meshes[i].GetComponent<MeshRenderer>().material.shader = Shader.Find("Standard"); 
                                //meshes[i].GetComponent<MeshRenderer>().material.SetFloat("_Glossiness", 1);//1是smoothness最高
                            }
                            else if (chara[2] == "etexture")
                            {
                                meshes[i].GetComponent<MeshRenderer>().material.mainTexture = GeoTools.ELoadTexture(chara[3]);
                                //meshes[i].GetComponent<MeshRenderer>().material.shader = Shader.Find("Standard"); 
                                //meshes[i].GetComponent<MeshRenderer>().material.SetFloat("_Glossiness", 1);//1是smoothness最高
                            }
                            else if (chara[2] == "stexture")
                            {
                                meshes[i].GetComponent<MeshRenderer>().sharedMaterial.mainTexture = GeoTools.LoadTexture(chara[3], sp);
                            }
                            else if (chara[2] == "materialcopy")
                            {
                                try
                                {
                                    meshes[i].GetComponent<MeshRenderer>().material = new Material(GameObject.Find(chara[3]).GetComponent<Renderer>().material);
                                    //  GeoTools.Log(meshes[i].GetComponent<MeshRenderer>().material.shader.name);
                                }
                                catch (Exception ex)
                                {
                                    GeoTools.Log("Error! MaterialCopy Failed");
                                    GeoTools.Log(ex.ToString());
                                }
                            }
                            else if (chara[2] == "smaterialcopy")
                            {
                                try
                                {
                                    meshes[i].GetComponent<MeshRenderer>().sharedMaterial = new Material(GameObject.Find(chara[3]).GetComponent<Renderer>().sharedMaterial);
                                }
                                catch (Exception ex)
                                {
                                    GeoTools.Log("Error! MaterialCopy Failed");
                                    GeoTools.Log(ex.ToString());
                                }
                            }
                            else if (chara[2] == "materialPropcopy")
                            {
                                try
                                {
                                    //int index = Convert.ToInt32(chara[3]);
                                    //meshes[i].GetComponent<MeshRenderer>().material =
                                    //    gameObject.GetComponent<Prop>().MaterialTemp[index].GetComponent<Renderer>().material;
                                    ////     GeoTools.Log(meshes[i].GetComponent<MeshRenderer>().material.shader.name);
                                    
                                    meshes[i].GetComponent<MeshRenderer>().material = materialTemp.GetComponent<Renderer>().material;
                                }
                                catch (Exception ex)
                                {
                                    GeoTools.Log("Error! MaterialPropCopy Failed");
                                    GeoTools.Log(ex.ToString());
                                }

                            }
                            
                            else if (chara[2] == "shader")
                            {
                                meshes[i].GetComponent<MeshRenderer>().material.shader = Shader.Find(chara[3]);
                            }
                            else if (chara[2] == "settexture")
                            {
                                meshes[i].GetComponent<MeshRenderer>().material.SetTexture(chara[3], GeoTools.LoadTexture(chara[4], sp));
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
                                meshes[i].GetComponent<MeshCollider>().sharedMesh = GeoTools.MeshFromObj(chara[3], sp);
                            }
                            else if (chara[2] == "wmeshcollider")
                            {
                                meshes[i].GetComponent<MeshCollider>().sharedMesh = GeoTools.WMeshFromObj(chara[3], sp);
                            }
                            else if (chara[2] == "emeshcollider")
                            {
                                meshes[i].GetComponent<MeshCollider>().sharedMesh = GeoTools.EMeshFromObj(chara[3]);
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
                                //GeoTools.Log("meshes" + i.ToString() + ".loaction:" + meshes[i].transform.localPosition.ToString());
                            }
                            else if (chara[2] == "scale")
                            {
                                meshes[i].transform.localScale = new Vector3(
                                Convert.ToSingle(chara[3]),
                                Convert.ToSingle(chara[4]),
                                Convert.ToSingle(chara[5]));
                                //GeoTools.Log("meshes" + i.ToString() + ".scale:" + meshes[i].transform.localScale.ToString());
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
                //   for (int i = 0; i < this.meshes.Length; i++){GeoTools.MeshFilt(ref this.meshes[i]);}
                GeoTools.Log("ReadMeshObj Completed!");
            }
            catch (Exception ex)
            {
                GeoTools.Log("Error! ReadMeshObj Failed!");
                GeoTools.Log(ex.ToString());
                return;
            }
        }

        public void ClearMeshes()
        {

            if (meshes == null) return;
            if (meshes.Length <= 0) return;
            if (MeshSize > 0) GeoTools.Log("ClearMeshes");
            for (int i = 0; i < meshes.Length; i++)
            {
                Destroy(meshes[i]);
            }
            if (materialTemp != null)
            {
                Destroy(materialTemp);
            }          
            materialTemp = null;
            meshes = null;          
            MeshSize = 0;      
        }

        public void LoadMesh()
        {
            try
            {
                
                if (MeshSize > 100) MeshSize = 100;
                if (MeshSize < 0) { MeshSize = 0; return; }
                if (MeshSize > 0)
                {
                    meshes = new GameObject[MeshSize];
                    for (int i = 0; i < meshes.Length; i++)
                    {
                        meshes[i] = GameObject.CreatePrimitive(PrimitiveType.Plane);
                        Shader diffuse = Shader.Find("Legacy Shaders/Diffuse");
                        if (diffuse != null) meshes[i].GetComponent<Renderer>().material.shader = diffuse;
                        meshes[i].GetComponent<MeshCollider>().sharedMesh.Clear();
                        meshes[i].GetComponent<MeshFilter>().mesh.Clear();
                        meshes[i].transform.localScale = Vector3.one;
                        meshes[i].transform.localPosition = Vector3.zero;
                        meshes[i].name = "_mesh" + i.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                GeoTools.Log("Error! LoadMesh Failed!");
                GeoTools.Log(ex.ToString());
            }
        }

        public void LoadMeshWithOutCastShadow()
        {
            try
            {

                if (MeshSize > 100) MeshSize = 100;
                if (MeshSize < 0) { MeshSize = 0; return; }
                if (MeshSize > 0)
                {
                    meshes = new GameObject[MeshSize];
                    for (int i = 0; i < meshes.Length; i++)
                    {
                        meshes[i] = GameObject.CreatePrimitive(PrimitiveType.Plane);
                        Shader diffuse = Shader.Find("Legacy Shaders/Diffuse");
                        if (diffuse != null) meshes[i].GetComponent<Renderer>().material.shader = diffuse;
                        meshes[i].GetComponent<Renderer>().shadowCastingMode = ShadowCastingMode.Off;
                        meshes[i].GetComponent<MeshCollider>().sharedMesh.Clear();
                        meshes[i].GetComponent<MeshFilter>().mesh.Clear();
                        meshes[i].name = "_mesh" + i.ToString();
                    }
                }
            }
            catch (System.Exception ex)
            {
                GeoTools.Log("Error! LoadMesh Failed!");
                GeoTools.Log(ex.ToString());
            }
        }

    }
}
