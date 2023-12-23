using System.Collections;
using UnityEngine;

namespace Managers.Pool.Test
{
    public class PoolTestClient : MonoBehaviour
    {
        [SerializeField] private GameObject prefab;

        private Pool pool;

        private void Awake()
        {
            pool = PoolManager.Instance.CreatePool(prefab, 10);

            StartCoroutine(Spawn());
        }

        private IEnumerator Spawn()
        {
            var count = 10;
            while (count > 0)
            {
                yield return new WaitForSeconds(0.5f);
                count--;
                var poolable = PoolManager.Instance.Pop(prefab, transform);
                PoolManager.Instance.Push(poolable, 2.5f);
            }
        }

        private IEnumerator Push()
        {
            while (!pool.IsFull())
            {
                yield return new WaitForSeconds(0.1f);
            }

            // PoolManager.Instance.Pop()
        }
    }
}