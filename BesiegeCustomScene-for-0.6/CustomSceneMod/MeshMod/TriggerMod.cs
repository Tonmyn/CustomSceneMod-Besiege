using System;
//using System.IO;
using System.Text;
using UnityEngine;

namespace BesiegeCustomScene
{
    //public class TriggerMod : MonoBehaviour
    //{

    //    void OnDisable()
    //    {
    //        ClearTrigger();
    //    }
    //    void OnDestroy()
    //    {
    //        ClearTrigger();
    //    }

    //    public int Size { get { return TriggerSize; } }
    //    public int Index { get { return TriggerIndex; } set { TriggerIndex = value; } }


    //    private GameObject[] meshtriggers;
    //    private int TriggerSize = 0;
    //    internal int TriggerIndex = -1;
    //    [Obsolete]
    //    string ScenePath = GeoTools.ScenePath;
    //    [Obsolete]
    //    public void ReadScene(string SceneName)
    //    {
    //        //try
    //        //{
    //        //    //GeoTools.Log(Application.dataPath);
    //        //    if (!File.Exists(ScenePath + SceneName + ".txt"))
    //        //    {
    //        //        GeoTools.Log("Error! Scene File not exists!");
    //        //        return;
    //        //    }
    //        //    StreamReader srd = File.OpenText(ScenePath + SceneName + ".txt");
    //        //    while (srd.Peek() != -1)
    //        //    {
    //        //        string str = srd.ReadLine();
    //        //        string[] chara = str.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
    //        //        if (chara.Length > 2)
    //        //        {
    //        //            #region Triggers
    //        //            if (chara[0] == "Triggers")
    //        //            {
    //        //                if (chara[1] == "size")
    //        //                {
    //        //                    this.TriggerSize = Convert.ToInt32(chara[2]);
    //        //                    //this.gameObject.GetComponent<TimeUI>().TriggerSize = TriggerSize;
    //        //                    LoadTrigger();
    //        //                }
    //        //            }
    //        //            else if (chara[0] == "Trigger")
    //        //            {
    //        //                int i = Convert.ToInt32(chara[1]);
    //        //                if (chara[2] == "mesh")
    //        //                {
    //        //                    meshtriggers[i].GetComponent<MeshFilter>().mesh = GeoTools.MeshFromObj(chara[3]);
    //        //                }
    //        //                else if (chara[2] == "wmesh")
    //        //                {
    //        //                    meshtriggers[i].GetComponent<MeshFilter>().mesh = GeoTools.WMeshFromObj(chara[3]);
    //        //                }
    //        //                else if (chara[2] == "scale")
    //        //                {
    //        //                    meshtriggers[i].transform.localScale = new Vector3(
    //        //                   Convert.ToSingle(chara[3]),
    //        //                   Convert.ToSingle(chara[4]),
    //        //                   Convert.ToSingle(chara[5]));
    //        //                }
    //        //                else if (chara[2] == "location")
    //        //                {
    //        //                    meshtriggers[i].transform.localPosition = new Vector3(
    //        //                    Convert.ToSingle(chara[3]),
    //        //                    Convert.ToSingle(chara[4]),
    //        //                    Convert.ToSingle(chara[5]));
    //        //                }
    //        //                else if (chara[2] == "rotation")
    //        //                {
    //        //                    meshtriggers[i].transform.rotation = new Quaternion(
    //        //                    Convert.ToSingle(chara[3]),
    //        //                    Convert.ToSingle(chara[4]),
    //        //                    Convert.ToSingle(chara[5]),
    //        //                    Convert.ToSingle(chara[6]));
    //        //                }
    //        //                else if (chara[2] == "eulerangles")
    //        //                {
    //        //                    meshtriggers[i].transform.Rotate(new Vector3(
    //        //                    Convert.ToSingle(chara[3]),
    //        //                    Convert.ToSingle(chara[4]),
    //        //                    Convert.ToSingle(chara[5])), Space.Self);
    //        //                }
    //        //                else if (chara[2] == "euleranglesworld")
    //        //                {
    //        //                    meshtriggers[i].transform.Rotate(new Vector3(
    //        //                    Convert.ToSingle(chara[3]),
    //        //                    Convert.ToSingle(chara[4]),
    //        //                    Convert.ToSingle(chara[5])), Space.World);
    //        //                }
    //        //                else if (chara[2] == "fromtorotation")
    //        //                {
    //        //                    meshtriggers[i].transform.rotation = Quaternion.FromToRotation(
    //        //                  new Vector3(Convert.ToSingle(chara[3]),
    //        //                    Convert.ToSingle(chara[4]),
    //        //                    Convert.ToSingle(chara[5])),
    //        //                    new Vector3(Convert.ToSingle(chara[6]),
    //        //                    Convert.ToSingle(chara[7]),
    //        //                    Convert.ToSingle(chara[8]))
    //        //                    );
    //        //                }
    //        //                else if (chara[2] == "shader")
    //        //                {
    //        //                    meshtriggers[i].GetComponent<MeshRenderer>().material.shader = Shader.Find("chara[3]");
    //        //                }
    //        //                else if (chara[2] == "texture")
    //        //                {
    //        //                    meshtriggers[i].GetComponent<MeshRenderer>().material.mainTexture = GeoTools.LoadTexture(chara[3]);
    //        //                }
    //        //                else if (chara[2] == "stexture")
    //        //                {
    //        //                    meshtriggers[i].GetComponent<MeshRenderer>().sharedMaterial.mainTexture = GeoTools.LoadTexture(chara[3]);
    //        //                }
    //        //                else if (chara[2] == "color")
    //        //                {
    //        //                    meshtriggers[i].GetComponent<MeshRenderer>().material.color = new Color(
    //        //                    Convert.ToSingle(chara[3]),
    //        //                    Convert.ToSingle(chara[4]),
    //        //                    Convert.ToSingle(chara[5]),
    //        //                    Convert.ToSingle(chara[6]));
    //        //                }
    //        //                else if (chara[2] == "meshcollider")
    //        //                {
    //        //                    meshtriggers[i].GetComponent<MeshCollider>().sharedMesh = GeoTools.MeshFromObj(chara[3]);
    //        //                    meshtriggers[i].GetComponent<MeshCollider>().convex = true;
    //        //                    meshtriggers[i].GetComponent<MeshCollider>().isTrigger = true;
    //        //                }
    //        //                else if (chara[2] == "wmeshcollider")
    //        //                {
    //        //                    meshtriggers[i].GetComponent<MeshCollider>().sharedMesh = GeoTools.WMeshFromObj(chara[3]);
    //        //                    meshtriggers[i].GetComponent<MeshCollider>().convex = true;
    //        //                    meshtriggers[i].GetComponent<MeshCollider>().isTrigger = true;
    //        //                }
    //        //                ///////////////////////////////////////////////
    //        //            }
    //        //            #endregion
    //        //        }
    //        //    }
    //        //    srd.Close();
    //        //    GeoTools.Log("ReadMeshTrigger Completed!");
    //        //}
    //        //catch (Exception ex)
    //        //{
    //        //    GeoTools.Log("Error! ReadMeshTrigger Failed!");
    //        //    GeoTools.Log(ex.ToString());
    //        //    return;
    //        //}
    //    }

