using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class PoolMono<T> where T : PoolObject
    {
        public T Prefab { get; private set; }
        public Transform Container { get; private set; }

        protected List<T> _pool;
        public PoolMono(T prefab, int count, Transform container, int layer)
        {
            Prefab = prefab;
            Container = container;
            CreatePool(count, layer);
        }
        public PoolMono(T prefab, int count, Transform container)
        {
            Prefab = prefab;
            Container = container;
            CreatePool(count, prefab.gameObject.layer);
        }

        private void CreatePool(int count, int layer)
        {
            _pool = new List<T>();

            for (int i = 0; i < count; i++)
            {
                CreateObject().gameObject.layer = layer;
            }
        }

        private T CreateObject(bool isActiveAfterInstance = false)
        {
            var createdObject = GameObject.Instantiate<T>(Prefab, Container);
            createdObject.gameObject.SetActive(isActiveAfterInstance);
            _pool.Add(createdObject);
            return createdObject;
        }

        public bool TryGetObjectFromPool(out T poolObject, bool active)
        {
            foreach (var obj in _pool)
            {
                if (!obj.gameObject.activeInHierarchy)
                {
                    poolObject = obj;
                    poolObject.gameObject.SetActive(active);
                    return true;
                }
            }
            poolObject = null;
            return false;
        }

        public T GetObjectFromPool(bool active = true)
        {
            if (TryGetObjectFromPool(out var poolObject, active))
            {
                return poolObject;
            }
            else return CreateObject(active);
        }
    }
}