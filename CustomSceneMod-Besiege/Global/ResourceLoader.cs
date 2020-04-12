using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Modding;


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
        public void LoadEntityObject(GameObject gameObject,MeshPropertise meshPropertise,string path ,bool data = false, Transform parent = null, Action onLoad = null)
        {
            gameObject.layer = 29;
            gameObject.isStatic = true;
            gameObject.SetActive(true);
  
            onLoad += () => { Debug.Log("collider"); gameObject.AddComponent<MeshCollider>().sharedMesh = gameObject.GetComponent<MeshFilter>().mesh; };
            LoadVirtualObject(gameObject, meshPropertise, path, data, parent, onLoad);
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
        public void LoadVirtualObject(GameObject gameObject, MeshPropertise meshPropertise, string path, bool data = false, Transform parent = null, Action onLoad = null)
        {
            gameObject.name = meshPropertise.MeshName;
            gameObject.AddComponent<MeshFilter>();
            gameObject.AddComponent<MeshRenderer>();

            var meshPath = string.Format(@"{0}\{1}.obj", path, meshPropertise.MeshName);
            var texturePath = string.Format(@"{0}\{1}.png", path, meshPropertise.TextureName);

            if (ModIO.ExistsFile(meshPath, data))
            {
                var mesh = ModResource.CreateMeshResource(meshPropertise.MeshName, meshPath, data);
                mesh.OnLoad += onLoad;

                mesh.SetOnObject(gameObject);
                if (ModIO.ExistsFile(texturePath, data))
                {
                    var texture = ModResource.CreateTextureResource(meshPropertise.TextureName, texturePath, data);
                    texture.SetOnObject(gameObject);
                }
            }
            else
            {
                UnityEngine.Object.Destroy(gameObject);
            }
        }
    }
}
