using Modding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CustomScene
{
    [Obsolete]
    public class SettingFile
    {
        /// <summary>设置路径</summary>
        public static string SettingsPath = GeoTools.ModPath + "Settings.txt";

        public Settings settings;

        public struct Settings
        {
            public KeyCode sceneUI;

            public KeyCode sceneUI_refresh;

            public bool sceneUI_showOnAwake;

            public KeyCode timerUI;

            public KeyCode timerUI_refresh;

            public bool timerUI_showOnAwake;

            public string language;

            public List<string> sceneNames;

            public static Settings Default { get; } = new Settings
            {
                sceneUI = KeyCode.F9,
                sceneUI_refresh = KeyCode.F5,
                sceneUI_showOnAwake = true,
                timerUI = KeyCode.F8,
                timerUI_refresh = KeyCode.F5,
                timerUI_showOnAwake = true,
                language = "CHN",
                sceneNames = new List<string>()
            };

        }

        public SettingFile()
        {
            string filePath = SettingsPath;

            settings = Settings.Default;

            try
            {

                if (!ModIO.ExistsFile(filePath))
                {
                    GeoTools.Log("Error! Settings.txt File not exists!");
                    return;
                }

                var srd = ModIO.OpenText(filePath);

                while (srd.Peek() != -1)
                {
                    string str = srd.ReadLine();
                    string[] chara = str.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    if (chara.Length >= 2)
                    {
                        #region Camera
                        if (chara[0] == "_Scene")
                        {
                            if (chara[1] == "display_UI")
                            {
                                settings.sceneUI = (KeyCode)Enum.Parse(typeof(KeyCode), chara[2].ToUpper(), true);
                            }
                            else if (chara[1] == "reload_UI")
                            {
                                settings.sceneUI_refresh = (KeyCode)Enum.Parse(typeof(KeyCode), chara[2].ToUpper(), true);
                            }
                            else if (chara[1] == "show_on_start")
                            {
                                if (chara[2] == "0" || chara[2] == "OFF") settings.sceneUI_showOnAwake = false;
                            }
                            else if (chara[1].ToLower() == "scenename")
                            {
                                settings.sceneNames.Add(chara[2]);
                            }

                        }
                        else if (chara[0] == "_Timer")
                        {
                            if (chara[1] == "display_UI")
                            {
                                settings.timerUI = (KeyCode)Enum.Parse(typeof(KeyCode), chara[2].ToUpper(), true);
                            }
                            else if (chara[1] == "reload_UI")
                            {
                                settings.timerUI_refresh = (KeyCode)Enum.Parse(typeof(KeyCode), chara[2].ToUpper(), true);
                            }
                            else if (chara[1] == "show_on_start")
                            {
                                if (chara[2] == "0" || chara[2] == "OFF") settings.timerUI_showOnAwake = false;
                            }
                        }
                        else if (chara[0] == "_Language")
                        {
                            if (chara[2] != settings.language)
                            {
                                settings.language = chara[2];
                            }
                        }
                        #endregion
                    }
                }
                srd.Close();

                GeoTools.Log("Read Settings Completed!");
            }
            catch (Exception ex)
            {
                GeoTools.Log("Read Settings Failed!");
                GeoTools.Log(ex.ToString());
                return;
            }

        }
    }

}

