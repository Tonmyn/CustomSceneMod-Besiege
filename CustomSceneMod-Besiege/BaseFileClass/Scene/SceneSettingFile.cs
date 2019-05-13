using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Modding.Serialization;

namespace CustomScene
{  
    public class SceneSettingFile
    {
        [CanBeEmpty]
        public string AuthorName { get; }
        [CanBeEmpty]
        public Version SceneVersion { get; }
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
    }
}
