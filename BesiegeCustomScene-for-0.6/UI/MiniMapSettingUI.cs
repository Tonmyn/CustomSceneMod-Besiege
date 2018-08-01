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

        RawImage rawImage;

        Rect windowRect = new Rect(15f, 100f, 180f, 200f);

        int windowID = GeoTools.GetWindowID();

        void Start()
        {
            GameObject go = new GameObject("Mini Map Mod");
            go.transform.SetParent(transform);
            miniMapMod = go.AddComponent<MiniMapMod>();

            rawImage = gameObject.AddComponent<RawImage>();
        }

        void OnGUI()
        {
            windowRect = GUI.Window(windowID, windowRect, new GUI.WindowFunction(MiniMapWindow), "小地图");
        }

        void MiniMapWindow(int WindowID)
        {
            GUILayout.BeginArea(new Rect(0, 0, 250, 250));
            {
                rawImage.useGUILayout = true;
                rawImage.texture = miniMapMod.cameraRenderTexture;
            }
            GUILayout.EndArea();
        }
    }
}
