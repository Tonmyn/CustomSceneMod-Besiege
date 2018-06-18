using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BesiegeCustomScene
{
    public class SkyMod : MonoBehaviour
    {
        void Start()
        {
            BesiegeConsoleController.ShowMessage("sky mod");
        }


        void Update()
        {
            if (Input.GetKey(KeyCode.Q))
            {
                GameObject.Find("Main Camera").GetComponent<Camera>().farClipPlane += 100;

                GameObject.Find("Main Camera").GetComponent<ColorfulFog>().enabled = false;

                GameObject.Find("Fog Volume").GetComponent<MeshRenderer>().enabled = false;
            }
        }
    }
}
