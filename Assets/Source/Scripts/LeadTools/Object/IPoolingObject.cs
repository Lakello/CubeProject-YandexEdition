using System;
using UnityEngine;

namespace LeadTools.Object
{
    public interface IPoolingObject<TInstance, TInit>
        where TInstance : MonoBehaviour
    {
        public event Action<IPoolingObject<TInstance, TInit>> Disabled;
        
        public Type SelfType { get; }

        public TInstance Instance { get; }

        public void Init(TInit init);
    }
}