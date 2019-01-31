using System;
using System.Collections.Generic;
//using System.IO;
using System.Text;
using UnityEngine;

namespace BesiegeCustomScene
{
    public class WaterMod : EnvironmentMod
    {
 
        private List<GameObject> waterObjects;
        private int WaterSize = 0;

        List<WaterPropertise> waterPropertise;

        class WaterPropertise
        {
            public Mesh mesh = new Mesh();

            public Vector3 position = Vector3.zero;

            public Vector3 scale = Vector3.one;

            public MeshCollider meshCollider = new MeshCollider();
        }

        public override void ReadEnvironment(SceneFolder scenePack)
        {
            ClearEnvironment();

            try
            {

                foreach (var str in scenePack.SettingFileDatas)
                {

                    string[] chara = str.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    if (chara.Length > 2)
                    {
                        #region Water
                        if (chara[0].ToLower() == "mwater")
                        {
                            if (chara[1].ToLower() == "size")
                            {
                                WaterSize = Mathf.Clamp(Convert.ToInt32(chara[2]), 0, 1000);
                                if (WaterSize <= 0)
                                {
                                    break;
                                }
                                waterPropertise = new List<WaterPropertise>();
                                for (int i = 0; i < WaterSize; i++)
                                {
                                    waterPropertise.Add(new WaterPropertise());
                                }
                                
                                //LoadEnvironment();
                            }
                            if (chara[1].ToLower() == "watertemp")
                            {
                                if (Convert.ToInt32(chara[2]) == 0)
                                {
                                    BesiegeCustomSceneMod.Mod.GetComponent<Prop>().WaterTemp.SetActive(false);
                                }
                                else
                                {
                                    BesiegeCustomSceneMod.Mod.GetComponent<Prop>().WaterTemp.SetActive(true);
                                }
                            }
                        }
                        else if (chara[0].ToLower() == "water")
                        {
                            int i = Convert.ToInt32(chara[1]);

                            if (chara[2] == "mesh")
                            {
                                waterPropertise[i].mesh = GeoTools.MeshFromObj(chara[3], GeoTools.isDataMode);
                            }
                            else if (chara[2] == "wmesh")
                            {
                                waterPropertise[i].mesh = GeoTools.WMeshFromObj(chara[3], GeoTools.isDataMode);
                            }
                            else if (chara[2] == "scale")
                            {
                                waterPropertise[i].scale = new Vector3(
                               Convert.ToSingle(chara[3]),
                               Convert.ToSingle(chara[4]),
                               Convert.ToSingle(chara[5]));
                            }
                            else if (chara[2] == "location")
                            {
                                waterPropertise[i].position = new Vector3(
                               Convert.ToSingle(chara[3]),
                               Convert.ToSingle(chara[4]),
                               Convert.ToSingle(chara[5]));
                            }
                            else if (chara[2] == "meshcollider")
                            {
                                waterPropertise[i].meshCollider.sharedMesh = GeoTools.MeshFromObj(chara[3], GeoTools.isDataMode);
                            }
                            else if (chara[2] == "wmeshcollider")
                            {
                                waterPropertise[i].meshCollider.sharedMesh = GeoTools.WMeshFromObj(chara[3], GeoTools.isDataMode);
                            }
                        }
                        #endregion
                    }
                }
#if DEBUG
                GeoTools.Log("Read Water Completed!");
#endif
            }
            catch (Exception ex)
            {
                GeoTools.Log("Error! Read Water Failed!");
                GeoTools.Log(ex.ToString());
                return;
            }
        }
        public override void LoadEnvironment()
        {
            try
            {        
                if (BesiegeCustomSceneMod.Mod.GetComponent<Prop>().TileTemp == null) return;
                
                if (WaterSize <= 0)
                {
                    return;
                }
                else
                {

                    waterObjects = new List<GameObject>();

                    for (int i = 0; i < WaterSize; i++)
                    {
                        waterObjects.Add(CreateWaterObject(waterPropertise[i]));
                    }

#if DEBUG
                    GeoTools.Log("Load Water Successfully");
#endif
                }
            }
            catch (Exception ex)
            {
                GeoTools.Log("Load Water Failed");
                GeoTools.Log(ex.ToString());
                ClearEnvironment();
                return;
            }
        }
        public override void ClearEnvironment()
        {
            ClearFloater();
            if (waterObjects == null) return;
            if (waterObjects.Count <= 0) return;
#if DEBUG
            GeoTools.Log("Clear Water");
#endif
            try
            {
                BesiegeCustomSceneMod.Mod.GetComponent<Prop>().WaterTemp.SetActive(false);
            }
            catch { }
            try
            {
                Destroy(GameObject.Find("WaterTempReflectionMain Camera"));
            }
            catch { }
            for (int i = 0; i < waterObjects.Count; i++)
            {
                Destroy(waterObjects[i]);
            }

            waterObjects = null;
            waterPropertise = null;
            WaterSize = 0;
        }

