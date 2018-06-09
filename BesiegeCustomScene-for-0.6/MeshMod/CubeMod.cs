using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace BesiegeCustomScene
{

    class CubeMod : MonoBehaviour
    {
        void Start()
        {

        }
        void OnDisable()
        {
            ClearCube();
        }
        void OnDestroy()
        {
            ClearCube();
        }
        ////////////////////////////
        private GameObject[] meshCubes;

        private int CubeAmount;
        [Obsolete]
        string ScenePath = GeoTools.ScenePath;
        [Obsolete]
        public void ReadScene(string SceneName)
        {
            try
            {

                if (!File.Exists(ScenePath + SceneName + ".txt"))
                {
                    GeoTools.Log("Error! Scene File not exists!");
                    return;
                }

                StreamReader srd = File.OpenText(ScenePath + SceneName + ".txt");

                CubeAmount = 0;

                while (srd.Peek() != -1)
                {
                    string str = srd.ReadLine();
                    string[] chara = str.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    if (chara.Length > 2)
                    {
                        #region Cube
                        if (chara[0].ToLower() == "cubes")
                        {
                            if (chara[1].ToLower() == "amount")
                            {
                                CubeAmount = Convert.ToInt32(chara[2]);
                                LoadCube();
                            }
                        }
                        else if (chara[0].ToLower() == "cube")
                        {
                            int i = Convert.ToInt32(chara[1]);
                            if (chara[2] == "mesh")
                            {
                                meshCubes[i].GetComponent<MeshFilter>().mesh = GeoTools.MeshFromObj(chara[3]);
                            }
                            else if (chara[2] == "wmesh")
                            {
                                meshCubes[i].GetComponent<MeshFilter>().mesh = GeoTools.WMeshFromObj(chara[3]);
                            }
                            else if (chara[2] == "scale")
                            {
                                meshCubes[i].transform.localScale = new Vector3(
                               Convert.ToSingle(chara[3]),
                               Convert.ToSingle(chara[4]),
                               Convert.ToSingle(chara[5]));
                            }
                            else if (chara[2] == "rotation")
                            {
                                meshCubes[i].transform.rotation = new Quaternion(
                                Convert.ToSingle(chara[3]),
                                Convert.ToSingle(chara[4]),
                                Convert.ToSingle(chara[5]),
                                Convert.ToSingle(chara[6]));
                            }
                            else if (chara[2] == "eulerangles")
                            {
                                meshCubes[i].transform.Rotate(new Vector3(
                                Convert.ToSingle(chara[3]),
                                Convert.ToSingle(chara[4]),
                                Convert.ToSingle(chara[5])), Space.Self);
                            }
                            else if (chara[2] == "euleranglesworld")
                            {
                                meshCubes[i].transform.Rotate(new Vector3(
                                Convert.ToSingle(chara[3]),
                                Convert.ToSingle(chara[4]),
                                Convert.ToSingle(chara[5])), Space.World);
                            }
                            else if (chara[2] == "fromtorotation")
                            {
                                meshCubes[i].transform.rotation = Quaternion.FromToRotation(
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
                                meshCubes[i].GetComponent<MeshRenderer>().material.shader = Shader.Find("chara[3]");
                            }
                            else if (chara[2] == "texture")
                            {
                                meshCubes[i].GetComponent<MeshRenderer>().material.mainTexture = GeoTools.LoadTexture(chara[3]);
                            }
                            else if (chara[2] == "stexture")
                            {
                                meshCubes[i].GetComponent<MeshRenderer>().sharedMaterial.mainTexture = GeoTools.LoadTexture(chara[3]);
                            }
                            else if (chara[2] == "color")
                            {
                                meshCubes[i].GetComponent<MeshRenderer>().material.color = new Color(
                                Convert.ToSingle(chara[3]),
                                Convert.ToSingle(chara[4]),
                                Convert.ToSingle(chara[5]),
                                Convert.ToSingle(chara[6]));
                            }
                            else if (chara[2] == "meshcollider")
                            {
                                meshCubes[i].GetComponent<MeshCollider>().sharedMesh = GeoTools.MeshFromObj(chara[3]);
                                meshCubes[i].GetComponent<MeshCollider>().convex = true;
                                meshCubes[i].GetComponent<MeshCollider>().isTrigger = true;
                            }
                            else if (chara[2] == "wmeshcollider")
                            {
                                meshCubes[i].GetComponent<MeshCollider>().sharedMesh = GeoTools.WMeshFromObj(chara[3]);
                                meshCubes[i].GetComponent<MeshCollider>().convex = true;
                                meshCubes[i].GetComponent<MeshCollider>().isTrigger = true;
                            }
                            else if (chara[2].ToLower() == "startlocation" || chara[2].ToLower() == "location")
                            {
                                meshCubes[i].GetComponent<CubeScript>().StartLocation = meshCubes[i].transform.localPosition = new Vector3(
                                    Convert.ToSingle(chara[3]),
                                    Convert.ToSingle(chara[4]),
                                    Convert.ToSingle(chara[5]));
                            }
                            else if (chara[2].ToLower() == "movevector")
                            {
                                meshCubes[i].GetComponent<CubeScript>().MoveVector = new Vector3(
                                    Convert.ToSingle(chara[3]),
                                    Convert.ToSingle(chara[4]),
                                    Convert.ToSingle(chara[5]));

                            }
                            else if (chara[2] == "velocity")
                            {
                                meshCubes[i].GetComponent<CubeScript>().Velocity = Convert.ToSingle(chara[3]);
                            }
                            else if (chara[2] == "control")
                            {
                                meshCubes[i].GetComponent<CubeScript>().Key = (KeyCode)Enum.Parse(typeof(KeyCode), chara[3]);
                            }
                            //else if (chara[2].ToLower() == "parent")
                            //{
                            //    meshCubes[i].GetComponent<CubeScript>().ParentName = chara[3];
                            //}
                            ///////////////////////////////////////////////
                        }

                        #endregion
                    }
                }

                if (CubeAmount <= 0)
                {
                    ClearCube();
                }

                srd.Close();

                GeoTools.Log("Read Cube Completed! CubeAmount:" + CubeAmount);
            }
            catch (Exception ex)
            {
                GeoTools.Log("Error! Read Cube Failed!");
                GeoTools.Log(ex.ToString());
                return;
            }
        }

        public void ReadScene(ScenePack scenePack)
        {
            try
            {
                ClearCube();

                if (!File.Exists(scenePack.SettingFilePath))
                {
                    GeoTools.Log("Error! Scene File not exists!");
                    return;
                }

                FileStream fs = new FileStream(scenePack.SettingFilePath, FileMode.Open);
                //打开数据文件
                StreamReader srd = new StreamReader(fs, Encoding.Default);
              
                while (srd.Peek() != -1)
                {
                    string str = srd.ReadLine();
                    string[] chara = str.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    if (chara.Length > 2)
                    {
                        #region Cube
                        if (chara[0].ToLower() == "cubes")
                        {
                            if (chara[1].ToLower() == "amount")
                            {                               
                                CubeAmount = Convert.ToInt32(chara[2]);
                                LoadCube();
                            }
                        }
                        else if (chara[0].ToLower() == "cube")
                        {
                            int i = Convert.ToInt32(chara[1]);
                            if (chara[2] == "mesh")
                            {
                                meshCubes[i].GetComponent<MeshFilter>().mesh = GeoTools.MeshFromObj(chara[3],scenePack);
                            }
                            else if (chara[2] == "wmesh")
                            {
                                meshCubes[i].GetComponent<MeshFilter>().mesh = GeoTools.WMeshFromObj(chara[3],scenePack);
                            }
                            else if (chara[2] == "scale")
                            {
                                meshCubes[i].transform.localScale = new Vector3(
                               Convert.ToSingle(chara[3]),
                               Convert.ToSingle(chara[4]),
                               Convert.ToSingle(chara[5]));
                            }
                            else if (chara[2] == "rotation")
                            {
                                meshCubes[i].transform.rotation = new Quaternion(
                                Convert.ToSingle(chara[3]),
                                Convert.ToSingle(chara[4]),
                                Convert.ToSingle(chara[5]),
                                Convert.ToSingle(chara[6]));
                            }
                            else if (chara[2] == "eulerangles")
                            {
                                meshCubes[i].transform.Rotate(new Vector3(
                                Convert.ToSingle(chara[3]),
                                Convert.ToSingle(chara[4]),
                                Convert.ToSingle(chara[5])), Space.Self);
                            }
                            else if (chara[2] == "euleranglesworld")
                            {
                                meshCubes[i].transform.Rotate(new Vector3(
                                Convert.ToSingle(chara[3]),
                                Convert.ToSingle(chara[4]),
                                Convert.ToSingle(chara[5])), Space.World);
                            }
                            else if (chara[2] == "fromtorotation")
                            {
                                meshCubes[i].transform.rotation = Quaternion.FromToRotation(
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
                                meshCubes[i].GetComponent<MeshRenderer>().material.shader = Shader.Find("chara[3]");
                            }
                            else if (chara[2] == "texture")
                            {
                                meshCubes[i].GetComponent<MeshRenderer>().material.mainTexture = GeoTools.LoadTexture(chara[3],scenePack);
                            }
                            else if (chara[2] == "stexture")
                            {
                                meshCubes[i].GetComponent<MeshRenderer>().sharedMaterial.mainTexture = GeoTools.LoadTexture(chara[3],scenePack);
                            }
                            else if (chara[2] == "color")
                            {
                                meshCubes[i].GetComponent<MeshRenderer>().material.color = new Color(
                                Convert.ToSingle(chara[3]),
                                Convert.ToSingle(chara[4]),
                                Convert.ToSingle(chara[5]),
                                Convert.ToSingle(chara[6]));
                            }
                            else if (chara[2] == "meshcollider")
                            {
                                meshCubes[i].GetComponent<MeshCollider>().sharedMesh = GeoTools.MeshFromObj(chara[3],scenePack);
                                meshCubes[i].GetComponent<MeshCollider>().convex = true;
                            }
                            else if (chara[2] == "wmeshcollider")
                            {
                                meshCubes[i].GetComponent<MeshCollider>().sharedMesh = GeoTools.WMeshFromObj(chara[3],scenePack);
                                meshCubes[i].GetComponent<MeshCollider>().convex = true;
                            }
                            else if (chara[2].ToLower() == "startlocation" || chara[2].ToLower() == "location")
                            {
                                meshCubes[i].GetComponent<CubeScript>().StartLocation = meshCubes[i].transform.localPosition = new Vector3(
                                    Convert.ToSingle(chara[3]),
                                    Convert.ToSingle(chara[4]),
                                    Convert.ToSingle(chara[5]));
                            }
                            else if (chara[2].ToLower() == "movevector")
                            {
                                meshCubes[i].GetComponent<CubeScript>().MoveVector = new Vector3(
                                    Convert.ToSingle(chara[3]),
                                    Convert.ToSingle(chara[4]),
                                    Convert.ToSingle(chara[5]));

                            }
                            else if (chara[2] == "velocity")
                            {
                                meshCubes[i].GetComponent<CubeScript>().Velocity = Convert.ToSingle(chara[3]);
                            }
                            else if (chara[2] == "control")
                            {
                                meshCubes[i].GetComponent<CubeScript>().Key = (KeyCode)Enum.Parse(typeof(KeyCode), chara[3]);
                            }
                            //else if (chara[2].ToLower() == "parent")
                            //{
                            //    meshCubes[i].GetComponent<CubeScript>().ParentName = chara[3];
                            //}
                            ///////////////////////////////////////////////
                        }

                        #endregion
                    }
                }

                srd.Close();
#if DEBUG
                GeoTools.Log("Read Cube Completed! CubeAmount:" + CubeAmount);
#endif
            }
            catch (Exception ex)
            {
                GeoTools.Log("Error! Read Cube Failed!");
                GeoTools.Log(ex.ToString());
                return;
            }
        }

        public void LoadCube()
        {
            try
            {

                if (CubeAmount <= 0)
                {
                    return;
                }
                if (CubeAmount > 100) CubeAmount = 100;
                if (CubeAmount > 0)
                {
                    meshCubes = new GameObject[CubeAmount];
                    for (int i = 0; i < CubeAmount; i++)
                    {
                        //meshCubes[i] = GameObject.CreatePrimitive( PrimitiveType.Cube);
                        meshCubes[i] = GameObject.CreatePrimitive(PrimitiveType.Plane);
                        Shader diffuse = Shader.Find("Legacy Shaders/Diffuse");
                        if (diffuse != null) meshCubes[i].GetComponent<Renderer>().material.shader = diffuse;
                        meshCubes[i].GetComponent<MeshCollider>().sharedMesh.Clear();
                        meshCubes[i].GetComponent<MeshFilter>().mesh.Clear();
                        meshCubes[i].AddComponent<CubeScript>();
                        meshCubes[i].transform.localScale = Vector3.one;
                        meshCubes[i].transform.localPosition = Vector3.zero;
                        meshCubes[i].name = "_meshCube" + i.ToString();
                    }
                }
            }
            catch (System.Exception ex)
            {
                GeoTools.Log("Error! Load Cube Failed!");
                GeoTools.Log(ex.ToString());
            }

#if DEBUG
            GeoTools.Log("加载方块");
#endif

        }

        public void ClearCube()
        {

            if (meshCubes == null) return;

            if (meshCubes.Length <= 0) return;
#if DEBUG
            GeoTools.Log("ClearCubes Amount: " + meshCubes.Length);
#endif
            for (int i = 0; i < meshCubes.Length; i++)
            {
                Destroy(meshCubes[i]);
            }

            meshCubes = null;
            CubeAmount = 0;

        }


    }

    class CubeScript : MonoBehaviour
    {
        public Vector3 StartLocation;

        public Vector3 EndLocation;

        public Vector3 MoveVector;

        public KeyCode Key;

        public float Velocity = 1;

        public string ParentName;

        private float CurrentLerp = 0;

        public bool EnableMove = false;

        public Rigidbody myrigibody;

        private bool NeedReset = true;

        private Vector3 OriginLocation;

        private Vector3 CurrentLocation;

        private GameObject Parent;

        private Rigidbody Prigibody;

        private void Start()
        {
            myrigibody = gameObject.AddComponent<Rigidbody>();
            myrigibody.isKinematic = true;


        }

        private void FixedUpdate()
        {

            if (StatMaster.levelSimulating)
            {
                
                NeedReset = true;

                if (CurrentLerp >= 1 || CurrentLerp <= 0)
                {
                    EnableMove = false;
                    if (Input.GetKeyDown(Key))
                    {
                        Velocity = -Velocity;
                        EnableMove = true;
                    }
                }

                //if (Input.GetKey(Key))
                //{
                //    myrigibody.transform.localPosition += new Vector3(0, 5, 0) * Time.deltaTime; 
                //}
                

                if (EnableMove)
                {
                    CurrentLerp = Mathf.Clamp01(Mathf.MoveTowards(CurrentLerp, 1f, -Velocity * Time.deltaTime));
                    //myrigibody.MovePosition(Vector3.Lerp(StartLocation + Parent.transform.localPosition,StartLocation + MoveVector + Parent.transform.localPosition , CurrentLerp));

                    myrigibody.MovePosition(myrigibody.transform.localPosition - MoveVector * Velocity * Time.deltaTime);
                    
                }

                //GameObject.Find("_mesh0").transform.Translate(-Vector3.right*Time.deltaTime);
                //GameObject.Find("_mesh0").transform.eulerAngles += new Vector3(0, 5, 0) * Time.deltaTime;
                //Prigibody.MovePosition(Prigibody.position + Prigibody.transform.right * Time.deltaTime);
                //Prigibody.MoveRotation(Prigibody.rotation * Quaternion.Euler(new Vector3(0, 2, 0) * Time.deltaTime));

                //myrigibody.MovePosition(myrigibody.position + myrigibody.transform.right * Time.deltaTime);
                //myrigibody.MoveRotation(myrigibody.rotation * Quaternion.Euler(new Vector3(0, 2, 0) * Time.deltaTime));

            }
            else
            {



                if (NeedReset)
                {
                    //if (Parent == null)
                    //{
                    //    Parent = GameObject.Find("_mesh0");
                    //    transform.SetParent(Parent.transform);
                    //    Prigibody = Parent.AddComponent<Rigidbody>();
                    //    Prigibody.isKinematic = true;
                    //}

                    NeedReset = false;
                    //transform.SetParent(GameObject.Find(ParentName).transform);
                    //gameObject.transform.position = OriginLocation = StartLocation + transform.parent.position; ;
                    transform.position = OriginLocation = StartLocation;
                    //GameObject.Find("_mesh0").transform.localPosition = GameObject.Find("_mesh0").transform.eulerAngles = Vector3.zero;
                    Velocity = Mathf.Abs(Velocity);
                    CurrentLerp = 0;

                }



            }



        }
    }

}