    //    public void ReadScene(CustomSceneMod.ScenePack scenePack)
    //    {
    //        //try
    //        //{
    //        //    ClearTrigger();

    //        //    if (!File.Exists(scenePack.SettingFilePath))
    //        //    {
    //        //        GeoTools.Log("Error! Scene File not exists!");
    //        //        return;
    //        //    }

    //        //    FileStream fs = new FileStream(scenePack.SettingFilePath, FileMode.Open);
    //        //    //打开数据文件
    //        //    StreamReader srd = new StreamReader(fs, Encoding.Default);

    //        //    while (srd.Peek() != -1)
    //        //    {
    //        //        string str = srd.ReadLine();
    //        //        string[] chara = str.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
    //        //        if (chara.Length > 2)
    //        //        {
    //        //            #region Triggers
    //        //            if (chara[0] == "Triggers")
    //        //            {
    //        //                if (chara[1] == "size")
    //        //                {
    //        //                    this.TriggerSize = Convert.ToInt32(chara[2]);
    //        //                    //this.gameObject.GetComponent<TimeUI>().TriggerSize = TriggerSize;
    //        //                    LoadTrigger();
    //        //                }
    //        //            }
    //        //            else if (chara[0] == "Trigger")
    //        //            {
    //        //                int i = Convert.ToInt32(chara[1]);
    //        //                if (chara[2] == "mesh")
    //        //                {
    //        //                    meshtriggers[i].GetComponent<MeshFilter>().mesh = GeoTools.MeshFromObj(chara[3]);
    //        //                }
    //        //                else if (chara[2] == "wmesh")
    //        //                {
    //        //                    meshtriggers[i].GetComponent<MeshFilter>().mesh = GeoTools.WMeshFromObj(chara[3]);
    //        //                }
    //        //                else if (chara[2] == "scale")
    //        //                {
    //        //                    meshtriggers[i].transform.localScale = new Vector3(
    //        //                   Convert.ToSingle(chara[3]),
    //        //                   Convert.ToSingle(chara[4]),
    //        //                   Convert.ToSingle(chara[5]));
    //        //                }
    //        //                else if (chara[2] == "location")
    //        //                {
    //        //                    meshtriggers[i].transform.localPosition = new Vector3(
    //        //                    Convert.ToSingle(chara[3]),
    //        //                    Convert.ToSingle(chara[4]),
    //        //                    Convert.ToSingle(chara[5]));
    //        //                }
    //        //                else if (chara[2] == "rotation")
    //        //                {
    //        //                    meshtriggers[i].transform.rotation = new Quaternion(
    //        //                    Convert.ToSingle(chara[3]),
    //        //                    Convert.ToSingle(chara[4]),
    //        //                    Convert.ToSingle(chara[5]),
    //        //                    Convert.ToSingle(chara[6]));
    //        //                }
    //        //                else if (chara[2] == "eulerangles")
    //        //                {
    //        //                    meshtriggers[i].transform.Rotate(new Vector3(
    //        //                    Convert.ToSingle(chara[3]),
    //        //                    Convert.ToSingle(chara[4]),
    //        //                    Convert.ToSingle(chara[5])), Space.Self);
    //        //                }
    //        //                else if (chara[2] == "euleranglesworld")
    //        //                {
    //        //                    meshtriggers[i].transform.Rotate(new Vector3(
    //        //                    Convert.ToSingle(chara[3]),
    //        //                    Convert.ToSingle(chara[4]),
    //        //                    Convert.ToSingle(chara[5])), Space.World);
    //        //                }
    //        //                else if (chara[2] == "fromtorotation")
    //        //                {
    //        //                    meshtriggers[i].transform.rotation = Quaternion.FromToRotation(
    //        //                  new Vector3(Convert.ToSingle(chara[3]),
    //        //                    Convert.ToSingle(chara[4]),
    //        //                    Convert.ToSingle(chara[5])),
    //        //                    new Vector3(Convert.ToSingle(chara[6]),
    //        //                    Convert.ToSingle(chara[7]),
    //        //                    Convert.ToSingle(chara[8]))
    //        //                    );
    //        //                }
    //        //                else if (chara[2] == "shader")
    //        //                {
    //        //                    meshtriggers[i].GetComponent<MeshRenderer>().material.shader = Shader.Find("chara[3]");
    //        //                }
    //        //                else if (chara[2] == "texture")
    //        //                {
    //        //                    meshtriggers[i].GetComponent<MeshRenderer>().material.mainTexture = GeoTools.LoadTexture(chara[3]);
    //        //                }
    //        //                else if (chara[2] == "stexture")
    //        //                {
    //        //                    meshtriggers[i].GetComponent<MeshRenderer>().sharedMaterial.mainTexture = GeoTools.LoadTexture(chara[3]);
    //        //                }
    //        //                else if (chara[2] == "color")
    //        //                {
    //        //                    meshtriggers[i].GetComponent<MeshRenderer>().material.color = new Color(
    //        //                    Convert.ToSingle(chara[3]),
    //        //                    Convert.ToSingle(chara[4]),
    //        //                    Convert.ToSingle(chara[5]),
    //        //                    Convert.ToSingle(chara[6]));
    //        //                }
    //        //                else if (chara[2] == "meshcollider")
    //        //                {
    //        //                    meshtriggers[i].GetComponent<MeshCollider>().sharedMesh = GeoTools.MeshFromObj(chara[3]);
    //        //                    meshtriggers[i].GetComponent<MeshCollider>().convex = true;
    //        //                    meshtriggers[i].GetComponent<MeshCollider>().isTrigger = true;
    //        //                }
    //        //                else if (chara[2] == "wmeshcollider")
    //        //                {
    //        //                    meshtriggers[i].GetComponent<MeshCollider>().sharedMesh = GeoTools.WMeshFromObj(chara[3]);
    //        //                    meshtriggers[i].GetComponent<MeshCollider>().convex = true;
    //        //                    meshtriggers[i].GetComponent<MeshCollider>().isTrigger = true;
    //        //                }
    //        //                ///////////////////////////////////////////////
    //        //            }
    //        //            #endregion
    //        //        }
    //        //    }
    //        //    srd.Close();
    //        //    GeoTools.Log("ReadMeshTrigger Completed!");
    //        //}
    //        //catch (Exception ex)
    //        //{
    //        //    GeoTools.Log("Error! ReadMeshTrigger Failed!");
    //        //    GeoTools.Log(ex.ToString());
    //        //    return;
    //        //}
    //    }

