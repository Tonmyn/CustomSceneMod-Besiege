using System.IO;

namespace BesiegeCustomScene
{
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

            if (!File.Exists(string.Format("{0}/setting.txt", Path)))
            {
                Type = SceneType.Empty;
            }
            else
            {
                Type = SceneType.Enabled;
            }

        }

    }
}
