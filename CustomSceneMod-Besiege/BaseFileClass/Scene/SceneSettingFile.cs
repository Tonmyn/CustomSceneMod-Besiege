using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Modding.Serialization;
using UnityEngine;

namespace CustomScene
{
    [Obsolete]
    public class SceneSettingFile : Element
    {
        [RequireToValidate]
        public string AuthorName;
        [RequireToValidate]
        public string SceneDescription;
        //[CanBeEmpty]
        //public CameraPropertise CameraPropertise;
        //[CanBeEmpty]
        //public CloudPropertise CloudPropertise;
        [CanBeEmpty]
        public TerrainPropertise MeshsPropertise;
        [CanBeEmpty]
        public TerrainUnitPropertise[] MeshPropertises;
        //[CanBeEmpty]
        //public SkyPropertise SkyPropertise;
        //[CanBeEmpty]
        //public SnowPropertise SnowPropertise;
        //[CanBeEmpty]
        //public WaterPropertise WaterPropertise;


        public override string ToString()
        {
            //StringBuilder sb = new StringBuilder();
            //sb.Append(string.Format("Scene Authoer:{0}\nScene Description:{1}\nPropertise:\n", AuthorName, SceneDescription));

            //foreach (var pro in Propertise)
            //{
            //    sb.Append(pro.ToString());
            //}
            //return sb.ToString();


            return string.Format("Scene Authoer:{0}\nScene Description:{1}\nPropertise:\n{2}", AuthorName, SceneDescription, MeshsPropertise);
        }
    }
}
