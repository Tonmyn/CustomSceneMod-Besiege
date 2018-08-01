using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BesiegeCustomScene
{
    public class MiniMapMod : MonoBehaviour
    {

        GameObject miniMapCamera;

        public RenderTexture cameraRenderTexture;

        GameObject mainCamera;

        Camera camera;

        //小地图相机与主摄像机的距离
        float distance;

        void Start()
        {
            mainCamera = GameObject.Find("Main Camera");
            distance = 100;

            initCamera();
        }

        void initCamera()
        {
            miniMapCamera = new GameObject("Mini Map Camera");
            miniMapCamera.transform.SetParent(transform);
            camera = miniMapCamera.AddComponent<Camera>();
           

        }


        void Update()
        {
            miniMapCamera.transform.position = mainCamera.transform.position + Vector3.up * distance;
            cameraRenderTexture = camera.targetTexture;

        }
    }
}
