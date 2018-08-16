using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Modding;

namespace BesiegeCustomScene
{
    public class DataLoader
    {

        public Dictionary<string,Type> dic_datas;

        public List<string> datas;

        public DataLoader(string[] datas)
        {


           // datas = new List<string>();

           //var textReader = GeoTools.FileReader(data_path);

           // while (textReader.Peek() != -1)
           // {
           //     datas.Add(textReader.ReadLine());
           // }

        }

        public DataLoader()
        {

        }

        public bool ReadBool(string[] str, int index)
        {
            bool value = false;

            if ((Convert.ToInt32(str[index + 1]) == 0) || (str[index+1].ToUpper() == "FALSE") || (str[index+1].ToUpper()=="OFF"))
            {
                value = false;
            }
            if ((Convert.ToInt32(str[index + 1]) == 1) || (str[index + 1].ToUpper() == "TRUE") || (str[index + 1].ToUpper() == "ON"))
            {
                value = true;
            }
            return value;               
        }

        public int? ReadInt(string[] str,int index)
        {
            int? i = 0;
            i = Convert.ToInt32(str[index + 1]);
            return i;
        }

        public float? ReadFloat(string[] str, int index)
        {
            float? i = 0;

            return i = Convert.ToSingle(str[index + 1]);
        }

        public Vector3? ReadVector3(string[] str, int index)
        {
            Vector3 vector = Vector3.zero;

            vector = new Vector3
                    (
                    Convert.ToSingle(str[index + 1]),
                    Convert.ToSingle(str[index + 2]),
                    Convert.ToSingle(str[index + 3])
                    );

            return vector;
        }

        public Vector4? ReadVector4(string[] str, int index)
        {
            Vector4 vector = Vector4.zero;

            vector = new Vector4
                    (
                    Convert.ToSingle(str[index + 1]),
                    Convert.ToSingle(str[index + 2]),
                    Convert.ToSingle(str[index + 3]),
                    Convert.ToSingle(str[index + 4])        
                    );

            return vector;
        }

        public Color? ReadColor(string[] str, int index)
        {
            Color? color = Color.white;

            color = new Color
                    (                             
                    Convert.ToSingle(str[index + 1]),                              
                    Convert.ToSingle(str[index + 2]),                               
                    Convert.ToSingle(str[index + 3]),                               
                    Convert.ToSingle(str[index + 4])                              
                    );

            return color;
        }

        public Texture2D ReadTexture2D(string[] str, int index,bool data)
        {
            Texture2D texture2D = Texture2D.whiteTexture;

            //double startTime = (double)Time.time;
            //创建文件流
            //FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            //fileStream.Seek(0, SeekOrigin.Begin);



            //创建文件长度的缓冲区
            //byte[] bytes = new byte[fileStream.Length];

            byte[] bytes =ModIO.ReadAllBytes(str[index + 1],data);
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



    }
}