        GameObject CreateWaterObject(WaterPropertise waterPropertise)
        {
            GameObject go = Instantiate(BesiegeCustomSceneMod.Mod.GetComponent<Prop>().TileTemp);
            go.name = "Water Object";
            go.SetActive(true);
            go.transform.SetParent(transform);
            go.transform.localScale = waterPropertise.scale;
            go.transform.localPosition = waterPropertise.position;

            //Mesh mesh = go.GetComponent<Mesh>();
            //if (waterPropertise.mesh != null)
            //{
            //    mesh = waterPropertise.mesh;
            //}

            //MeshCollider mc = go.GetComponent<MeshCollider>();
            //mc.sharedMesh = mesh;
            //mc.convex = true;
            //mc.isTrigger = true;


            return go;
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
                GeoTools.Log("Error! LoadFloater Failed");
            }
        }
        public void ClearFloater()
        {
            try
            {
                bool sign = false;
                MyBlockInfo[] infoArray = UnityEngine.Object.FindObjectsOfType<MyBlockInfo>();
                foreach (MyBlockInfo info in infoArray)
                {
                    if (info.gameObject.GetComponent<Floater>() != null)
                    {
                        Destroy(info.gameObject.GetComponent<Floater>());
                        sign = true;
                    }
                }
                if (sign) GeoTools.Log("ClearFloater");
            }
            catch
            {
                GeoTools.Log("Error! ClearFloater Failed");
            }
        }
    }

    public class Floater : MonoBehaviour
    {
        // Fields
        private float AngularDrag = 0f;
        private float Drag = 0f;
        private float Force = 0f;
        private float ForceScale = 8f;
        private float minVolue = 0f;
        private float volume = 0f;
        private float WaterHeight = 0f;
        private bool fireTag = false;
        // Methods
        private void FixedUpdate()
        {
            if (base.GetComponent<Rigidbody>() == null)
            {
                UnityEngine.Object.Destroy(this);
            }
            else if (base.transform.position.y < (this.WaterHeight - this.minVolue))
            {
                if (fireTag) base.GetComponent<FireTag>().WaterHit();
                base.GetComponent<Rigidbody>().drag = (this.Drag + 3f) + (this.Force * this.ForceScale);
                base.GetComponent<Rigidbody>().angularDrag = (this.AngularDrag + 3f) + (this.Force * this.ForceScale);
                base.GetComponent<Rigidbody>().AddForce(new Vector3(UnityEngine.Random.Range(-0.01f, 0.01f),
                    this.Force - 0.2f * base.GetComponent<Rigidbody>().mass,
                    UnityEngine.Random.Range(-0.01f, 0.01f)), ForceMode.Impulse);
                base.GetComponent<Rigidbody>().useGravity = false;
            }
            else if (base.transform.position.y > this.WaterHeight)
            {
                base.GetComponent<Rigidbody>().drag = this.Drag;
                base.GetComponent<Rigidbody>().angularDrag = this.AngularDrag;
                base.GetComponent<Rigidbody>().useGravity = true;
            }
        }
        private void Start()
        {
            try
            {
                this.WaterHeight = GameObject.Find("water0").transform.localPosition.y;
                if (base.GetComponent<Rigidbody>() == null)
                {
                    UnityEngine.Object.Destroy(this);
                    return;
                }
                else
                {
                    float x = base.gameObject.transform.localScale.x;
                    float y = base.gameObject.transform.localScale.y;
                    float z = base.gameObject.transform.localScale.z;
                    if ((x >= y) && (x >= z))
                    {
                        this.minVolue = x;
                    }
                    else if ((y >= x) && (y >= z))
                    {
                        this.minVolue = y;
                    }
                    else
                    {
                        this.minVolue = z;
                    }
                    this.volume = (x * y) * z;
                    this.Drag = base.GetComponent<Rigidbody>().drag;
                    this.AngularDrag = base.GetComponent<Rigidbody>().angularDrag;
                    if (base.GetComponent<FireTag>() != null) fireTag = true;
                    if (base.GetComponent<MyBlockInfo>().blockName == "SMALL WOOD BLOCK")
                    {
                        this.Force = (2f * this.volume) / this.ForceScale;
                    }
                    else if (base.GetComponent<MyBlockInfo>().blockName == "WOODEN BLOCK")
                    {
                        this.Force = (4f * this.volume) / this.ForceScale;
                    }
                    else if (base.GetComponent<MyBlockInfo>().blockName == "WOODEN POLE")
                    {
                        this.Force = (2f * this.volume) / this.ForceScale;
                    }
                    else if (base.GetComponent<MyBlockInfo>().blockName == "WOODEN PANEL")
                    {
                        this.Force = (2f * this.volume) / this.ForceScale;
                    }
                    else if (base.GetComponent<MyBlockInfo>().blockName == "PROPELLER")
                    {
                        this.Force = (4f * this.volume) / this.ForceScale;
                    }
                    else if (base.GetComponent<MyBlockInfo>().blockName == "SMALL PROPELLER")
                    {
                        this.Force = (2f * this.volume) / this.ForceScale;
                    }
                    else if (base.GetComponent<MyBlockInfo>().blockName == "WING")
                    {
                        this.Force = (8f * this.volume) / this.ForceScale;
                    }
                    else if (base.GetComponent<MyBlockInfo>().blockName == "WING PANEL")
                    {
                        this.Force = (4f * this.volume) / this.ForceScale;
                    }
                    else if (base.GetComponent<MyBlockInfo>().blockName == "PROPELLOR SMALL")
                    {
                        this.Force = (2f * this.volume) / this.ForceScale;
                    }
                    else if (base.GetComponent<MyBlockInfo>().blockName == "Rocket")
                    {
                        this.Force = (0.8f * this.volume) / this.ForceScale;
                    }
                    else if (base.GetComponent<MyBlockInfo>().blockName == "WHEEL")
                    {
                        this.Force = (2f * this.volume) / this.ForceScale;
                    }
                    else if (base.GetComponent<MyBlockInfo>().blockName == "LARGE WHEEL")
                    {
                        this.Force = (2f * this.volume) / this.ForceScale;
                    }
                    else if (base.GetComponent<MyBlockInfo>().blockName == "WHEEL FREE")
                    {
                        this.Force = (2f * this.volume) / this.ForceScale;
                    }
                    else if (base.GetComponent<MyBlockInfo>().blockName == "UNPOWERED LARGE WHEEL")
                    {
                        this.Force = (2f * this.volume) / this.ForceScale;
                    }
                    else if (base.GetComponent<MyBlockInfo>().blockName == "FLYING SPIRAL")
                    {
                        this.Force = (0.8f * this.volume) / this.ForceScale;
                    }
                    else if (base.GetComponent<MyBlockInfo>().blockName == "BALLOON")
                    {
                        this.Force = (8f * this.volume) / this.ForceScale;
                    }
                    else if (base.GetComponent<MyBlockInfo>().blockName == "FLAMETHROWER")
                    {
                        this.Force = (0.8f * this.volume) / this.ForceScale;
                    }
                    else
                    {
                        this.Force = 0f;
                    }
                }
            }
            catch (Exception exception)
            {
                GeoTools.Log(exception.ToString());
                UnityEngine.Object.Destroy(this);
            }
        }
    }

}
