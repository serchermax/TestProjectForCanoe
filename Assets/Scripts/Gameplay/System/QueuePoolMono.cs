using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class QueuePoolMono<T> where T : PoolObject
    {
        public T Prefab { get; private set; }
        public Transform Container { get; private set; }

        protected Queue<T> _pool;

        public QueuePoolMono(T prefab, int count, Transform container)
        {
            Prefab = prefab;
            Container = container;

            CreatePool(count);
        }

        private void CreatePool(int count)
        {
            _pool = new Queue<T>();

            for (int i = 0; i < count; i++)
            {
                CreateObject();
            }
        }

        private void CreateObject(bool isActiveAfterInstance = false)
        {
            var createdObject = GameObject.Instantiate<T>(Prefab, Container);
            createdObject.gameObject.SetActive(isActiveAfterInstance);
            _pool.Enqueue(createdObject);
        }

        public T GetObjectFromPool()
        {
            T poolObject = _pool.Dequeue();

            if (poolObject.gameObject.activeInHierarchy)
            {
                poolObject.StopTimer();
                poolObject.BackToPool();
            }
            poolObject.gameObject.SetActive(true);

            _pool.Enqueue(poolObject);
            return poolObject;
        }
    }
}
