using System.Collections.Generic;
using UnityEngine;

namespace TomGustin.ObjectPooling
{
    public sealed class ObjectPooling : MonoBehaviour
    {
        private readonly Dictionary<GameObject, List<GameObject>> pooledObjects = new Dictionary<GameObject, List<GameObject>>();
        private readonly Dictionary<GameObject, GameObject> spawnedObjects = new Dictionary<GameObject, GameObject>();

        private static ObjectPooling _instance;

        private void Awake()
        {
            _instance = this;
        }

        public static void CreatePool(GameObject prefab, int initialSize)
        {
            if (_instance.pooledObjects.ContainsKey(prefab)) return;

            List<GameObject> newList = new List<GameObject>();

            for (int i = 0; i < initialSize; i++)
            {
                GameObject spawned = Instantiate(prefab, _instance.transform);
                spawned.SetActive(false);
                newList.Add(spawned);
            }

            _instance.pooledObjects.Add(prefab, newList);
        }

        public static GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent)
        {
            GameObject activePoolObject = null;

            if (_instance.pooledObjects.TryGetValue(prefab, out List<GameObject> value))
            {
                while (!activePoolObject && value.Count > 0)
                {
                    activePoolObject = value[0];
                    value.RemoveAt(0);
                } 

                if (activePoolObject)
                {
                    activePoolObject.SetActive(true);
                } else
                {
                    activePoolObject = Instantiate(prefab);
                }

                activePoolObject.transform.parent = parent;
                activePoolObject.transform.position = position;
                activePoolObject.transform.rotation = rotation;

                _instance.spawnedObjects.Add(activePoolObject, prefab);
            } else
            {
                CreatePool(prefab, 0);

                activePoolObject = Instantiate(prefab);
                activePoolObject.transform.parent = parent;
                activePoolObject.transform.position = position;
                activePoolObject.transform.rotation = rotation;

                _instance.spawnedObjects.Add(activePoolObject, prefab);
            }

            return activePoolObject;
        }

        public static void Unspawn(GameObject obj)
        {
            if (_instance.spawnedObjects.TryGetValue(obj, out GameObject value))
            {
                _instance.pooledObjects[value].Add(obj);
                _instance.spawnedObjects.Remove(obj);
                obj.transform.SetParent(_instance.transform);
                obj.SetActive(false);
            } else
            {
                Destroy(obj);
            }
        }
    }

    public static class ObjectPoolingExtend
    {
        public static void CreatePool(this GameObject prefab, int initialValue = 1)
        {
            ObjectPooling.CreatePool(prefab, initialValue);
        }

        public static GameObject Spawn(this GameObject prefab)
        {
            return ObjectPooling.Spawn(prefab, Vector3.zero, Quaternion.identity, null);
        }

        public static GameObject Spawn(this GameObject prefab, Vector3 position)
        {
            return ObjectPooling.Spawn(prefab, position, Quaternion.identity, null);
        }

        public static GameObject Spawn(this GameObject prefab, Vector3 position, Quaternion rotation)
        {
            return ObjectPooling.Spawn(prefab, position, rotation, null);
        }

        public static GameObject Spawn(this GameObject prefab, Vector3 position, Quaternion rotation, Transform parent)
        {
            return ObjectPooling.Spawn(prefab, position,rotation,parent);
        }

        public static void Unspawn(this GameObject obj)
        {
            ObjectPooling.Unspawn(obj);
        }
    }
}