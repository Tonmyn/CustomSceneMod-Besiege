using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CustomScene
{
    public class MiniMapMod : MonoBehaviour
    {

        GameObject miniMapCamera;

        public RenderTexture cameraRenderTexture = new RenderTexture(Screen.width, Screen.height, 16);

        GameObject mainCamera;

        Camera camera;

        //小地图相机与主摄像机的距离
        float distance;

        void Start()
        {
            mainCamera = GameObject.Find("Main Camera");
            distance = 100;

            InitCamera();
        }

        void InitCamera()
        {
            miniMapCamera = new GameObject("Mini Map Camera");
            miniMapCamera.transform.SetParent(transform);
            camera = miniMapCamera.AddComponent<Camera>();
            camera.gameObject.AddComponent<CameraShadowControl>();
            camera.transform.rotation = Quaternion.LookRotation(Vector3.down);
            camera.fieldOfView = 43;
        }


        void Update()
        {
            miniMapCamera.transform.position = mainCamera.transform.position + Vector3.up * distance;
            camera.targetTexture = cameraRenderTexture;
        }
    }

    class CameraShadowControl : MonoBehaviour
    {
        float storedShadowDistance;

        void OnPreRender()
        {
            storedShadowDistance = QualitySettings.shadowDistance;
            QualitySettings.shadowDistance = 0;
        }


        void OnPostRender()
        {
            QualitySettings.shadowDistance = storedShadowDistance;
        }
    }
}
