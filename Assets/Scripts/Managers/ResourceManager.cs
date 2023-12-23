using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class ResourceManager : Singleton<ResourceManager>
    {
        private const string PrefixExtern = "Extern/";
        private const string PrefixPrefabs = "Prefabs/";

        public GameObject Instantiate(string path, Transform parent = null)
        {
            var prefab = Resources.Load($"{PrefixPrefabs}{path}") as GameObject;

            if (prefab == null)
            {
                Debug.LogError($"Failed to Load Prefab: {path}");
                return null;
            }

            var go = Instantiate(prefab, parent);
            go.name = prefab.name;
            return go;
        }

        public List<GameObject> LoadAll(string path, bool recursive = true)
        {
            var gos = new List<GameObject>();


            return gos;
        }

        public T LoadExternResource<T>(string path) where T : Object
        {
            var resource = Resources.Load<T>($"{PrefixExtern}{path}");

            return resource;
        }

        public Sprite LoadSprite(string path)
        {
            return Resources.Load<Sprite>($"{PrefixExtern}{path}");
        }

        public void Destroy(GameObject go)
        {
            if (go == null)
            {
                return;
            }

            GameObject.Destroy(go);
        }
    }
}