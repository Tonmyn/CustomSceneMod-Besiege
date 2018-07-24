using Modding;
using System;
using System.Collections.Generic;
//using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

namespace BesiegeCustomScene
{
    public class LanguageManager : MonoBehaviour
    {

        public enum Language
        {
            CHN = 0,
            EN = 1, 
        }

        public class LanguageFile
        {
            public Dictionary<int, string> dic_Translation;

            public LanguageFile(string path)
            {
                string filePath = path;

                dic_Translation = new Dictionary<int, string>();

                try
                {

                    if (!ModIO.ExistsFile(filePath))
                    {
                        GeoTools.Log("Error! Language File not exists!");
                        return;
                    }

                    //var fs = ModIO.Open(filePath, System.IO.FileMode.Open);
                    //打开数据文件
                    var srd = ModIO.OpenText(filePath);

                    while (srd.Peek() != -1)
                    {
                        string str = srd.ReadLine();
   
                        if (str.Contains(" = ") && str.Length > 2)
                        {
                            //BesiegeConsoleController.ShowMessage(str);

                            string id = str.Substring(0, str.IndexOf(" = "));
                            //BesiegeConsoleController.ShowMessage(id);

                            if (Regex.IsMatch(id, @"^[0-9]*$") && !string.IsNullOrEmpty(id))
                            {
                                int index = str.IndexOf(" = ") + 3;
                                string value = str.Substring(index, str.Length - index).Replace("\"", string.Empty);

                                dic_Translation[int.Parse(id)] = value;
                                //BesiegeConsoleController.ShowMessage(value);
                            }
                        }

                    }
                    
                    srd.Close();

                    //foreach (var v in dic_Translation)
                    //{
                    //    BesiegeConsoleController.ShowMessage(v.Key + "|" + v.Value);
                    //}

                    GeoTools.Log("Read Language File Completed!");
                }
                catch (Exception ex)
                {
                    GeoTools.Log("Read Language File Failed!");
                    GeoTools.Log(ex.ToString());
                    return;
                }
            }
        }

        List<LanguageFile> languageFiles;

        Dictionary<string, LanguageFile> dic_LanguageFile;

        LanguageFile currentLanguageFile = null;

        Language currentLanguage;

        static string languageFilePath = GeoTools.UIPath;

        void Awake()
        {

            languageFiles = new List<LanguageFile>();

            dic_LanguageFile = new Dictionary<string, LanguageFile>();

            try
            {
                for (int i = 0; i < Enum.GetNames(typeof(Language)).Length; i++)
                {
                    string languageName = Enum.GetNames(typeof(Language))[i];
                    Language language = (Language)Enum.Parse(typeof(Language), languageName);

                    LanguageFile languageFile = new LanguageFile(languageFilePath + languageName + ".txt");
                    languageFiles.Add(languageFile);
                    dic_LanguageFile[languageName] = languageFile;
                    //BesiegeConsoleController.ShowMessage("")
                }
            }
            catch (Exception e)
            {
                ConsoleController.ShowMessage(e.Message);
            }

            currentLanguage = (Language)Enum.Parse(typeof(Language), GetComponent<SettingsManager>().settings.language);

            try

            {
                Set_Language(currentLanguage);
            }
            catch (Exception e)
            {
                BesiegeConsoleController.ShowMessage(e.Message);
            }


        }

        //void Update()
        //{
        //    if (Input.GetKeyDown(KeyCode.F))
        //    {
        //        //for (int i = 0; i < languageFiles[0].dic_Translation.Count; i++)
        //        //{
        //        //    BesiegeConsoleController.ShowMessage(languageFiles[0].dic_Translation[i]);
        //        //}
        //        //    ConsoleController.ShowMessage("??");
        //        //    BesiegeConsoleController.ShowMessage(languageFilePath + Enum.GetNames(typeof(Language))[0] + ".txt");

        //        //try
        //        //{
        //        //    LanguageFile lf = new LanguageFile(languageFilePath + Enum.GetNames(typeof(Language))[0] + ".txt");

        //        //    foreach (var v in lf.dic_Translation)
        //        //    {
        //        //        BesiegeConsoleController.ShowMessage(v.Key +"|"+ v.Value+"|");
        //        //    }
        //        //}
        //        //catch (Exception e)
        //        //{
        //        //    BesiegeConsoleController.ShowMessage(e.Message);
        //        //}
        //        //foreach (var v in dic_LanguageFile[Language.CHN.ToString()].dic_Translation)
        //        //{
        //        //    BesiegeConsoleController.ShowMessage(v.Key + v.Value);
        //        //}
        //        //BesiegeConsoleController.ShowMessage(dic_LanguageFile[Language.CHN]);

        //        //languageFiles = new List<LanguageFile>();

        //        //dic_LanguageFile = new Dictionary<string, LanguageFile>();

        //        //try
        //        //{
        //        //    for (int i = 0; i < Enum.GetNames(typeof(Language)).Length; i++)
        //        //    {
        //        //        string languageName = Enum.GetNames(typeof(Language))[i];
        //        //        Language language = (Language)Enum.Parse(typeof(Language), languageName);

        //        //        LanguageFile languageFile = new LanguageFile(languageFilePath + languageName + ".txt");
        //        //        languageFiles.Add(languageFile);
        //        //        dic_LanguageFile[languageName] = languageFile;
        //        //        BesiegeConsoleController.ShowMessage("read lanuage " + languageName);
        //        //    }
        //        //}
        //        //catch (Exception e)
        //        //{
        //        //    ConsoleController.ShowMessage(e.Message);
        //        //}

        //        foreach (var v in Get_CurretLanguageFile().dic_Translation)
        //        {
        //            BesiegeConsoleController.ShowMessage(v.Key + v.Value);
        //        }
        //    }
        //    if (Input.GetKeyDown(KeyCode.G))
        //    {
        //        //foreach (var v in languageFiles[0].dic_Translation)
        //        //{
        //        //    BesiegeConsoleController.ShowMessage(v.Key + v.Value);
        //        //}

        //        foreach (var v in dic_LanguageFile[Language.EN.ToString()].dic_Translation)
        //        {
        //            BesiegeConsoleController.ShowMessage(v.Key + v.Value);
        //        }
        //    }
        //}

        public Language Get_CurretLanguage()
        {
            return currentLanguage;
        }

        public void Set_Language(Language language)
        {
            currentLanguageFile = dic_LanguageFile[language.ToString()];
        }

        public LanguageFile Get_CurretLanguageFile()
        {
            return currentLanguageFile;
        }

    }


}
