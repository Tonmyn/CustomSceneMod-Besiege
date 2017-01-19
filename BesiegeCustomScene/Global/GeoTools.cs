using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BesiegeCustomScene
{
    public static class GeoTools
    {
        public static string MeshPath = Application.dataPath + "/Mods/BesiegeCustomScene/Mesh/";
        public static string TexturePath = Application.dataPath + "/Mods/BesiegeCustomScene/Texture/";
        public static string ScenePath = Application.dataPath + "/Mods/BesiegeCustomScene/Scene/";
        public static string UIPath = Application.dataPath + "/Mods/BesiegeCustomScene/UI/";
        public static string ShaderPath = Application.dataPath + "/Mods/BesiegeCustomScene/Shader/";
        public static bool StringToKeyCode(string str, out KeyCode output)
        {
            if (str == "F1") { output = KeyCode.F1; return true; }
            else if (str == "F2") { output = KeyCode.F2; return true; }
            else if (str == "F3") { output = KeyCode.F3; return true; }
            else if (str == "F4") { output = KeyCode.F4; return true; }
            else if (str == "F5") { output = KeyCode.F5; return true; }
            else if (str == "F6") { output = KeyCode.F6; return true; }
            else if (str == "F7") { output = KeyCode.F7; return true; }
            else if (str == "F8") { output = KeyCode.F8; return true; }
            else if (str == "F9") { output = KeyCode.F9; return true; }
            else if (str == "F10") { output = KeyCode.F10; return true; }
            else if (str == "F11") { output = KeyCode.F11; return true; }
            else if (str == "F12") { output = KeyCode.F12; return true; }
            else if (str == "Q" || str == "q") { output = KeyCode.Q; return true; }
            else if (str == "W" || str == "w") { output = KeyCode.W; return true; }
            else if (str == "E" || str == "e") { output = KeyCode.E; return true; }
            else if (str == "R" || str == "r") { output = KeyCode.R; return true; }
            else if (str == "T" || str == "t") { output = KeyCode.T; return true; }
            else if (str == "Y" || str == "y") { output = KeyCode.Y; return true; }
            else if (str == "U" || str == "u") { output = KeyCode.U; return true; }
            else if (str == "I" || str == "i") { output = KeyCode.I; return true; }
            else if (str == "O" || str == "o") { output = KeyCode.O; return true; }
            else if (str == "P" || str == "p") { output = KeyCode.P; return true; }
            else if (str == "A" || str == "a") { output = KeyCode.A; return true; }
            else if (str == "S" || str == "s") { output = KeyCode.S; return true; }
            else if (str == "D" || str == "d") { output = KeyCode.D; return true; }
            else if (str == "F" || str == "f") { output = KeyCode.F; return true; }
            else if (str == "G" || str == "g") { output = KeyCode.G; return true; }
            else if (str == "H" || str == "h") { output = KeyCode.H; return true; }
            else if (str == "J" || str == "j") { output = KeyCode.J; return true; }
            else if (str == "K" || str == "k") { output = KeyCode.K; return true; }
            else if (str == "L" || str == "l") { output = KeyCode.L; return true; }
            else if (str == "Z" || str == "z") { output = KeyCode.Z; return true; }
            else if (str == "X" || str == "x") { output = KeyCode.X; return true; }
            else if (str == "C" || str == "c") { output = KeyCode.C; return true; }
            else if (str == "V" || str == "v") { output = KeyCode.V; return true; }
            else if (str == "B" || str == "b") { output = KeyCode.B; return true; }
            else if (str == "N" || str == "n") { output = KeyCode.N; return true; }
            else if (str == "M" || str == "m") { output = KeyCode.M; return true; }
            else { output = KeyCode.Escape; return false; }
        }
        public static Mesh MeshFromPoints(int u, int v, float scaleu, float scalev)
        {
            Mesh mesh = new Mesh();
            List<Vector3> newVertices = new List<Vector3>();
            List<Vector2> newUV = new List<Vector2>();
            List<int> triangleslist = new List<int>();
            for (int i = 0; i < u; i++)
            {
                for (int j = 0; j < v; j++)
                {
                    newVertices.Add(new Vector3(i * scaleu, 0, j * scalev));
                    newUV.Add(new Vector2((float)i / (float)u, (float)j / (float)v));
                    if (i > 0 && j > 0)
                    {
                        triangleslist.Add((j - 1) * u + i - 1);
                        triangleslist.Add((j - 1) * u + i);
                        triangleslist.Add((j) * u + i);
                        triangleslist.Add((j - 1) * u + i - 1);
                        triangleslist.Add((j) * u + i);
                        triangleslist.Add((j) * u + i - 1);
                    }
                }
            }
            mesh.vertices = newVertices.ToArray();
            mesh.uv = newUV.ToArray();
            mesh.triangles = triangleslist.ToArray();
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
            return mesh;
        }
        public static Mesh EMeshFromObj(string Objpath)
        {
            List<Vector3> newVertices = new List<Vector3>();
            List<Vector2> newUV = new List<Vector2>();
            List<Vector2> newUV2 = new List<Vector2>();
            List<int> triangleslist = new List<int>();
            List<Vector3> newNormals = new List<Vector3>();
            Mesh mesh = new Mesh();
            StreamReader srd;
            try
            {
                srd = File.OpenText(Objpath);
            }
            catch
            {
                Debug.Log("File open failed");
                return null;
            }
            try
            {
                while (srd.Peek() != -1)
                {
                    string str = srd.ReadLine();
                    string[] chara = str.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                    if (chara.Length > 2)
                    {
                        if (chara[0] == "v")
                        {
                            Vector3 v1 = new Vector3(
                              Convert.ToSingle(chara[1]),
                              Convert.ToSingle(chara[2]),
                              Convert.ToSingle(chara[3]));
                            newVertices.Add(v1);
                        }
                        else if (chara[0] == "vt")
                        {
                            Vector2 uv1 = new Vector2(
                              Convert.ToSingle(chara[1]),
                              Convert.ToSingle(chara[2]));

                            newUV.Add(uv1);
                            newUV2.Add(uv1);
                        }
                        else if (chara[0] == "vn")
                        {
                            Vector3 v2 = new Vector3(
                              Convert.ToSingle(chara[1]),
                              Convert.ToSingle(chara[2]),
                              Convert.ToSingle(chara[3]));

                            newNormals.Add(v2);
                        }
                        else if (chara[0] == "f")
                        {
                            if (chara.Length == 4)
                            {
                                int a = Convert.ToInt32(chara[1].Split('/')[0]);
                                int b = Convert.ToInt32(chara[2].Split('/')[0]);
                                int c = Convert.ToInt32(chara[3].Split('/')[0]);
                                triangleslist.Add(a - 1);
                                triangleslist.Add(b - 1);
                                triangleslist.Add(c - 1);
                            }
                            if (chara.Length == 5)
                            {
                                int a = Convert.ToInt32(chara[1].Split('/')[0]);
                                int b = Convert.ToInt32(chara[2].Split('/')[0]);
                                int c = Convert.ToInt32(chara[3].Split('/')[0]);
                                int d = Convert.ToInt32(chara[4].Split('/')[0]);
                                triangleslist.Add(a - 1);
                                triangleslist.Add(b - 1);
                                triangleslist.Add(c - 1);
                                triangleslist.Add(a - 1);
                                triangleslist.Add(c - 1);
                                triangleslist.Add(d - 1);
                            }
                        }
                    }
                }
                mesh.vertices = newVertices.ToArray();
                mesh.uv = newUV.ToArray(); mesh.uv2 = newUV.ToArray();
                mesh.triangles = triangleslist.ToArray();
                mesh.normals = newNormals.ToArray();
                Debug.Log("ReadFile " + Objpath + " Completed!" + "Vertices:" + newVertices.Count.ToString());
                srd.Close();
               // mesh.RecalculateBounds();
               // mesh.RecalculateNormals();
               // mesh.Optimize();
            }
            catch (Exception ex)
            {
                Debug.Log("Obj model " + Objpath + " error!");
                Debug.Log("newUV==>" + newUV.Count.ToString());
                Debug.Log("triangleslist==>" + triangleslist.Count.ToString());
                Debug.Log("newNormals==>" + newNormals.Count.ToString());
                Debug.Log(ex.ToString());
            }
            return mesh;
        }
        public static Mesh MeshFromObj(string Objname)
        {
            List<Vector3> Normals = new List<Vector3>();
            List<Vector2> UV = new List<Vector2>();
            List<Vector3> newVertices = new List<Vector3>();
            List<Vector3> Vertices = new List<Vector3>();
            List<Vector2> newUV = new List<Vector2>();
            List<int> triangleslist = new List<int>();
            List<Vector3> newNormals = new List<Vector3>();
            Mesh mesh = new Mesh();
            StreamReader srd;
            try
            {
                srd = File.OpenText(MeshPath + Objname + ".obj");
            }
            catch
            {
                Debug.Log("Open " + Objname + " failed");
                return null;
            }
            try
            {
                while (srd.Peek() != -1)
                {
                    string str = srd.ReadLine();
                    string[] chara = str.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                    if (chara.Length > 2)
                    {
                        if (chara[0] == "v")
                        {
                            Vector3 v1 = new Vector3(
                              Convert.ToSingle(chara[1]),
                              Convert.ToSingle(chara[2]),
                              Convert.ToSingle(chara[3]));
                            Vertices.Add(v1);
                        }
                        else if (chara[0] == "vt")
                        {
                            Vector2 uv1 = new Vector2(
                              Convert.ToSingle(chara[1]),
                              Convert.ToSingle(chara[2]));

                            UV.Add(uv1);
                        }
                        else if (chara[0] == "vn")
                        {
                            Vector3 v2 = new Vector3(
                              Convert.ToSingle(chara[1]),
                              Convert.ToSingle(chara[2]),
                              Convert.ToSingle(chara[3]));

                            Normals.Add(v2);
                        }
                        else if (chara[0] == "f")
                        {
                            if (chara.Length == 4)
                            {
                                int a = Convert.ToInt32(chara[1].Split('/')[0]);
                                int b = Convert.ToInt32(chara[2].Split('/')[0]);
                                int c = Convert.ToInt32(chara[3].Split('/')[0]);
                                triangleslist.Add(newVertices.Count);
                                triangleslist.Add(newVertices.Count + 1);
                                triangleslist.Add(newVertices.Count + 2);
                                newVertices.Add(Vertices[a - 1]);
                                newVertices.Add(Vertices[b - 1]);
                                newVertices.Add(Vertices[c - 1]);
                                newNormals.Add(Normals[Convert.ToInt32(chara[1].Split('/')[2]) - 1]);
                                newNormals.Add(Normals[Convert.ToInt32(chara[2].Split('/')[2]) - 1]);
                                newNormals.Add(Normals[Convert.ToInt32(chara[3].Split('/')[2]) - 1]);
                                newUV.Add(UV[Convert.ToInt32(chara[1].Split('/')[1]) - 1]);
                                newUV.Add(UV[Convert.ToInt32(chara[2].Split('/')[1]) - 1]);
                                newUV.Add(UV[Convert.ToInt32(chara[3].Split('/')[1]) - 1]);
                            }
                            if (chara.Length == 5)
                            {
                                int a = Convert.ToInt32(chara[1].Split('/')[0]);
                                int b = Convert.ToInt32(chara[2].Split('/')[0]);
                                int c = Convert.ToInt32(chara[3].Split('/')[0]);
                                int d = Convert.ToInt32(chara[4].Split('/')[0]);
                                triangleslist.Add(newVertices.Count);
                                triangleslist.Add(newVertices.Count + 1);
                                triangleslist.Add(newVertices.Count + 2);
                                triangleslist.Add(newVertices.Count);
                                triangleslist.Add(newVertices.Count + 2);
                                triangleslist.Add(newVertices.Count + 3);
                                newVertices.Add(Vertices[a - 1]);
                                newVertices.Add(Vertices[b - 1]);
                                newVertices.Add(Vertices[c - 1]);
                                newVertices.Add(Vertices[d - 1]);
                                newNormals.Add(Normals[Convert.ToInt32(chara[1].Split('/')[2]) - 1]);
                                newNormals.Add(Normals[Convert.ToInt32(chara[2].Split('/')[2]) - 1]);
                                newNormals.Add(Normals[Convert.ToInt32(chara[3].Split('/')[2]) - 1]);
                                newNormals.Add(Normals[Convert.ToInt32(chara[4].Split('/')[2]) - 1]);
                                newUV.Add(UV[Convert.ToInt32(chara[1].Split('/')[1]) - 1]);
                                newUV.Add(UV[Convert.ToInt32(chara[2].Split('/')[1]) - 1]);
                                newUV.Add(UV[Convert.ToInt32(chara[3].Split('/')[1]) - 1]);
                                newUV.Add(UV[Convert.ToInt32(chara[4].Split('/')[1]) - 1]);
                            }
                        }
                    }
                }
                mesh.vertices = newVertices.ToArray();
                mesh.uv = newUV.ToArray();
                mesh.triangles = triangleslist.ToArray();
                mesh.normals = newNormals.ToArray();
                Debug.Log("ReadFile " + Objname + " Completed!" + "Vertices:" + newVertices.Count.ToString());
                srd.Close();
                mesh.RecalculateBounds();
                mesh.RecalculateNormals();
                mesh.Optimize();
            }
            catch (Exception ex)
            {
                Debug.Log("Obj model " + Objname + " error!");
                Debug.Log("newUV==>" + newUV.Count.ToString());
                Debug.Log("triangleslist==>" + triangleslist.Count.ToString());
                Debug.Log("newNormals==>" + newNormals.Count.ToString());
                Debug.Log(ex.ToString());
            }
            return mesh;
        }
        public static Mesh WMeshFromObj(string Objname)
        {
            List<Vector3> newVertices = new List<Vector3>();
            List<Vector2> newUV = new List<Vector2>();
            List<int> triangleslist = new List<int>();
            List<Vector3> newNormals = new List<Vector3>();
            Mesh mesh = new Mesh();
            StreamReader srd;
            try
            {
                srd = File.OpenText(MeshPath + Objname + ".obj");
            }
            catch
            {
                Debug.Log("File open failed");
                return null;
            }
            try
            {
                while (srd.Peek() != -1)
                {
                    string str = srd.ReadLine();
                    string[] chara = str.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                    if (chara.Length > 2)
                    {
                        if (chara[0] == "v")
                        {
                            Vector3 v1 = new Vector3(
                              Convert.ToSingle(chara[1]),
                              Convert.ToSingle(chara[2]),
                              Convert.ToSingle(chara[3]));
                            newVertices.Add(v1);
                        }
                        else if (chara[0] == "vt")
                        {
                            Vector2 uv1 = new Vector2(
                              Convert.ToSingle(chara[1]),
                              Convert.ToSingle(chara[2]));

                            newUV.Add(uv1);
                        }
                        else if (chara[0] == "vn")
                        {
                            Vector3 v2 = new Vector3(
                              Convert.ToSingle(chara[1]),
                              Convert.ToSingle(chara[2]),
                              Convert.ToSingle(chara[3]));

                            newNormals.Add(v2);
                        }
                        else if (chara[0] == "f")
                        {
                            if (chara.Length == 4)
                            {
                                int a = Convert.ToInt32(chara[1].Split('/')[0]);
                                int b = Convert.ToInt32(chara[2].Split('/')[0]);
                                int c = Convert.ToInt32(chara[3].Split('/')[0]);
                                triangleslist.Add(a - 1);
                                triangleslist.Add(b - 1);
                                triangleslist.Add(c - 1);
                            }
                            if (chara.Length == 5)
                            {
                                int a = Convert.ToInt32(chara[1].Split('/')[0]);
                                int b = Convert.ToInt32(chara[2].Split('/')[0]);
                                int c = Convert.ToInt32(chara[3].Split('/')[0]);
                                int d = Convert.ToInt32(chara[4].Split('/')[0]);
                                triangleslist.Add(a - 1);
                                triangleslist.Add(b - 1);
                                triangleslist.Add(c - 1);
                                triangleslist.Add(a - 1);
                                triangleslist.Add(c - 1);
                                triangleslist.Add(d - 1);
                            }
                        }
                    }
                }
                mesh.vertices = newVertices.ToArray();
                mesh.uv = newUV.ToArray();
                mesh.triangles = triangleslist.ToArray();
                mesh.normals = newNormals.ToArray();
                Debug.Log("ReadFile " + Objname + " Completed!" + "Vertices:" + newVertices.Count.ToString());
                srd.Close();
                //  mesh.RecalculateBounds();
                //  mesh.RecalculateNormals();
                //  mesh.Optimize();
            }
            catch (Exception ex)
            {
                Debug.Log("Obj model " + Objname + " error!");
                Debug.Log("newUV==>" + newUV.Count.ToString());
                Debug.Log("triangleslist==>" + triangleslist.Count.ToString());
                Debug.Log("newNormals==>" + newNormals.Count.ToString());
                Debug.Log(ex.ToString());
            }
            return mesh;
        }
        public static Texture ELoadTexture(string TexturePath)
        {
            try
            {
                WWW jpg = new WWW("File:///"+TexturePath);
                if (jpg.size > 5)
                {
                    return jpg.texture;
                }
                else
                {
                    Debug.Log("No image in folder or image could not be used!");
                    return GameObject.Find("FloorBig").GetComponent<Renderer>().material.mainTexture;
                }
            }
            catch
            {
                Debug.Log("No image in folder,use white image instead !");
                return GameObject.Find("FloorBig").GetComponent<Renderer>().material.mainTexture;
            }
        }
        public static Texture LoadTexture(string TextureName)
        {
            try
            {
                WWW png = new WWW("File:///" + TexturePath + TextureName + ".png");
                WWW jpg = new WWW("File:///" + TexturePath + TextureName + ".jpg");
                if (png.size > 5)
                {
                    return png.texture;
                }
                else if (jpg.size > 5)
                {
                    return jpg.texture;
                }
                else
                {
                    Debug.Log("No image in folder or image could not be used!");
                    return GameObject.Find("FloorBig").GetComponent<Renderer>().material.mainTexture;
                }
            }
            catch
            {
                Debug.Log("No image in folder,use white image instead !");
                return GameObject.Find("FloorBig").GetComponent<Renderer>().material.mainTexture;
            }
        }
        public static void HideObject(string Objname, ref Vector3 gscale)
        {
            try
            {
                if (GameObject.Find(Objname).transform.localScale != Vector3.zero)
                {
                    gscale = GameObject.Find("FloorGrid").transform.localScale;
                    GameObject.Find(Objname).transform.localScale = new Vector3(0, 0, 0);
                }
            }
            catch (Exception ex)
            {
                Debug.Log(ex.ToString());
            }

        }
        public static void UnhideObject(string Objname, Vector3 gscale)
        {
            try
            {
                GameObject.Find(Objname).transform.localScale = gscale;
            }
            catch (Exception ex)
            {
                Debug.Log(ex.ToString());
            }

        }
        public static Mesh LoadHeightMap(float uscale, float vscale, int u, int v, int heightscale, float texturescale, string HeightMap)
        {
            if (uscale < 1) uscale = 1;
            if (vscale < 1) vscale = 1;
            if (uscale > 100) uscale = 100;
            if (vscale > 100) vscale = 100;
            if (heightscale < 10) heightscale = 10;
            if (heightscale > 100) heightscale = 100;
            if (u < 4) u = 4;
            if (v < 4) v = 4;
            if (u > 2048) u = 2048;
            if (v > 2048) v = 2048;
            if (texturescale > 2048) texturescale = 2048;
            if (texturescale < 0.01f) texturescale = 0.01f;
            Mesh mesh = new Mesh();
            try
            {
                Texture2D te2 = (Texture2D)LoadTexture(HeightMap);
                if (te2.width < u || te2.height < v)
                {
                    Debug.Log("ResetBigFloor Failed ! Need a larger Height map！");
                    u = te2.width; v = te2.height;
                }
                List<Vector3> newVertices = new List<Vector3>();
                List<Vector2> newUV = new List<Vector2>();
                List<int> triangleslist = new List<int>();
                for (int j = 0; j < v; j++)
                {
                    for (int i = 0; i < u; i++)
                    {
                        newVertices.Add(new Vector3(i * uscale, te2.GetPixel(i, j).grayscale * heightscale, j * vscale));
                        newUV.Add(new Vector2((float)i / (float)u * texturescale, (float)j / (float)v * texturescale));
                        if (i > 0 && j > 0)
                        {
                            triangleslist.Add((j - 1) * u + i - 1);
                            triangleslist.Add((j) * u + i);
                            triangleslist.Add((j - 1) * u + i);
                            triangleslist.Add((j - 1) * u + i - 1);
                            triangleslist.Add((j) * u + i - 1);
                            triangleslist.Add((j) * u + i);
                        }
                    }
                }
                mesh.vertices = newVertices.ToArray();
                mesh.uv = newUV.ToArray();
                mesh.triangles = triangleslist.ToArray();
                mesh.RecalculateBounds();
                mesh.RecalculateNormals();
                mesh.Optimize();
                return mesh;
            }
            catch (System.Exception ex)
            {
                Debug.Log("ResetBigFloor Failed ! Please Open dev to search your FloorBig！");
                Debug.Log(ex.ToString());
                return null;
            }
        }
        public static void PrintAssets()
        {
            try
            {
                WWW iteratorVariable0 = new WWW("file:///" + GeoTools.ShaderPath + "Water.unity3d");
                AssetBundle iteratorVariable1 = iteratorVariable0.assetBundle;
                string[] names = iteratorVariable1.GetAllAssetNames();
                for (int i = 0; i < names.Length; i++)
                {
                    Debug.Log(names[i]);
                }
                iteratorVariable1.Unload(true);
            }
            catch (System.Exception ex)
            {
                Debug.Log(ex.ToString());
            }
        }
        public static void ResetWaterMaterial(ref Material mat)
        {
            try
            {
                // mat.SetTexture("_BumpMap", LoadTexture("SimpleFoam"));
                // mat.SetTexture("_ShoreTex", LoadTexture("SmallWaves"));
                // mat.SetTextureScale("_BumpMap", new Vector2(300, 300));
                // mat.SetTextureScale("_ShoreTex", new Vector2(300, 300));
                mat.SetFloat("_Shininess", 200);
                mat.SetFloat("_FresnelScale", 0.75f);
                mat.SetColor("_SpecularColor", Color.black);
                mat.SetVector("_DistortParams", new Vector4(0.144f, 1.1428571f, 1.853064f, -0.5314285f));
                mat.SetVector("_InvFadeParemeter", new Vector4(0.2189655f, 0.1594483f, 0.04310345f, 0f));
                mat.SetVector("_AnimationTiling", new Vector4(2.2f, 2.2f, -1.1f, -1.1f));
                mat.SetVector("_AnimationDirection", new Vector4(1.0f, 1.0f, 1.0f, 1.0f));
                mat.SetVector("_BumpTiling", new Vector4(0.12f, 0.07f, 0.08f, 0.06f));
                mat.SetVector("_BumpDirection", new Vector4(1.0f, 1.0f, -1.0f, 1.0f));
                mat.SetVector("_Foam", new Vector4(0.4266667f, 0.4866667f, 0f, 0.0f));
                mat.SetFloat("_GerstnerIntensity", 2);
                mat.SetVector("_GAmplitude", new Vector4(0.14f, 0.76f, 0.175f, 0.225f));
                mat.SetVector("_GFrequency", new Vector4(0.15f, 0.138f, 0.159f, 0.6f));
                mat.SetVector("_GSteepness", new Vector4(9f, 9f, 9f, 9f));
                mat.SetVector("_GSpeed", new Vector4(-0.13f, 0.12f, 0.11f, 0.3f));
                mat.SetVector("_GDirectionAB", new Vector4(0.4691436f, 0.3540513f, -0.2f, 0.1f));
                mat.SetVector("_GDirectionCD", new Vector4(0.7033888f, -0.6799999f, 0.7175735f, -0.2f));
            }
            catch (System.Exception ex)
            {
                Debug.Log(ex.ToString());
            }
        }
        public static List<Mesh> MeshFromLargeObj(string Objname, int FaceCount)
        {/////f必须在最后 只支持犀牛导出obj
            List<Mesh> meshes = new List<Mesh>();
            List<Vector3> Normals = new List<Vector3>();
            List<Vector2> UV = new List<Vector2>();
            List<Vector3> Vertices = new List<Vector3>();
            List<Vector3> newNormals = new List<Vector3>();
            List<Vector2> newUV = new List<Vector2>();
            List<Vector3> newVertices = new List<Vector3>();
            List<int> triangleslist = new List<int>();

            StreamReader srd;
            try
            {
                srd = File.OpenText(MeshPath + Objname + ".obj");
            }
            catch
            {
                Debug.Log("Open " + Objname + " failed");
                return null;
            }
            try
            {
                int index = 0;
                Mesh mesh = new Mesh();

                while (srd.Peek() != -1)
                {
                    string str = srd.ReadLine();
                    string[] chara = str.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                    if (chara.Length > 2)
                    {
                        if (chara[0] == "v")
                        {
                            Vector3 v1 = new Vector3(
                              Convert.ToSingle(chara[1]),
                              Convert.ToSingle(chara[2]),
                              Convert.ToSingle(chara[3]));
                            Vertices.Add(v1);
                        }
                        else if (chara[0] == "vt")
                        {
                            Vector2 uv1 = new Vector2(
                              Convert.ToSingle(chara[1]),
                              Convert.ToSingle(chara[2]));

                            UV.Add(uv1);
                        }
                        else if (chara[0] == "vn")
                        {
                            Vector3 v2 = new Vector3(
                              Convert.ToSingle(chara[1]),
                              Convert.ToSingle(chara[2]),
                              Convert.ToSingle(chara[3]));

                            Normals.Add(v2);
                        }
                        else if (chara[0] == "f")
                        {
                            if (index == 0)
                            {
                                mesh = new Mesh();
                                triangleslist.Clear();
                                newUV.Clear();
                                newNormals.Clear();
                                newVertices.Clear();
                            }
                            if (chara.Length == 4)
                            {
                                int a = Convert.ToInt32(chara[1].Split('/')[0]);
                                int b = Convert.ToInt32(chara[2].Split('/')[0]);
                                int c = Convert.ToInt32(chara[3].Split('/')[0]);
                                triangleslist.Add(newVertices.Count);
                                triangleslist.Add(newVertices.Count + 1);
                                triangleslist.Add(newVertices.Count + 2);
                                newVertices.Add(Vertices[a - 1]);
                                newVertices.Add(Vertices[b - 1]);
                                newVertices.Add(Vertices[c - 1]);
                                newNormals.Add(Normals[Convert.ToInt32(chara[1].Split('/')[2]) - 1]);
                                newNormals.Add(Normals[Convert.ToInt32(chara[2].Split('/')[2]) - 1]);
                                newNormals.Add(Normals[Convert.ToInt32(chara[3].Split('/')[2]) - 1]);
                                newUV.Add(UV[Convert.ToInt32(chara[1].Split('/')[1]) - 1]);
                                newUV.Add(UV[Convert.ToInt32(chara[2].Split('/')[1]) - 1]);
                                newUV.Add(UV[Convert.ToInt32(chara[3].Split('/')[1]) - 1]);
                            }
                            if (chara.Length == 5)
                            {
                                int a = Convert.ToInt32(chara[1].Split('/')[0]);
                                int b = Convert.ToInt32(chara[2].Split('/')[0]);
                                int c = Convert.ToInt32(chara[3].Split('/')[0]);
                                int d = Convert.ToInt32(chara[4].Split('/')[0]);
                                triangleslist.Add(newVertices.Count);
                                triangleslist.Add(newVertices.Count + 1);
                                triangleslist.Add(newVertices.Count + 2);
                                triangleslist.Add(newVertices.Count);
                                triangleslist.Add(newVertices.Count + 2);
                                triangleslist.Add(newVertices.Count + 3);
                                newVertices.Add(Vertices[a - 1]);
                                newVertices.Add(Vertices[b - 1]);
                                newVertices.Add(Vertices[c - 1]);
                                newVertices.Add(Vertices[d - 1]);
                                newNormals.Add(Normals[Convert.ToInt32(chara[1].Split('/')[2]) - 1]);
                                newNormals.Add(Normals[Convert.ToInt32(chara[2].Split('/')[2]) - 1]);
                                newNormals.Add(Normals[Convert.ToInt32(chara[3].Split('/')[2]) - 1]);
                                newNormals.Add(Normals[Convert.ToInt32(chara[4].Split('/')[2]) - 1]);
                                newUV.Add(UV[Convert.ToInt32(chara[1].Split('/')[1]) - 1]);
                                newUV.Add(UV[Convert.ToInt32(chara[2].Split('/')[1]) - 1]);
                                newUV.Add(UV[Convert.ToInt32(chara[3].Split('/')[1]) - 1]);
                                newUV.Add(UV[Convert.ToInt32(chara[4].Split('/')[1]) - 1]);
                            }

                            index++;
                            if (index >= FaceCount)
                            {
                                index = 0;
                                mesh.vertices = newVertices.ToArray();
                                mesh.uv = newUV.ToArray();
                                mesh.triangles = triangleslist.ToArray();
                                mesh.normals = newNormals.ToArray();

                                mesh.RecalculateBounds();
                                mesh.RecalculateNormals();
                                mesh.Optimize();
                                meshes.Add(mesh);
                                Debug.Log("Vertices:" + newVertices.Count.ToString());
                            }

                        }
                    }
                }
                srd.Close();

                index = 0;
                mesh.vertices = newVertices.ToArray();
                mesh.uv = newUV.ToArray();
                mesh.triangles = triangleslist.ToArray();
                mesh.normals = newNormals.ToArray();

                mesh.RecalculateBounds();
                mesh.RecalculateNormals();
                mesh.Optimize();
                meshes.Add(mesh);
                Debug.Log("Vertices:" + newVertices.Count.ToString());

                Debug.Log("ReadFile " + Objname + " Completed!");
            }
            catch (Exception ex)
            {
                Debug.Log("Obj model " + Objname + " error!");
                Debug.Log("newUV==>" + newUV.Count.ToString());
                Debug.Log("triangleslist==>" + triangleslist.Count.ToString());
                Debug.Log("newNormals==>" + newNormals.Count.ToString());
                Debug.Log(ex.ToString());
            }

            return meshes;
        }
        public static void PrintMaterial(ref Material mat)
        {
            Debug.Log(mat.shaderKeywords.Length);
            for (int i = 0; i < mat.shaderKeywords.Length; i++)
            {
                Debug.Log(mat.shaderKeywords[i]);
            }

        }
        public static void PrintShader()
        {
            Material[] shaders = GameObject.FindObjectsOfType<Material>();
            Debug.Log(shaders.Length);
            for (int i = 0; i < shaders.Length; i++)
            {
                Debug.Log(shaders[i].shader.name);
            }

        }
        public static void PrintAssetBundle(string name)
        {
            try
            {
                WWW iteratorVariable0 = new WWW("file:///" + GeoTools.ShaderPath + name);
                AssetBundle iteratorVariable1 = iteratorVariable0.assetBundle;
                string[] names = iteratorVariable1.GetAllAssetNames();
                for (int i = 0; i < names.Length; i++) { Debug.Log(names[i]); }
            }
            catch (Exception ex)
            {
                Debug.Log("Error! assetBundle failed");
                Debug.Log(ex.ToString());
            }
        }
        public static void GetCenter()
        {
            string stroutput = "";
            try
            {
                MyBlockInfo[] infoArray = UnityEngine.Object.FindObjectsOfType<MyBlockInfo>();
                Vector3 center = new Vector3(0, 0, 0);
                float _weight = 0;
                //List<Transform> list = new List<Transform>();
                foreach (MyBlockInfo info in infoArray)
                {
                    Vector3 v = info.gameObject.GetComponent<Rigidbody>().worldCenterOfMass;
                    float t = info.gameObject.GetComponent<Rigidbody>().mass;
                    center.x = v.x;// * t;
                    center.y = v.y;// * t;
                    center.z = v.z; // * t;
                    _weight += t;
                }
                center.x /= infoArray.Length;
                center.y /= infoArray.Length;
                center.z /= infoArray.Length;
                stroutput= "Center:" + center.x.ToString() + "/" + center.y.ToString() + "/" + center.z.ToString() + " Weight:" + _weight.ToString();
            }
            catch(Exception ex)
            {
                Debug.Log(ex.ToString());
                stroutput= "Could not get Center";
            }
            Debug.Log(stroutput);
        }
        public static void GetLevelInfo()
        {
            Scene scene1 = SceneManager.GetActiveScene();
            Debug.Log("ActiveScene : " + scene1.rootCount.ToString() + "=>" + scene1.name);
            Debug.Log("SceneCount : " + SceneManager.sceneCountInBuildSettings.ToString());
        }
        public static void OpenScene(string Scene)
        {
            if (SceneManager.GetActiveScene().name != Scene)
            {
                SceneManager.LoadScene(Scene, LoadSceneMode.Single);//打开level  
            }
        }
    }
}
