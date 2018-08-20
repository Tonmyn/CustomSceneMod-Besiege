using System;
using System.Collections.Generic;
//using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Rendering;

namespace BesiegeCustomScene
{
    class MeshMod : EnvironmentMod
    {
  
        private List<GameObject> meshObjects;
        //MeshPropertise[] meshPropertises;
        //private GameObject Meshs;
        private GameObject materialTemp;

        private int MeshSize = 0;

        ShadowCastingMode shadowCastingMode = ShadowCastingMode.On;

        //class MeshPropertise
        //{

        //}

        //class MeshsPropertise
        //{
        //    int Size;
        //    bool NoShadow;
        //    string MaterialTemp;
        //    string Textrue;
        //    Vector3 Position;
        //    Vector3 Scale;

        //}


        public override void ReadEnvironment(SceneFolder scenePack)
        {
            ClearEnvironment();
            try
            {

                foreach (var str in scenePack.SettingFileDatas)
                {

                    string[] chara = str.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                    if (chara.Length >= 2)
                    {

                        #region Mesheses
                        if (chara[0] == "Meshes")
                        {
                            if (chara[1].ToLower() == "size")
                            {
                                this.MeshSize = Convert.ToInt32(chara[2]);
                                LoadEnvironment();
                            }
                            else if (chara[1].ToLower() == "noshadow")
                            {
                                this.MeshSize = Convert.ToInt32(chara[2]);
                                shadowCastingMode = ShadowCastingMode.Off;
                                LoadEnvironment();
                                //LoadMeshWithOutCastShadow();//对于带烘焙贴图的和模型过大的这项必须取消
                            }

                            else if (chara[1].ToLower() == "materialtemp")
                            {

                                try
                                {
                                    materialTemp = GameObject.CreatePrimitive(PrimitiveType.Plane);
                                    materialTemp.GetComponent<Renderer>().material.shader = Shader.Find("Standard");
                                    materialTemp.GetComponent<Renderer>().material.SetFloat("_Glossiness", 1);
                                    materialTemp.GetComponent<Renderer>().material.mainTexture = GeoTools.LoadTexture(chara[2], scenePack, GeoTools.isDataMode);
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
                                List<Mesh> _meshes = GeoTools.MeshFromLargeObj(chara[4], Convert.ToInt32(chara[5]), scenePack, GeoTools.isDataMode);
                                for (int i = start; i <= end; i++)
                                {
                                    meshObjects[i].GetComponent<MeshFilter>().mesh = _meshes[index];
                                    index++;
                                }
                            }
                            else if (chara[3] == "largeobjcollider")
                            {
                                int index = 0;
                                List<Mesh> _meshes = GeoTools.MeshFromLargeObj(chara[4], Convert.ToInt32(chara[5]), scenePack, GeoTools.isDataMode);
                                for (int i = start; i <= end; i++)
                                {
                                    meshObjects[i].GetComponent<MeshCollider>().sharedMesh = _meshes[index];
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
                                List<Mesh> _meshes = GeoTools.LoadHeightMap(_width, _height, scale, texturescale, chara[11], scenePack, GeoTools.isDataMode);
                                for (int i = start; i <= end; i++)
                                {
                                    meshObjects[i].GetComponent<MeshFilter>().mesh = _meshes[index];
                                    meshObjects[i].GetComponent<MeshCollider>().sharedMesh = _meshes[index];
                                    index++;
                                }
                            }
                            else if (chara[3] == "color")
                            {
                                for (int i = start; i <= end; i++)
                                {
                                    meshObjects[i].GetComponent<MeshRenderer>().material.color = new Color(
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
                                    meshObjects[i].GetComponent<MeshRenderer>().material.shader = Shader.Find(chara[4]);
                                }
                            }
                            else if (chara[3] == "setfloat")
                            {
                                for (int i = start; i <= end; i++)
                                {
                                    meshObjects[i].GetComponent<MeshRenderer>().material.SetFloat(chara[4], Convert.ToSingle(chara[5]));
                                }
                            }
                            else if (chara[3] == "settexture")
                            {
                                for (int i = start; i <= end; i++)
                                {
                                    //meshObjects[i].GetComponent<MeshRenderer>().material.SetTexture(chara[4], GeoTools.LoadTexture(chara[5]));
                                }
                            }
                            else if (chara[3] == "setcolor")
                            {
                                for (int i = start; i <= end; i++)
                                {
                                    meshObjects[i].GetComponent<MeshRenderer>().material.SetColor(chara[4], new Color(
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
                                    meshObjects[i].GetComponent<MeshRenderer>().material.SetVector(chara[4], new Vector4(
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
                                    meshObjects[i].GetComponent<MeshRenderer>().material.mainTexture = GeoTools.LoadTexture(chara[4], scenePack, GeoTools.isDataMode);
                                }
                            }
                            else if (chara[3] == "materialcopy")
                            {
                                try
                                {
                                    for (int i = start; i <= end; i++)
                                    {
                                        meshObjects[i].GetComponent<MeshRenderer>().material = new Material(GameObject.Find(chara[4]).GetComponent<Renderer>().material);
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
                                        meshObjects[i].GetComponent<Renderer>().shadowCastingMode = ShadowCastingMode.Off;
                                    }
                                    else
                                    {
                                        meshObjects[i].GetComponent<Renderer>().shadowCastingMode = ShadowCastingMode.On;
                                    }
                                }

                            }
                            else if (chara[3] == "receiveshadow")
                            {
                                for (int i = start; i <= end; i++)
                                {
                                    if (Convert.ToInt32(chara[4]) == 0)
                                    {
                                        meshObjects[i].GetComponent<Renderer>().receiveShadows = false;
                                    }
                                    else
                                    {
                                        meshObjects[i].GetComponent<Renderer>().receiveShadows = true;
                                    }
                                }
                            }
                            else if (chara[3] == "location")
                            {
                                for (int i = start; i <= end; i++)
                                {
                                    meshObjects[i].transform.localPosition = new Vector3(
                                Convert.ToSingle(chara[4]),
                                Convert.ToSingle(chara[5]),
                                Convert.ToSingle(chara[6]));
                                }
                            }
                            else if (chara[3] == "scale")
                            {
                                for (int i = start; i <= end; i++)
                                {
                                    meshObjects[i].transform.localScale = new Vector3(
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
                                meshObjects[i].GetComponent<MeshFilter>().mesh = GeoTools.MeshFromObj(chara[3], scenePack, GeoTools.isDataMode);
                            }
                            else if (chara[2] == "wmesh")
                            {
                                meshObjects[i].GetComponent<MeshFilter>().mesh = GeoTools.WMeshFromObj(chara[3], scenePack, GeoTools.isDataMode);
                            }
                            else if (chara[2] == "emesh")
                            {
                                meshObjects[i].GetComponent<MeshFilter>().mesh = GeoTools.EMeshFromObj(chara[3]);
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
                                scenePack);
                                meshObjects[i].GetComponent<MeshFilter>().mesh = mesh;
                                meshObjects[i].GetComponent<MeshCollider>().sharedMesh = mesh;
                            }
                            else if (chara[2] == "plannarmesh")
                            {
                                Mesh mesh = GeoTools.MeshFromPoints(
                                Convert.ToInt32(chara[3]),
                                Convert.ToInt32(chara[4]),
                                Convert.ToSingle(chara[5]),
                                Convert.ToSingle(chara[6]));
                                meshObjects[i].GetComponent<MeshFilter>().mesh = mesh;
                                meshObjects[i].GetComponent<MeshCollider>().sharedMesh = mesh;
                            }
                            else if (chara[2] == "plannarmeshcollider")
                            {
                                Mesh mesh = GeoTools.MeshFromPoints(
                                Convert.ToInt32(chara[3]),
                                Convert.ToInt32(chara[4]),
                                Convert.ToSingle(chara[5]),
                                Convert.ToSingle(chara[6]));
                                // meshes[i].GetComponent<MeshFilter>().mesh = mesh;
                                meshObjects[i].GetComponent<MeshCollider>().sharedMesh = mesh;
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
                                scenePack);
                                // meshes[i].GetComponent<MeshFilter>().mesh = mesh;
                                meshObjects[i].GetComponent<MeshCollider>().sharedMesh = mesh;
                            }
                            else if (chara[2] == "plannarmeshrenderer")
                            {
                                Mesh mesh = GeoTools.MeshFromPoints(
                                Convert.ToInt32(chara[3]),
                                Convert.ToInt32(chara[4]),
                                Convert.ToSingle(chara[5]),
                                Convert.ToSingle(chara[6]));
                                meshObjects[i].GetComponent<MeshFilter>().mesh = mesh;
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
                                scenePack);
                                meshObjects[i].GetComponent<MeshFilter>().mesh = mesh;
                                //meshes[i].GetComponent<MeshCollider>().sharedMesh = mesh;
                            }

                            else if (chara[2] == "color")
                            {
                                meshObjects[i].GetComponent<MeshRenderer>().material.color = new Color(
                                Convert.ToSingle(chara[3]),
                                Convert.ToSingle(chara[4]),
                                Convert.ToSingle(chara[5]),
                                Convert.ToSingle(chara[6]));
                            }
                            else if (chara[2] == "receiveshadow")
                            {
                                if (Convert.ToInt32(chara[3]) == 0)
                                {
                                    meshObjects[i].GetComponent<Renderer>().receiveShadows = false;
                                }
                                else
                                {
                                    meshObjects[i].GetComponent<Renderer>().receiveShadows = true;
                                }
                            }
                            else if (chara[2] == "castshadow" || chara[2] == "shadow")
                            {
                                if (Convert.ToInt32(chara[3]) == 0)
                                {
                                    meshObjects[i].GetComponent<Renderer>().shadowCastingMode = ShadowCastingMode.Off;
                                }
                                else
                                {
                                    meshObjects[i].GetComponent<Renderer>().shadowCastingMode = ShadowCastingMode.On;
                                }
                            }
                            else if (chara[2] == "texture")
                            {
                                meshObjects[i].GetComponent<MeshRenderer>().material.mainTexture = GeoTools.LoadTexture(chara[3], scenePack, GeoTools.isDataMode);
                                //meshes[i].GetComponent<MeshRenderer>().material.shader = Shader.Find("Standard"); 
                                //meshes[i].GetComponent<MeshRenderer>().material.SetFloat("_Glossiness", 1);//1是smoothness最高
                            }
                            else if (chara[2] == "etexture")
                            {
                                //meshObjects[i].GetComponent<MeshRenderer>().material.mainTexture = GeoTools.ELoadTexture(chara[3]);
                                ////meshes[i].GetComponent<MeshRenderer>().material.shader = Shader.Find("Standard"); 
                                ////meshes[i].GetComponent<MeshRenderer>().material.SetFloat("_Glossiness", 1);//1是smoothness最高
                            }
                            else if (chara[2] == "stexture")
                            {
                                meshObjects[i].GetComponent<MeshRenderer>().sharedMaterial.mainTexture = GeoTools.LoadTexture(chara[3], scenePack, GeoTools.isDataMode);
                            }
                            else if (chara[2] == "materialcopy")
                            {
                                try
                                {
                                    meshObjects[i].GetComponent<MeshRenderer>().material = new Material(GameObject.Find(chara[3]).GetComponent<Renderer>().material);
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
                                    meshObjects[i].GetComponent<MeshRenderer>().sharedMaterial = new Material(GameObject.Find(chara[3]).GetComponent<Renderer>().sharedMaterial);
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

                                    meshObjects[i].GetComponent<MeshRenderer>().material = materialTemp.GetComponent<Renderer>().material;
                                }
                                catch (Exception ex)
                                {
                                    GeoTools.Log("Error! MaterialPropCopy Failed");
                                    GeoTools.Log(ex.ToString());
                                }

                            }

                            else if (chara[2] == "shader")
                            {
                                meshObjects[i].GetComponent<MeshRenderer>().material.shader = Shader.Find(chara[3]);
                            }
                            else if (chara[2] == "settexture")
                            {
                                meshObjects[i].GetComponent<MeshRenderer>().material.SetTexture(chara[3], GeoTools.LoadTexture(chara[4], scenePack, GeoTools.isDataMode));
                            }
                            else if (chara[2] == "setcolor")
                            {
                                meshObjects[i].GetComponent<MeshRenderer>().material.SetColor(chara[3], new Color(
                                Convert.ToSingle(chara[4]),
                                Convert.ToSingle(chara[5]),
                                Convert.ToSingle(chara[6]),
                                Convert.ToSingle(chara[7])));
                            }
                            else if (chara[2] == "setvector")
                            {
                                meshObjects[i].GetComponent<MeshRenderer>().material.SetVector(chara[3], new Vector4(
                                Convert.ToSingle(chara[4]),
                                Convert.ToSingle(chara[5]),
                                Convert.ToSingle(chara[6]),
                                Convert.ToSingle(chara[7])));
                            }
                            else if (chara[2] == "setfloat")
                            {
                                meshObjects[i].GetComponent<MeshRenderer>().material.SetFloat(chara[3], Convert.ToSingle(chara[4]));
                            }
                            else if (chara[2] == "meshcollider")
                            {
                                meshObjects[i].GetComponent<MeshCollider>().sharedMesh = GeoTools.MeshFromObj(chara[3], scenePack, GeoTools.isDataMode);
                            }
                            else if (chara[2] == "wmeshcollider")
                            {
                                meshObjects[i].GetComponent<MeshCollider>().sharedMesh = GeoTools.WMeshFromObj(chara[3], scenePack, GeoTools.isDataMode);
                            }
                            else if (chara[2] == "emeshcollider")
                            {
                                meshObjects[i].GetComponent<MeshCollider>().sharedMesh = GeoTools.EMeshFromObj(chara[3]);
                            }
                            else if (chara[2] == "dynamicFriction")
                            {
                                float t = Convert.ToSingle(chara[3]);
                                if (t < 0) t = 0;
                                if (t > 1) t = 1;
                                meshObjects[i].GetComponent<MeshCollider>().material.dynamicFriction = t;
                            }
                            else if (chara[2] == "staticFriction")
                            {
                                float t = Convert.ToSingle(chara[3]);
                                if (t < 0) t = 0;
                                if (t > 1) t = 1;
                                meshObjects[i].GetComponent<MeshCollider>().material.staticFriction = t;
                            }
                            else if (chara[2] == "bounciness")
                            {
                                float t = Convert.ToSingle(chara[3]);
                                if (t < 0) t = 0;
                                if (t > 1) t = 1;
                                meshObjects[i].GetComponent<MeshCollider>().material.bounciness = t;
                            }
                            else if (chara[2] == "frictionCombine ")
                            {
                                if (chara[3] == "Average" || chara[3] == "average")
                                { meshObjects[i].GetComponent<MeshCollider>().material.frictionCombine = PhysicMaterialCombine.Average; }
                                else if (chara[3] == "Multiply" || chara[3] == "multiply")
                                { meshObjects[i].GetComponent<MeshCollider>().material.frictionCombine = PhysicMaterialCombine.Multiply; }
                                else if (chara[3] == "Minimum" || chara[3] == "minimum")
                                { meshObjects[i].GetComponent<MeshCollider>().material.frictionCombine = PhysicMaterialCombine.Minimum; }
                                else if (chara[3] == "Maximum" || chara[3] == "maximum")
                                { meshObjects[i].GetComponent<MeshCollider>().material.frictionCombine = PhysicMaterialCombine.Maximum; }
                            }
                            else if (chara[2] == "bounceCombine  ")
                            {
                                if (chara[3] == "Average" || chara[3] == "average")
                                { meshObjects[i].GetComponent<MeshCollider>().material.bounceCombine = PhysicMaterialCombine.Average; }
                                else if (chara[3] == "Multiply" || chara[3] == "multiply")
                                { meshObjects[i].GetComponent<MeshCollider>().material.bounceCombine = PhysicMaterialCombine.Multiply; }
                                else if (chara[3] == "Minimum" || chara[3] == "minimum")
                                { meshObjects[i].GetComponent<MeshCollider>().material.bounceCombine = PhysicMaterialCombine.Minimum; }
                                else if (chara[3] == "Maximum" || chara[3] == "maximum")
                                { meshObjects[i].GetComponent<MeshCollider>().material.bounceCombine = PhysicMaterialCombine.Maximum; }
                            }
                            else if (chara[2] == "location")
                            {
                                meshObjects[i].transform.localPosition = new Vector3(
                                Convert.ToSingle(chara[3]),
                                Convert.ToSingle(chara[4]),
                                Convert.ToSingle(chara[5]));
                                //GeoTools.Log("meshes" + i.ToString() + ".loaction:" + meshes[i].transform.localPosition.ToString());
                            }
                            else if (chara[2] == "scale")
                            {
                                meshObjects[i].transform.localScale = new Vector3(
                                Convert.ToSingle(chara[3]),
                                Convert.ToSingle(chara[4]),
                                Convert.ToSingle(chara[5]));
                                //GeoTools.Log("meshes" + i.ToString() + ".scale:" + meshes[i].transform.localScale.ToString());
                            }
                            else if (chara[2] == "rotation")
                            {
                                meshObjects[i].transform.rotation = new Quaternion(
                                Convert.ToSingle(chara[3]),
                                Convert.ToSingle(chara[4]),
                                Convert.ToSingle(chara[5]),
                                Convert.ToSingle(chara[6]));
                            }
                            else if (chara[2] == "eulerangles")
                            {
                                meshObjects[i].transform.Rotate(new Vector3(
                                Convert.ToSingle(chara[3]),
                                Convert.ToSingle(chara[4]),
                                Convert.ToSingle(chara[5])), Space.Self);
                            }
                            else if (chara[2] == "euleranglesworld")
                            {
                                meshObjects[i].transform.Rotate(new Vector3(
                                Convert.ToSingle(chara[3]),
                                Convert.ToSingle(chara[4]),
                                Convert.ToSingle(chara[5])), Space.World);
                            }
                            else if (chara[2] == "fromtorotation")
                            {
                                meshObjects[i].transform.rotation = Quaternion.FromToRotation(
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

                //   for (int i = 0; i < this.meshes.Length; i++){GeoTools.MeshFilt(ref this.meshes[i]);}
                GeoTools.Log("Read MeshObj Completed!");
            }
            catch (Exception ex)
            {
                GeoTools.Log("Error! Read MeshObj Failed!");
                GeoTools.Log(ex.ToString());
                return;
            }
        }
        public override void LoadEnvironment()
        {
            try
            {
                MeshSize = Mathf.Clamp(MeshSize, 0, 100);

                if (MeshSize <= 0)
                {
                    return;
                }
                else
                {
                    if (meshObjects == null)
                    {
                        meshObjects = new List<GameObject>();
                            ;
                        for (int i = 0; i < MeshSize; i++)
                        {
                            meshObjects.Add(CreateMeshObject(shadowCastingMode));
                        }
#if DEBUG
                        GeoTools.Log("Load Mesh Successfully");
#endif
                    }
                }
            }
            catch (Exception ex)
            {
                GeoTools.Log("Error! Load Mesh Failed!");
                GeoTools.Log(ex.Message);
            }
        }
        public override void ClearEnvironment()
        {

            if (meshObjects == null) return;
            if (meshObjects.Count <= 0) return;
#if DEBUG
            GeoTools.Log("Clear Meshes");
#endif
            for (int i = 0; i < meshObjects.Count; i++)
            {
                Destroy(meshObjects[i]);
            }
            if (materialTemp != null)
            {
                Destroy(materialTemp);
            }          
            materialTemp = null;
            meshObjects = null;          
            MeshSize = 0;      
        }

        GameObject CreateMeshObject(ShadowCastingMode shadowCastingMode = ShadowCastingMode.On)
        {
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Plane);
            Shader diffuse = Shader.Find("Legacy Shaders/Diffuse");
            if (diffuse != null) go.GetComponent<Renderer>().material.shader = diffuse;
            if (shadowCastingMode == ShadowCastingMode.Off)
            {
                go.GetComponent<Renderer>().shadowCastingMode = ShadowCastingMode.Off;
            }
            go.GetComponent<MeshCollider>().sharedMesh.Clear();
            go.GetComponent<MeshFilter>().mesh.Clear();
            go.transform.SetParent(transform);
            go.transform.localScale = Vector3.one;
            go.transform.localPosition = Vector3.zero;   
            go.name = "Mesh Object";

            return go;
        }

        //public void LoadMeshWithOutCastShadow()
        //{
        //    try
        //    {

        //        if (MeshSize > 100) MeshSize = 100;
        //        if (MeshSize < 0) { MeshSize = 0; return; }
        //        if (MeshSize > 0)
        //        {
        //            meshObjects = new GameObject[MeshSize];
        //            for (int i = 0; i < meshObjects.Length; i++)
        //            {
        //                meshObjects[i] = GameObject.CreatePrimitive(PrimitiveType.Plane);
        //                Shader diffuse = Shader.Find("Legacy Shaders/Diffuse");
        //                if (diffuse != null) meshObjects[i].GetComponent<Renderer>().material.shader = diffuse;
        //                meshObjects[i].GetComponent<Renderer>().shadowCastingMode = ShadowCastingMode.Off;
        //                meshObjects[i].GetComponent<MeshCollider>().sharedMesh.Clear();
        //                meshObjects[i].GetComponent<MeshFilter>().mesh.Clear();
        //                meshObjects[i].name = "_mesh" + i.ToString();
        //            }
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {
        //        GeoTools.Log("Error! LoadMesh Failed!");
        //        GeoTools.Log(ex.ToString());
        //    }
        //}

    }
}
