using System;
using UnityEngine;

namespace LeadTools.Object
{
	public class ObjectSpawner<TInstance, TInit>
		where TInstance : MonoBehaviour
	{
		private readonly IPoolingObject<TInstance, TInit> _defaultPoolingObject;
		private readonly ObjectFactory<TInstance, TInit> _factory;
		private readonly ObjectPool<TInstance, TInit> _pool;

		public ObjectSpawner(Transform container, IPoolingObject<TInstance, TInit> defaultPoolingObject)
		{
			_defaultPoolingObject = defaultPoolingObject;
			_factory = new ObjectFactory<TInstance, TInit>(container);
			_pool = new ObjectPool<TInstance, TInit>();
		}

		public IPoolingObject<TInstance, TInit> Spawn(
			TInit init,
			IPoolingObject<TInstance, TInit> poolingObject = null,
			Func<Vector3> getSpawnPosition = null)
		{
			if (poolingObject == null)
				poolingObject = _defaultPoolingObject;
			
			IPoolingObject<TInstance, TInit> spawningObject = GetObject(poolingObject);

			if (getSpawnPosition != null)
			{
				Vector3 position = getSpawnPosition();
				spawningObject.Instance.transform.position = position;
			}

			spawningObject.Disabled += _pool.Return;

			spawningObject.Init(init);

			return spawningObject;
		}

		private IPoolingObject<TInstance, TInit> GetObject(IPoolingObject<TInstance, TInit> poolingObject) =>
			_pool.TryGetObjectByType(poolingObject.SelfType) ?? CreateObject(poolingObject);

		private IPoolingObject<TInstance, TInit> CreateObject(IPoolingObject<TInstance, TInit> poolingObject) =>
			_factory.GetNewObject(poolingObject);
	}
}