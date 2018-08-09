using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace BesiegeCustomScene.UI
{
    class MiniMapSettingUI : MonoBehaviour
    {
        MiniMapMod miniMapMod;

        Rect windowRect;

        private int windowID = GeoTools.GetWindowID();

        private float width = 306;
        private float textureWidth;

        void Start()
        {
            GameObject go = new GameObject("Mini Map Mod");
            go.transform.SetParent(transform);
            miniMapMod = go.AddComponent<MiniMapMod>();
            textureWidth = width - 6;
            windowRect = new Rect(15f, 100f, width, width * miniMapMod.cameraRenderTexture.height / miniMapMod.cameraRenderTexture.width + 20);

        }

        void OnGUI()
        {
            windowRect = GUI.Window(windowID, windowRect, new GUI.WindowFunction(MiniMapWindow), "小地图");
        }

        void MiniMapWindow(int WindowID)
        {
            GUI.DrawTexture(new Rect((width - textureWidth) / 2, 18, textureWidth, textureWidth * Screen.height / Screen.width), miniMapMod.cameraRenderTexture, ScaleMode.ScaleToFit);
        }
    }
}
