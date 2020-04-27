using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Modding;
using Modding.Serialization;

namespace CustomScene
{
    abstract public class EnvironmentMod<T> where T : IEnvironmentPropertise
    {
        public abstract string Path { get; }
        public abstract string PropertisePath { get; }
        public abstract bool Data { get; set; }
        public abstract T Propertise { get; set; }
        public abstract bool Enabled { get; protected set; }
        public int TotalWorkNumber { get; protected set; } = 1;
        public int CurrentWorkNumber { get; protected set; } = 0;
        public bool isExistPropertiseFile { get { return ModIO.ExistsFile(PropertisePath, Data); } }

        public abstract void Load(Transform transform);
        public abstract void Clear();
        //public abstract void Create(string name,bool data = false);

        ~EnvironmentMod()
        {
            Clear();
        }
    }

    public interface  IEnvironmentPropertise 
    {


    }
}
