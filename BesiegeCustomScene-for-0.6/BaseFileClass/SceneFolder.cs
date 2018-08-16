using Modding;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BesiegeCustomScene
{
    /// <summary>地图包类</summary>
    public class SceneFolder
    {

        /// <summary>
        /// 地图路径
        /// </summary>
        public string Path;

        /// <summary>
        /// 地图名称
        /// </summary>
        public string Name;

        /// <summary>
        /// 网格文件路径
        /// </summary>
        public string MeshsPath;

        /// <summary>
        /// 贴图文件路径
        /// </summary>
        public string TexturesPath;

        /// <summary>
        /// 设置文件路径
        /// </summary>
        public string SettingFilePath;

        /// <summary>
        /// 设置文件数据
        /// </summary>
        public List<string> SettingFileDatas;

        /// <summary>
        /// 地图包类型
        /// </summary>
        public enum SceneType
        {
            /// <summary>可用</summary>
            Enabled = 0,
            /// <summary>不可用</summary>
            Disable = 1,
            /// <summary>空</summary>
            Empty = 3,
        }

        public SceneType Type;

        public SceneFolder(string scenePath,bool data = false)
        {           
            scenePath = scenePath.Replace(@"\", "/");

            Name = scenePath.Substring(scenePath.LastIndexOf("/") + 1, scenePath.Length - scenePath.LastIndexOf("/") - 1);
            Path = scenePath;
            MeshsPath = Path + "/Meshs";
            TexturesPath = Path + "/Textures";
            SettingFilePath = string.Format("{0}/setting.txt", Path);


            if (ModIO.ExistsFile(string.Format("{0}/setting.txt", Path),data) && ModIO.ExistsDirectory(Path,data))
            {
                Type = SceneType.Enabled;
            }
            else
            {
                Type = SceneType.Empty;
            }

            SettingFileDatas = new List<string>();
            if (Type == SceneType.Enabled)
            {
                var textReader = GeoTools.FileReader(SettingFilePath,data);

                while (textReader.Peek() != -1)
                {
                    SettingFileDatas.Add(textReader.ReadLine());
                }

            }

        }


    }
}
