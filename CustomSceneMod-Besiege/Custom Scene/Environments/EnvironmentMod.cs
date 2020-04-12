using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Modding;
using Modding.Serialization;

namespace CustomScene
{
    abstract public class EnvironmentMod<T> where T : EnvironmentPropertise
    {
        public abstract string Path { get; }
        public abstract T Propertise { get; set; }
        public abstract bool Enabled { get; protected set; }
        public int TotalWorkNumber { get; protected set; } = 1;
        public int CurrentWorkNumber { get; protected set; } = 0;
        public abstract  void Load(Transform transform);
        public abstract void Clear();
    }

    public abstract class EnvironmentPropertise : Element
    {


    }
}
