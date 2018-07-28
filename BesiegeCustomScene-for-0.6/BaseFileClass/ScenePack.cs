using Modding;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BesiegeCustomScene
{
    /// <summary>地图包类</summary>
    public class ScenePack
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

        public ScenePack(DirectoryInfo folderName)
        {

            Name = folderName.Name;
            Path = folderName.FullName;
            MeshsPath = Path + "/Meshs";
            TexturesPath = Path + "/Textures";
            SettingFilePath = string.Format("{0}/setting.txt", Path);

            if (!ModIO.ExistsFile(string.Format("{0}/setting.txt", Path)))
            {
                Type = SceneType.Empty;
            }
            else
            {
                Type = SceneType.Enabled;
            }

            SettingFileDatas = new List<string>();
            if (Type == SceneType.Enabled)
            {
                var textReader = GeoTools.FileReader(SettingFilePath);

                while (textReader.Peek() != -1)
                {
                    SettingFileDatas.Add(textReader.ReadLine());
                }
            }

        }

    }
}
