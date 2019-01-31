using Modding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BesiegeCustomScene
{
    /// <summary>语言文件</summary>
    [Obsolete]
    public class LanguageFile
    {
        public Dictionary<int, string> dicTranslation;

        public LanguageFile(string path)
        {
            string filePath = path;

            dicTranslation = new Dictionary<int, string>();

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

                            dicTranslation[int.Parse(id)] = value;
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
}
