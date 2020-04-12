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
}
