using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace BesiegeCustomScene
{
    class LanguageManager : MonoBehaviour
    {

        enum Language
        {
            CHN = 0,
            EN = 1, 
        }

        class LanguageFile
        {
            Dictionary<int, string> dic_Translation;

            public LanguageFile(string path)
            {
                string filePath = path;

                dic_Translation = new Dictionary<int, string>();

                try
                {

                    if (!File.Exists(filePath))
                    {
                        GeoTools.Log("Error! Language File not exists!");
                        return;
                    }

                    FileStream fs = new FileStream(filePath, FileMode.Open);
                    //打开数据文件
                    StreamReader srd = new StreamReader(fs, Encoding.Default);

                    while (srd.Peek() != -1)
                    {
                        string str = srd.ReadLine();
                        string[] chara = str.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                        if (chara.Length > 2)
                        {
                            string value = chara[0].Substring(0, chara[0].IndexOf(" = "));
                            if (Regex.IsMatch(value, @"^[+-]?/d*$"))
                            {
                                dic_Translation.Add(int.Parse(value), chara[0].Substring(chara[0].IndexOf(" = "), chara[0].Length - (chara[0].IndexOf(" = ") + 2)));
                            }
                        }
                    }
                    srd.Close();

                    GeoTools.Log("Read Language Completed!");
                }
                catch (Exception ex)
                {
                    GeoTools.Log("Read Language Failed!");
                    GeoTools.Log(ex.ToString());
                    return;
                }
            }
        }

        LanguageFile[] languageFiles;

        string curretLanguage;

        static string languageFilePath = Application.dataPath + "/Mods/BesiegeCustomScene/UI/";

        void Awake()
        {
            for (int i = 0; i < Enum.GetNames(new Language().GetType()).Length; i++)
            {
                languageFiles[i] = new LanguageFile(languageFilePath + (string)Enum.ToObject(typeof(Language), i) + ".txt");
                
            }
        }


    }


}
