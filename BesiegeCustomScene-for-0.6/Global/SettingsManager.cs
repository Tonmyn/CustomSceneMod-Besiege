﻿using Modding;
using System;
using System.Collections.Generic;
//using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BesiegeCustomScene
{
    public class SettingsManager :MonoBehaviour
    {

        public SettingFile settingFile;
    
        void Awake()
        {
            settingFile = new SettingFile();         
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                //new SettingsFile();
                BesiegeConsoleController.ShowMessage("??show  setting manager");
            }
        }

    }


}
