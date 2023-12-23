using System;
using System.Collections.Generic;
using System.Linq;
using Managers.Pool.PoolTask;
using UnityEngine;
using Utils;
using Object = UnityEngine.Object;

namespace Managers.Pool
{
    public class Pool
    {
        private readonly Stack<Poolable> poolStack = new();
        public Action<Pool> clearPoolAction;
        public int MaxCount { get; set; } = 10;
        public GameObject Original { get; private set; }
        public Transform Root { get; set; }

        public void Init(GameObject original, int count = 5)
        {
            Original = original;
            Root = new GameObject().transform;
            Root.name = $"{original.name}_Root";

            for (var i = 0; i < count; i++)
            {
                Push(Create());
            }
        }

        private Poolable Create()
        {
            var go = Object.Instantiate(Original);
            go.name = Original.name;

            var poolable = go.GetOrAddComponent<Poolable>();
            PoolManager.Instance.poolTask.SetInfo(WithdrawScheduleInfo.Of(poolable, this));

            return poolable;
        }

        public int GetStackSize()
        {
            return poolStack.Count;
        }

        public void Push(Poolable poolable)
        {
            if (poolable)
            {
                poolable.transform.parent = Root;
                poolable.gameObject.SetActive(false);
                poolable.isUsing = false;

                poolStack.Push(poolable);

                if (IsFull())
                {
                    clearPoolAction?.Invoke(this);
                }
            }
        }

        public Poolable Pop(Transform parent = null)
        {
            Poolable poolable;

            if (poolStack.Any())
            {
                poolable = poolStack.Pop();
            }
            else
            {
                poolable = Create();
            }

            poolable.transform.SetParent(parent);
            poolable.gameObject.SetActive(true);
            poolable.isUsing = true;

            return poolable;
        }

        public bool IsFull()
        {
            return poolStack.Count > MaxCount;
        }
    }
}