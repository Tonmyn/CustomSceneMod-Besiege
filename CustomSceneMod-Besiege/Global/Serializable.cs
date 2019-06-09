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
    public class Serializable
    {


        public class Shader : ISerializable<Shader>
        {
            [CanBeEmpty]
            public string name = "Shader name";
            [CanBeEmpty]
            public ShaderPropertise[] propertise = new ShaderPropertise[] { new ShaderPropertise() { Name = "Propertise", Value = "Value", DataType = "Data type" } };

            public Shader ToClass()
            {
                throw new NotImplementedException();
            }

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
            public string name = "Texture name";

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
            public Texture texture = new Texture();
            [CanBeEmpty]
            public Color color = Color.white;
            [RequireToValidate]
            public Shader shader = new Shader();

            public UnityEngine.Material ToClass()
            {
                throw new NotImplementedException();
            }

            public override string ToString()
            {
                return string.Format("Material:\n\t{0}\t{1}\n\t{2}", texture.ToString(), color.ToString(), shader.ToString());
            }
        }

        public class Mesh : ISerializable<ModMesh>
        {
            [RequireToValidate]
            public string meshName = "Mesh name";

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
            public Material material = new Material();
          

            public MeshRenderer ToClass()
            {
                throw new NotImplementedException();
            }

            public override string ToString()
            {
                return string.Format("Renderer:\n\treceiveShadows:{0}\n\tcastShadows:{1}\n\t{2}", receiveShadows, castShadows, material);
            }
        }

        public class Collider : ISerializable<UnityEngine.MeshCollider>
        {
            //[RequireToValidate]
            //public Mesh mesh = new Mesh();
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


            public MeshCollider ToClass()
            {
                throw new NotImplementedException();
            }

            public override string ToString()
            {
                return string.Format("Collider:\n\tDynamicFriction:{0}\n\tStaticFriction:{1}\n\tBouncines:{2}\n\tFrictionCombine:{3}\n\tBounceCombine:{4}\n", dynamicFriction, staticFriction, bouncines, frictionCombine, bounceCombine);
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

        public class Object:Transform,ISerializable<GameObject>
        {
            [RequireToValidate]
            public string name = "Object name";

            [RequireToValidate]
            public Mesh mesh = new Mesh();
            [RequireToValidate]
            public Renderer renderer = new Renderer();
            [CanBeEmpty]
            public Collider collider = new Collider();

            public GameObject ToClass()
            {
                var gameObject = new GameObject(name);

                gameObject.transform.position = position;
                gameObject.transform.rotation = Quaternion.Euler(rotation);
                gameObject.transform.localScale = scale;

                gameObject.AddComponent<MeshFilter>().mesh = mesh.ToClass().Mesh;

                var mr = gameObject.AddComponent<MeshRenderer>();
                mr.material = renderer.material.ToClass();
                mr.receiveShadows = renderer.receiveShadows;
                mr.shadowCastingMode = renderer.castShadows ? UnityEngine.Rendering.ShadowCastingMode.On : UnityEngine.Rendering.ShadowCastingMode.Off;

                var mc = gameObject.AddComponent<MeshCollider>();
                mc.sharedMesh = mesh.ToClass().Mesh;
                mc.material.staticFriction = collider.staticFriction;
                mc.material.dynamicFriction = collider.dynamicFriction;
                mc.material.bounciness = collider.bouncines;
                mc.material.frictionCombine = collider.frictionCombine;
                mc.material.bounceCombine = collider.bounceCombine;

                return gameObject;
            }
            public GameObject ToClass(bool hasCollider = true)
            {
                var gameObject = new GameObject(name);

                gameObject.AddComponent<MeshFilter>().mesh = mesh.ToClass().Mesh;

                var mr = gameObject.AddComponent<MeshRenderer>();
                mr.material = renderer.material.ToClass();
                mr.receiveShadows = renderer.receiveShadows;
                mr.shadowCastingMode = renderer.castShadows ? UnityEngine.Rendering.ShadowCastingMode.On : UnityEngine.Rendering.ShadowCastingMode.Off;

                if (hasCollider == false) { return gameObject; }

                var mc = gameObject.AddComponent<MeshCollider>();
                mc.sharedMesh = mesh.ToClass().Mesh;
                mc.material.staticFriction = collider.staticFriction;
                mc.material.dynamicFriction = collider.dynamicFriction;
                mc.material.bounciness = collider.bouncines;
                mc.material.frictionCombine = collider.frictionCombine;
                mc.material.bounceCombine = collider.bounceCombine;

                return gameObject;
            }

            public override string ToString()
            {
                return "Object: " + name + "\n" + base.ToString() + mesh.ToString() + renderer.ToString() + collider.ToString();
            }
        }
    }

    interface ISerializable<T>
    {
        T ToClass(); 
    }


   
}
