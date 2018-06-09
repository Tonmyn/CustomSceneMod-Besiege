using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace BesiegeCustomScene
{
    public class WaterMod : MonoBehaviour
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
        private GameObject[] Mwater;
        private int WaterSize = 0;
        [Obsolete]
        public void ReadScene(string SceneName)
        {
            WaterSize = 0;
            try
            {
                //  GeoTools.Log(Application.dataPath);
                if (!File.Exists(GeoTools.ScenePath + SceneName + ".txt"))
                {
                    GeoTools.Log("Error! Scene File not exists!");
                    return;
                }
                StreamReader srd = File.OpenText(GeoTools.ScenePath + SceneName + ".txt");
                while (srd.Peek() != -1)
                {
                    string str = srd.ReadLine();
                    string[] chara = str.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    if (chara.Length > 2)
                    {
                        #region Water
                        if (chara[0] == "MWater" || chara[0] == "Mwater")
                        {
                            if (chara[1] == "size")
                            {
                                this.WaterSize = Convert.ToInt32(chara[2]);
                                LoadWater();
                            }
                            if (chara[1] == "watertemp")
                            {
                                if (Convert.ToInt32(chara[2]) == 0)
                                {
                                    gameObject.GetComponent<Prop>().WaterTemp.SetActive(false);
                                }
                                else
                                {
                                    gameObject.GetComponent<Prop>().WaterTemp.SetActive(true);
                                }
                            }
                        }
                        else if (chara[0] == "Water")
                        {
                            int i = Convert.ToInt32(chara[1]);
                            if (chara[2] == "mesh")
                            {
                                Mwater[i].GetComponent<MeshFilter>().mesh = GeoTools.MeshFromObj(chara[3]);
                            }
                            else if (chara[2] == "wmesh")
                            {
                                Mwater[i].GetComponent<MeshFilter>().mesh = GeoTools.WMeshFromObj(chara[3]);
                            }
                            else if (chara[2] == "scale")
                            {
                                Mwater[i].transform.localScale = new Vector3(
                               Convert.ToSingle(chara[3]),
                               Convert.ToSingle(chara[4]),
                               Convert.ToSingle(chara[5]));
                            }
                            else if (chara[2] == "location")
                            {
                                Mwater[i].transform.localPosition = new Vector3(
                               Convert.ToSingle(chara[3]),
                               Convert.ToSingle(chara[4]),
                               Convert.ToSingle(chara[5]));
                            }
                            else if (chara[2] == "meshcollider")
                            {
                                Mwater[i].GetComponent<MeshCollider>().sharedMesh = GeoTools.MeshFromObj(chara[3]);
                                Mwater[i].GetComponent<MeshCollider>().convex = true;
                                Mwater[i].GetComponent<MeshCollider>().isTrigger = true;
                            }
                            else if (chara[2] == "wmeshcollider")
                            {
                                Mwater[i].GetComponent<MeshCollider>().sharedMesh = GeoTools.WMeshFromObj(chara[3]);
                                Mwater[i].GetComponent<MeshCollider>().convex = true;
                                Mwater[i].GetComponent<MeshCollider>().isTrigger = true;
                            }
                        }
                        #endregion
                    }
                }
                srd.Close();
                GeoTools.Log("ReadWater Completed!");
            }
            catch (Exception ex)
            {
                GeoTools.Log("Error! ReadWater Failed!");
                GeoTools.Log(ex.ToString());
                return;
            }
        }
        public void ReadScene(ScenePack scenePack)
        {
            WaterSize = 0;
            try
            {
                ClearWater();

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
                        #region Water
                        if (chara[0] == "MWater" || chara[0] == "Mwater")
                        {
                            if (chara[1] == "size")
                            {
                                this.WaterSize = Convert.ToInt32(chara[2]);
                                LoadWater();
                            }
                            if (chara[1] == "watertemp")
                            {
                                if (Convert.ToInt32(chara[2]) == 0)
                                {
                                    gameObject.GetComponent<Prop>().WaterTemp.SetActive(false);
                                }
                                else
                                {
                                    gameObject.GetComponent<Prop>().WaterTemp.SetActive(true);
                                }
                            }
                        }
                        else if (chara[0] == "Water")
                        {
                            int i = Convert.ToInt32(chara[1]);
                            if (chara[2] == "mesh")
                            {
                                Mwater[i].GetComponent<MeshFilter>().mesh = GeoTools.MeshFromObj(chara[3]);
                            }
                            else if (chara[2] == "wmesh")
                            {
                                Mwater[i].GetComponent<MeshFilter>().mesh = GeoTools.WMeshFromObj(chara[3]);
                            }
                            else if (chara[2] == "scale")
                            {
                                Mwater[i].transform.localScale = new Vector3(
                               Convert.ToSingle(chara[3]),
                               Convert.ToSingle(chara[4]),
                               Convert.ToSingle(chara[5]));
                            }
                            else if (chara[2] == "location")
                            {
                                Mwater[i].transform.localPosition = new Vector3(
                               Convert.ToSingle(chara[3]),
                               Convert.ToSingle(chara[4]),
                               Convert.ToSingle(chara[5]));
                            }
                            else if (chara[2] == "meshcollider")
                            {
                                Mwater[i].GetComponent<MeshCollider>().sharedMesh = GeoTools.MeshFromObj(chara[3]);
                                Mwater[i].GetComponent<MeshCollider>().convex = true;
                                Mwater[i].GetComponent<MeshCollider>().isTrigger = true;
                            }
                            else if (chara[2] == "wmeshcollider")
                            {
                                Mwater[i].GetComponent<MeshCollider>().sharedMesh = GeoTools.WMeshFromObj(chara[3]);
                                Mwater[i].GetComponent<MeshCollider>().convex = true;
                                Mwater[i].GetComponent<MeshCollider>().isTrigger = true;
                            }
                        }
                        #endregion
                    }
                }
                srd.Close();
                GeoTools.Log("ReadWater Completed!");
            }
            catch (Exception ex)
            {
                GeoTools.Log("Error! ReadWater Failed!");
                GeoTools.Log(ex.ToString());
                return;
            }
        }
        public void LoadWater()
        {
            try
            {
                
                if (WaterSize <= 0) return;
                if (this.gameObject.GetComponent<Prop>().TileTemp == null) return;              
                Mwater = new GameObject[WaterSize];
                for (int i = 0; i < Mwater.Length; i++)
                {
                    Mwater[i] = Instantiate(gameObject.GetComponent<Prop>().TileTemp);
                    Mwater[i].name = "water" + i.ToString();
                    Mwater[i].SetActive(true);
                    Mwater[i].transform.localScale = new Vector3(1, 1, 1);
                    Mwater[i].transform.localPosition = new Vector3(0, 0, 0);
                }
            }
            catch (Exception ex)
            {
                GeoTools.Log("LoadWater Failed");
                GeoTools.Log(ex.ToString());
            }
        }
        public void ClearWater()
        {
            ClearFloater();
            if (Mwater == null) return;
            if (Mwater.Length <= 0) return;
            if (WaterSize > 0) GeoTools.Log("ClearWater");
            gameObject.GetComponent<Prop>().WaterTemp.SetActive(false);
            try
            {
                Destroy(GameObject.Find("WaterTempReflectionMain Camera"));
            }
            catch { }
            for (int i = 0; i < Mwater.Length; i++)
            {
                Destroy(Mwater[i]);
            }

            Mwater = null;
            WaterSize = 0;
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
