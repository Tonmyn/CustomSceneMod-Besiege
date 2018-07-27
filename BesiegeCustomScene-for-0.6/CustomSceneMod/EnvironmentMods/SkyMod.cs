using Modding;
using System;
using System.Collections.Generic;
//using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace BesiegeCustomScene
{
    class SkyMod : EnvironmentMod
    {

        Mesh skyBallMesh;
        Texture skyBallTexture;
        GameObject starSphere;
        GameObject skySphere;

        string skyBoxTexturePath;
        //string skyBoxMeshPath;

        public void ReadScene(CustomSceneMod.ScenePack scenePack)
        {
            //skyBoxTexturePath = scenePack.TexturesPath + "/SkyBoxTexture.jpg";
            //skyBoxMeshPath = scenePack.MeshsPath + "/Skydome.obj";

            
            //try
            //{
            //    if (!File.Exists(scenePack.SettingFilePath))
            //    {
            //        GeoTools.Log("Error! Scene File not exists!");
            //        return;
            //    }

            //    FileStream fs = new FileStream(scenePack.SettingFilePath, FileMode.Open);

            //    //打开数据文件
            //    StreamReader srd = new StreamReader(fs, Encoding.Default);

            //    while (srd.Peek() != -1)
            //    {
            //        string str = srd.ReadLine();
            //        string[] chara = str.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            //        if (chara.Length > 2)
            //        {
            //            #region Mesheses
            //            if (chara[0] == "Sky")
            //            {
            //                skyBoxTexturePath = scenePack.TexturesPath + "/" + chara[1];
            //                create();
            //            }
            //            #endregion                 
            //        }
            //    }
            //    srd.Close();
            //    //   for (int i = 0; i < this.meshes.Length; i++){GeoTools.MeshFilt(ref this.meshes[i]);}
            //    GeoTools.Log("Read Sky Completed!");
            //}
            //catch (Exception ex)
            //{
            //    GeoTools.Log("Error! Read Sky Failed!");
            //    GeoTools.Log(ex.ToString());
            //    return;
            //}

          
        }

        public override void ReadEnvironment(CustomSceneMod.ScenePack scenePack)
        {
            ClearEnvironment();

            foreach (var str in scenePack.SettingFileDatas)
            {
                string[] chara = str.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                if (chara.Length > 2)
                {
                    if (chara[0] == "Sky")
                    {
                        skyBoxTexturePath = scenePack.TexturesPath + "/" + chara[1];
                        //create();
                    }
                }
            }
        }

        public override void LoadEnvironment()
        {
            skyBallMesh = getMesh();
            skyBallTexture = getTexture(skyBoxTexturePath);
            
            starSphere = GameObject.Find("STAR SPHERE");
            if (starSphere != null)
            {
                starSphere.SetActive(false);
            }

            skySphere = CreateSkyMaterialBall(skyBallMesh,skyBallTexture);

            skySphere.AddComponent<CameraFollower>();

        }

        public override void ClearEnvironment()
        {
          
            Destroy(starSphere);
            Destroy(skyBallMesh);
            Destroy(skyBallTexture);

            skyBoxTexturePath = "";

            int childCount = skySphere.transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                Destroy(skySphere.transform.GetChild(i).gameObject);
            }
            Destroy(skySphere);
        }

        Mesh getMesh()
        {
            Mesh mesh;

            GameObject a = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            mesh = a.GetComponent<MeshFilter>().mesh;
            Destroy(a);
            return mesh;
            //skyBallMesh = GeoTools.MeshFromObj(skyBoxMeshPath, true);
        }

        Texture2D getTexture(string path)
        {
            //skyBallTexture = LoadByIO(skyBoxTexturePath);

            Texture2D texture2D = Texture2D.whiteTexture;

            //double startTime = (double)Time.time;
            //创建文件流
            //FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            //fileStream.Seek(0, SeekOrigin.Begin);



            //创建文件长度的缓冲区
            //byte[] bytes = new byte[fileStream.Length];

            byte[] bytes = ModIO.ReadAllBytes(path);
            //读取文件
            //fileStream.Read(bytes, 0, (int)fileStream.Length);
            //释放文件读取liu
            //fileStream.Close();
            //fileStream.Dispose();
            //fileStream = null;

            //创建Texture
            int width = 300;
            int height = 372;
            texture2D = new Texture2D(width, height);
            texture2D.LoadImage(bytes);
            return texture2D;
        }

        GameObject CreateSkyMaterialBall(Mesh mesh,Texture texture2D)
        {
            GameObject go = new GameObject("SKY SPHERE");
            try
            {         
                go.AddComponent<MeshFilter>().mesh = mesh;
                MeshRenderer mr = go.AddComponent<MeshRenderer>();

                //mr.material = new Material(Shader.Find("Custom/ReflectBumpEmiss/late"));
                mr.material = new Material(Shader.Find("Particles/Alpha Blended"));
                mr.material.mainTexture = texture2D;
                mr.material.mainTexture.wrapMode = TextureWrapMode.Clamp;
                mr.receiveShadows = false;
                mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

                BesiegeConsoleController.ShowMessage("create sky material ball");
                return go;
                
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                //Debug.Log("Exception happened! Check if there is such a texture file named \"SkyBoxTexture.jpg\" \n under \\Besiege_Data\\Mods\\Blocks\\Resources\\ and a obj file called \"Skydome.obj\"! "); return;
                return go = null;
            }
        }

        //private Texture2D LoadByIO(string path)
        //{
        //    //Image image = gameObject.AddComponent<Image>();

        //    double startTime = (double)Time.time;
        //    //创建文件流
        //    FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
        //    fileStream.Seek(0, SeekOrigin.Begin);
        //    //创建文件长度的缓冲区
        //    byte[] bytes = new byte[fileStream.Length];
        //    //读取文件
        //    fileStream.Read(bytes, 0, (int)fileStream.Length);
        //    //释放文件读取liu
        //    fileStream.Close();
        //    fileStream.Dispose();
        //    fileStream = null;

        //    //创建Texture
        //    int width = 300;
        //    int height = 372;
        //    Texture2D texture2D = new Texture2D(width, height);
        //    texture2D.LoadImage(bytes);

        //    return texture2D;

        //    //Sprite sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height),
        //    //    new Vector2(0.5f, 0.5f));
        //    //image.sprite = sprite;
        //    //double time = (double)Time.time - startTime;
        //    //Debug.Log("IO加载用时：" + time);
        //}
    }

    public class CameraFollower : MonoBehaviour
    {
        Camera main;

        void Start()
        {
            main = GameObject.Find("Main Camera").GetComponent<Camera>();
            this.transform.localScale = Vector3.one * main.farClipPlane / /*42*/ 10;
        }

        void Update()
        {
            this.transform.position = main.transform.position;      
        }

    }
}
