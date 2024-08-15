using System;
using Game.Player;
using UnityEngine;

namespace LeadTools.Extensions
{
	public static class TransformExtension
	{
		public static bool IsThereFreeSeat(
			this Transform originTransform,
			ref Vector3 direction,
			LayerMask layerMask,
			Action successCallback = null,
			int distance = 1)
		{
			if (distance < 1)
			{
				distance = 1;
			}

			var directions = new Directions();

			return Find(ref direction);

			bool Find(ref Vector3 direction)
			{
				var origin = originTransform.transform.position + direction * distance;

				if (Physics.Raycast(origin, Vector3.down, Mathf.Infinity, layerMask))
				{
					successCallback?.Invoke();

					return true;
				}
				else
				{
					directions.SetValue(direction, false);

					direction = directions.GetAnyDirection();

					if (direction == Vector3.zero)
					{
						return false;
					}

					return Find(ref direction);
				}
			}
		}

		public static void RotateToTarget(this Transform origin, Transform target)
		{
			var offset = target.transform.position - origin.position;
			offset.Set(offset.x, 0, offset.z);
			origin.transform.rotation = Quaternion.Euler(0f, Vector3.SignedAngle(Vector3.forward, offset, Vector3.up), 0f);
		}
	}
}