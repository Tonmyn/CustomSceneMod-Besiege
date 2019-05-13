using System;
using UnityEngine;

namespace CustomScene
{
   
    public class WaterTile : MonoBehaviour
    {
        public PlanarReflection reflection;
        public WaterBase waterBase;


        public void Start()
        {
            AcquireComponents();
        }


        void AcquireComponents()
        {
            if (!reflection)
            {
                if (transform.parent)
                {
                    reflection = transform.parent.GetComponent<PlanarReflection>();
                }
                else
                {
                    reflection = transform.GetComponent<PlanarReflection>();
                }
            }

            if (!waterBase)
            {
                if (transform.parent)
                {
                    waterBase = transform.parent.GetComponent<WaterBase>();
                }
                else
                {
                    waterBase = transform.GetComponent<WaterBase>();
                }
            }
        }


#if UNITY_EDITOR
        public void Update()
        {
            AcquireComponents();
        }
#endif


        public void OnWillRenderObject()
        {
           // Camera cam=GameObject.Find("Main Camera").GetComponent<Camera>();
            if (reflection)
            {
                reflection.WaterTileBeingRendered(transform, Camera.current);
                //reflection.WaterTileBeingRendered(transform, cam);
            }
            if (waterBase)
            {
                waterBase.WaterTileBeingRendered(transform, Camera.current);
               // waterBase.WaterTileBeingRendered(transform, cam);
            }
        }
    }
}