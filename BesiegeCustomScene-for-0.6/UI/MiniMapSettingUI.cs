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

        private readonly int windowID = GeoTools.GetWindowID();

        private float width;
        private readonly float boarderWidth = 3;
        private readonly float textureWidth = 300;

        void Start()
        {
            GameObject go = new GameObject("Mini Map Mod");
            go.transform.SetParent(transform);
            miniMapMod = go.AddComponent<MiniMapMod>();
            width = textureWidth + boarderWidth * 2;
            windowRect = new Rect(15f, 100f, width, width * miniMapMod.cameraRenderTexture.height / miniMapMod.cameraRenderTexture.width + 20);

        }

        void OnGUI()
        {
            windowRect = GUI.Window(windowID, windowRect, new GUI.WindowFunction(MiniMapWindow), LanguageManager.MiniMapTitle);
        }

        void MiniMapWindow(int WindowID)
        {
            GUI.DrawTexture(new Rect(boarderWidth, 18, textureWidth, textureWidth * Screen.height / Screen.width), miniMapMod.cameraRenderTexture, ScaleMode.ScaleToFit);
        }
    }
}
