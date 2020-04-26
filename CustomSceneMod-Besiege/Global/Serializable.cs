using Modding.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Modding;
using Vector3 = Modding.Serialization.Vector3;

namespace CustomScene
{
    /// <summary>
    /// 序列化的类
    /// </summary>
   [Obsolete] public class Serializable
    {


        public class Shader
        {
            [RequireToValidate]
            public string name;
            [CanBeEmpty]
            public ShaderPropertise[] propertise;

            //public Shader(string name, params ShaderPropertise[] propertise)
            //{
            //    Name = name;
            //    Propertise = propertise;
            //}

            public override string ToString()
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("Shader Name:"+name+"\n");
                foreach (var pro in propertise)
                {
                    sb.Append("\t" + pro.ToString());
                }

                return sb.ToString();
            }

            public class ShaderPropertise
            {
                [RequireToValidate]
                public string Name;
                [RequireToValidate]
                public string Value;
                [RequireToValidate]
                public string DataType;

                public override string ToString()
                {
                    return string.Format("ShaderPropertise:[name:{0},value:{1},type:{2}]\n", Name, Value, DataType);
                }
            }

        }

        public class Texture: Element, ISerializable<ModTexture>
        {
            [RequireToValidate]
            public string name;

            public ModTexture ToClass()
            {
                return ModResource.CreateTextureResource(name, GeoTools.ScenePackPath, true, true);
            }

            public override string ToString()
            {
                return string.Format("Texture name:{0}\n", name);
            }
        }

        public class Material :ISerializable<UnityEngine.Material>
        {
            [CanBeEmpty]
            public Texture texture;
            [CanBeEmpty]
            public Color color;
            [RequireToValidate]
            public Shader shader;

            public UnityEngine.Material ToClass()
            {
                throw new NotImplementedException();
            }

            public override string ToString()
            {
                return string.Format("Material:\n\t{0}\n\t{1}\n\t{2}\n", texture.ToString(), color.ToString(), shader.ToString());
            }
        }

        public class Mesh : ISerializable<ModMesh>
        {
            [RequireToValidate]
            public string meshName;

            public ModMesh ToClass()
            {
                return ModResource.CreateMeshResource(meshName, GeoTools.ScenePackPath, true);
            }

            public override string ToString()
            {
                return string.Format("Mesh name:{0}\n", meshName);
            }
        }

        public class Renderer : ISerializable<UnityEngine.MeshRenderer>
        {
            [CanBeEmpty]
            public bool receiveShadows = true;
            [CanBeEmpty]
            public bool castShadows = true;
            [RequireToValidate]
            public Material material;
            public UnityEngine.MeshRenderer ToClass()
            {
                throw new NotImplementedException();
            }

            public override string ToString()
            {
                return string.Format("Renderer:\n\treceiveShadows:{0}\n\tcastShadows:{1}\n\t{2}\n", receiveShadows, castShadows, material);
            }
        }

        public class Collider : ISerializable<UnityEngine.MeshCollider>
        {
            [RequireToValidate]
            public Mesh mesh;
            [CanBeEmpty]
            public float dynamicFriction = 1;
            [CanBeEmpty]
            public float staticFriction = 1;
            [CanBeEmpty]
            public float bouncines = 0;
            [CanBeEmpty]
            public PhysicMaterialCombine frictionCombine = PhysicMaterialCombine.Average;
            [CanBeEmpty]
            public PhysicMaterialCombine bounceCombine = PhysicMaterialCombine.Average;

            public UnityEngine.MeshCollider ToClass()
            {
                throw new NotImplementedException();
            }
        }
        
        public class Transform
        {
            [CanBeEmpty]
            public Vector3 position = Vector3.zero;
            [CanBeEmpty]
            public Vector3 scale = Vector3.one;
            [CanBeEmpty]
            public Vector3 rotation = Vector3.zero;

            public override string ToString()
            {
                return string.Format("Transform:\n\tPosition:{0}\n\tScale:{1}\n\tRotation:{2}\n", position, scale, rotation);              
            }
        }

        public class Object:Transform
        {
            //public GameObject gameObject;

            //public Mesh meshFilter;
            //public Renderer meshRenderer;
            //public MeshCollider meshCollider;

        }
    }

    [Obsolete]interface ISerializable<T>
    {
        T ToClass(); 
    }
}