    //    public void LoadTrigger()
    //    {
    //        try
    //        {

    //            if (TriggerSize <= 0)
    //            {
    //                return;
    //            }
    //            if (TriggerSize > 100) TriggerSize = 100;
    //            if (TriggerSize > 0)
    //            {
    //                meshtriggers = new GameObject[TriggerSize];
    //                for (int i = 0; i < meshtriggers.Length; i++)
    //                {
    //                    meshtriggers[i] = GameObject.CreatePrimitive(PrimitiveType.Plane);
    //                    meshtriggers[i].GetComponent<MeshCollider>().sharedMesh.Clear();
    //                    meshtriggers[i].GetComponent<MeshFilter>().mesh.Clear();
    //                    meshtriggers[i].AddComponent<TriggerScript>();
    //                    meshtriggers[i].GetComponent<TriggerScript>().Index = i;
    //                    meshtriggers[i].transform.localScale = new Vector3(1, 1, 1);
    //                    meshtriggers[i].transform.localPosition = new Vector3(0, 0, 0);
    //                    meshtriggers[i].name = "_meshtrigger" + i.ToString();
    //                }
    //            }
    //        }
    //        catch (System.Exception ex)
    //        {
    //            GeoTools.Log("Error! LoadTrigger Failed!");
    //            GeoTools.Log(ex.ToString());
    //        }
    //    }

    //    public void ClearTrigger()
    //    {

    //        if (meshtriggers == null) return;
    //        if (meshtriggers.Length <= 0) return;
    //        if (TriggerSize > 0) GeoTools.Log("ClearTriggers");
    //        for (int i = 0; i < meshtriggers.Length; i++)
    //        {
    //            Destroy(meshtriggers[i]);
    //        }
    //        meshtriggers = null;
    //        TriggerSize = 0;
    //    }


       

    //}

    //public class TriggerScript : MonoBehaviour
    //{
    //    public int Index = -1;

    //    TriggerMod triggerMod;

    //    void Awake()
    //    {
    //        triggerMod = GetComponent<TriggerMod>();
    //    }

    //    void OnTriggerEnter(Collider other)
    //    {
    //        if (StatMaster.levelSimulating)
    //        {
    //            if (triggerMod.TriggerIndex == Index - 1)
    //            {
    //                //TimeUI.TriggerIndex++;
    //                triggerMod.Index++;
    //            }
    //        }
    //    }
    //}

}