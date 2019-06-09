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
        [RequireToValidate]
        public Serializable.Shader Texture; 
        //[CanBeEmpty]
        //public CameraPropertise CameraPropertise;
        //[CanBeEmpty]
        //public CloudPropertise CloudPropertise;
        //[CanBeEmpty]
        //public MeshPropertise MeshPropertise;
        //[CanBeEmpty]
        //public SkyPropertise SkyPropertise;
        //[CanBeEmpty]
        //public SnowPropertise SnowPropertise;
        //[CanBeEmpty]
        //public WaterPropertise WaterPropertise;


        public override string ToString()
        {
            return string.Format("{0}\n{1}\n{2}\n", AuthorName, SceneDescription,Texture);
        }
    }
}
