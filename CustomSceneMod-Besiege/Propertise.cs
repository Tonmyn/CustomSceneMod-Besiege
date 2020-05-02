using Modding.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomScene
{
    public enum PropertiseType
    {
        Terrain = 0,
        Cloud = 1,
        Sky = 2,
        Snow = 3,
        Water = 4,
        Wind = 5,
        Camera = 20
    }

    public interface IPropertise
    {
         PropertiseType PropertiseType { get; }
    }

    abstract public class Propertise :IPropertise
    {
        public PropertiseType PropertiseType { get; }
    
        abstract public Propertise FormatPropertise();

    }

    public class TransformPropertise : Element
    {
        [CanBeEmpty]
        public Vector3 Position { get; set; } = Vector3.zero;
        [CanBeEmpty]
        public Vector3 Rotation { get; set; } = Vector3.zero;
        [CanBeEmpty]
        public Vector3 Scale { get; set; } = Vector3.one;
    }

    public class MeshPropertise : TransformPropertise
    {
        [RequireToValidate]
        public string MeshName { get; set; } = "Mesh Name";
        [RequireToValidate]
        public string TextureName { get; set; } = "Texture Name";
        [CanBeEmpty]
        public string MeshVersion { get; set; } = "V1.0";
    }
}
