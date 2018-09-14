using Localisation;
using System;
using UnityEngine;

namespace BesiegeCustomScene
{
    public class LanguageManager : MonoBehaviour
    {

        public static readonly bool isChinese = LocalisationManager.Instance.currLangName.Contains("中文");

        //Mini Map
        public static string MiniMapTitle = isChinese ? "小地图" : "Mini Map";

        //Block Information
        public static string BlockInformationTitle = isChinese ? "速度计" : "Speedometer";

        public static string AccelerationLabel = isChinese ? "加速度" : "Acceleration";

        public static string VelocityLabel = isChinese ? "速度" : "Velocity";

        public static string PositionLabel = isChinese ? "位置" : "Position";

        public static string DistanceLabel = isChinese ? "里程" : "Distance";

        public static string GForceLabel = isChinese ? "过载" : "G-Force";

        public static string TimerLabel = isChinese ? "计时器" : "Timer";

        public static string TimeLabel = isChinese ? "时间" : "Time";

        public static string TriggerLabel = isChinese ? "触发器" : "Trigger";

        //Scene Setting
        public static string SceneWindowTitle = isChinese ? "地形设置" : "Scene Setting";

        public static string FogButtonLabel = isChinese ? "去掉迷雾" : "No Fog";

        public static string WorldBoundsButtonLabel = isChinese ? "去空气墙" : "No Bounds";

        public static string FloorGridButtonLabel = isChinese ? "去掉地板" : "No Floor";

        public static string SceneListLabel = isChinese ? "地形列表" : "Scenes List";

        public static string ReloadButonLabel = isChinese ? "重载地图" : "Reload Scene";

        public static string ScenesDirectoryButtonLabel = isChinese ? "打开地图文件夹" : "Scenes Folder";
    }
}
