using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Modding;
using System.IO;

namespace CustomScene
{
    public interface IResourceLoader
    {
        ResourceLoader resourceLoader { get;}
    }

    public  class ResourceLoader:SingleInstance<ResourceLoader>
    {
        public override string Name { get; } = "Resource Loader";

        private void Awake()
        {
            transform.SetParent(Mod.ModObject.transform);
        }

        /// <summary>
        /// 加载带碰撞箱的物体
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="meshPropertise"></param>
        /// <param name="path">Object's Directory Path</param>
        /// <param name="data"></param>
        /// <param name="onLoad"></param>
        public void LoadEntityObject<T>(GameObject gameObject,T meshPropertise,string path ,bool data = false, Transform parent = null, Action<GameObject,T> onLoad = null) where T:MeshPropertise
        {
            gameObject.name = meshPropertise.MeshName;
            gameObject.AddComponent<MeshFilter>();
            gameObject.AddComponent<MeshRenderer>();
            gameObject.AddComponent<MeshCollider>();

            var meshPath = string.Format(@"{0}\{1}.obj", path, meshPropertise.MeshName);
            var texturePath = string.Format(@"{0}\{1}.png", path, meshPropertise.TextureName);

            if (ModIO.ExistsFile(meshPath, data))
            {
                var mesh = ModResource.CreateMeshResource(GeoTools.GetRandomString(), meshPath, data);
                mesh.OnLoad += () =>
                {
                    gameObject.GetComponent<MeshFilter>().mesh = mesh;
                    gameObject.GetComponent<MeshCollider>().sharedMesh = mesh;
                    onLoad?.Invoke(gameObject, meshPropertise);
                };

                if (ModIO.ExistsFile(texturePath, data))
                {
                    var texture = ModResource.CreateTextureResource(GeoTools.GetRandomString(), texturePath, data);
                    texture.OnLoad += () => { gameObject.GetComponent<MeshRenderer>().material.mainTexture = texture; };
                }

                if (parent != null)
                {
                    gameObject.transform.SetParent(parent);
                }
            }
            else
            {
                UnityEngine.Object.Destroy(gameObject);
            }

            onLoad?.Invoke(gameObject, meshPropertise);
        }
        /// <summary>
        /// 加载不带碰撞箱的物体
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="meshPropertise"></param>
        /// <param name="path">Object's Directory Path</param>
        /// <param name="data"></param>
        /// <param name="parent"></param>
        /// <param name="onLoad"></param>
        public void LoadVirtualObject<T>(GameObject gameObject, T meshPropertise, string path, bool data = false, Transform parent = null, Action<GameObject,T> onLoad = null) where T : MeshPropertise
        {
            gameObject.name = meshPropertise.MeshName;
            gameObject.AddComponent<MeshFilter>();
            gameObject.AddComponent<MeshRenderer>();

            var meshPath = string.Format(@"{0}\{1}.obj", path, meshPropertise.MeshName);
            var texturePath = string.Format(@"{0}\{1}.png", path, meshPropertise.TextureName);

            if (ModIO.ExistsFile(meshPath, data))
            {
                var mesh = ModResource.CreateMeshResource(GeoTools.GetRandomString(), meshPath, data);
                mesh.OnLoad += () => 
                {
                    gameObject.GetComponent<MeshFilter>().mesh = mesh; 
                    onLoad?.Invoke(gameObject, meshPropertise);
                };

                if (ModIO.ExistsFile(texturePath, data))
                {
                    var texture = ModResource.CreateTextureResource(GeoTools.GetRandomString(), texturePath, data);
                    texture.OnLoad += () => { gameObject.GetComponent<MeshRenderer>().material.mainTexture = texture; };
                }

                if (parent != null)
                {
                    gameObject.transform.SetParent(parent);
                }
            }
            else
            {
                UnityEngine.Object.Destroy(gameObject);
            }
        }
    }
}
