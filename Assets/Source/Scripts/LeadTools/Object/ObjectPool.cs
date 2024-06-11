using System;
using System.Collections.Generic;
using UnityEngine;

namespace LeadTools.Object
{
	public class ObjectPool<TInstance, TInit>
		where TInstance : MonoBehaviour
	{
		private readonly Dictionary<Type, Queue<IPoolingObject<TInstance, TInit>>> _objects =
			new Dictionary<Type, Queue<IPoolingObject<TInstance, TInit>>>();

		public void Return(IPoolingObject<TInstance, TInit> poolingObject)
		{
			poolingObject.Disabled -= Return;

			Add(poolingObject);
		}

		public IPoolingObject<TInstance, TInit> TryGetObjectByType(Type objectType)
		{
			if (_objects.TryGetValue(objectType, out Queue<IPoolingObject<TInstance, TInit>> playersData))
			{
				if (playersData.Count > 0)
				{
					var data = playersData.Dequeue();

					data.Instance.gameObject.SetActive(true);

					return data;
				}
			}

			return null;
		}

		private void Add(IPoolingObject<TInstance, TInit> poolingObject)
		{
			poolingObject.Instance.gameObject.SetActive(false);

			if (_objects.TryGetValue(poolingObject.SelfType, out Queue<IPoolingObject<TInstance, TInit>> playersData))
				AddObject();
			else
				AddType();

			return;

			void AddType()
			{
				var queue = new Queue<IPoolingObject<TInstance, TInit>>();
				queue.Enqueue(poolingObject);

				_objects.Add(poolingObject.SelfType, queue);
			}

			void AddObject() =>
				playersData.Enqueue(poolingObject);
		}
	}
}