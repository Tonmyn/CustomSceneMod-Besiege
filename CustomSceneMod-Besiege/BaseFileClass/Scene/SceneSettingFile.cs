using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Modding.Serialization;

namespace CustomScene
{
    public class SceneSettingFile : Element
    {
        [RequireToValidate]
        public string AuthorName { get; set; }
        [RequireToValidate]
        public string SceneDescription { get; set; }
        [CanBeEmpty]
        public CameraPropertise CameraPropertise { get; }
        [CanBeEmpty]
        public CloudPropertise CloudPropertise { get; }
        [CanBeEmpty]
        public MeshPropertise MeshPropertise { get; }
        [CanBeEmpty]
        public SkyPropertise SkyPropertise { get; }
        [CanBeEmpty]
        public SnowPropertise SnowPropertise { get; }
        [CanBeEmpty]
        public WaterPropertise WaterPropertise { get; }


        public override string ToString()
        {
            return string.Format("{0},{1},{2}", AuthorName, SceneDescription,CameraPropertise);
        }
    }
}
