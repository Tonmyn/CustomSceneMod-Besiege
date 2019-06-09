using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Modding.Serialization;
using UnityEngine;

namespace CustomScene
{
    public class SceneSettingFile : Element
    {
        [RequireToValidate]
        public string AuthorName;
        [RequireToValidate]
        public string SceneDescription;
        //[RequireToValidate]
        //public Serializable.Object Texture;
        //[CanBeEmpty]
        //public CameraPropertise CameraPropertise;
        //[CanBeEmpty]
        //public CloudPropertise CloudPropertise;
        [CanBeEmpty]
        public MeshsPropertise MeshsPropertise;
        [CanBeEmpty]
        public MeshPropertise[] MeshPropertises;
        //[CanBeEmpty]
        //public SkyPropertise SkyPropertise;
        //[CanBeEmpty]
        //public SnowPropertise SnowPropertise;
        //[CanBeEmpty]
        //public WaterPropertise WaterPropertise;
        //[RequireToValidate]
        //public EnvironmentPropertise[] Propertise;

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("Scene Authoer:{0}\nScene Description:{1}\nMeshs Propertise:{2}\n", AuthorName, SceneDescription,MeshsPropertise));

            foreach (var pro in MeshPropertises)
            {
                sb.Append("\t" + pro.ToString() + "\n");
            }
            return sb.ToString();


           //return string.Format("Scene Authoer:{0}\nScene Description:{1}\nPropertise:\n{2}", AuthorName, SceneDescription,MeshPropertise);
        }
    }
}
