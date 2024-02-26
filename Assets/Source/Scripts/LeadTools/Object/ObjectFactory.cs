using LeadTools.Extensions;
using UnityEngine;

namespace LeadTools.Object
{
    public class ObjectFactory<TInstance, TInit>
        where TInstance : MonoBehaviour
    {
        private readonly Transform _parent;

        public ObjectFactory(Transform parent) =>
            _parent = parent;

        public IPoolingObject<TInstance, TInit> GetNewObject(IPoolingObject<TInstance, TInit> prefab) =>
            UnityEngine.Object.Instantiate(prefab.Instance.gameObject, _parent)
                .GetComponentElseThrow<IPoolingObject<TInstance, TInit>>();
    }
}
