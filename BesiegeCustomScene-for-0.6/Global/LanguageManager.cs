using Modding;
using System;
using System.Collections.Generic;
//using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

namespace BesiegeCustomScene
{
    public class LanguageManager : MonoBehaviour
    {

        public static readonly bool isChinese = (Application.systemLanguage == SystemLanguage.Chinese);

        //Mini Map
        public static String MiniMapTitle = isChinese ? "小地图" : "Mini Map";

        //Block Information
        public static String BlockInformationTitle = isChinese ? "速度计" : "Speedometer";

        public static String AccelerationLabel = isChinese ? "加速度" : "Acceleration";

        public static String VelocityLabel = isChinese ? "速度" : "Velocity";

        public static String PositionLabel = isChinese ? "位置" : "Position";

        public static String DistanceLabel = isChinese ? "里程" : "Distance";

        public static String GForceLabel = isChinese ? "过载" : "G-Force";

        public static String TimerLabel = isChinese ? "计时器" : "Timer";

        public static String TimeLabel = isChinese ? "时间" : "Time";

        public static String TriggerLabel = isChinese ? "触发器" : "Trigger";

        //Scene Setting
        public static String SceneWindowTitle = isChinese ? "地形设置" : "Scene Setting";

        public static String FogButtonLabel = isChinese ? "去掉迷雾" : "No Fog";

        public static String WorldBoundsButtonLabel = isChinese ? "去空气墙" : "No Bounds";

        public static String FloorGridButtonLabel = isChinese ? "去掉地板" : "No Floor";

        public static String SceneListLabel = isChinese ? "地形列表" : "Scene List";
    }
}
