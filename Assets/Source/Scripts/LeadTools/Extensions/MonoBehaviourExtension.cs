using System;
using System.Collections;
using CubeProject.Game;
using UnityEngine;

namespace LeadTools.Extensions
{
	public static class MonoBehaviourExtension
	{
		public static bool IsThereFreeSeat(
			this MonoBehaviour context,
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
				var origin = context.transform.position + direction * distance;

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

		public static void StopRoutine(this MonoBehaviour context, Coroutine routine)
		{
			if (context == null)
			{
				return;
			}

			if (routine != null)
			{
				context.StopCoroutine(routine);
			}
		}

		public static Coroutine WaitTime(this MonoBehaviour context, float delay, Action endCallback) =>
			context.StartCoroutine(Wait(delay, endCallback));

		public static void WaitRoutine(this MonoBehaviour context, Coroutine routine, Action endCallback) =>
			context.StartCoroutine(Wait(routine, endCallback));

		public static Coroutine PlaySmoothChangeValue(
			this MonoBehaviour context,
			Action<float> lerp,
			float totalTime,
			Action endCallback = null,
			Action startCallback = null) =>
			context.StartCoroutine(SmoothChangeValue(lerp, totalTime, endCallback, startCallback));

		public static IEnumerator SmoothChangeValue(
			Action<float> lerp,
			float totalTime,
			Action endCallback = null,
			Action startCallback = null)
		{
			startCallback?.Invoke();

			var currentTime = 0f;

			while (currentTime <= totalTime)
			{
				var normalTime = currentTime / totalTime;

				lerp(normalTime);

				currentTime += Time.fixedDeltaTime;

				yield return new WaitForFixedUpdate();
			}

			endCallback?.Invoke();
		}
		
		public static Coroutine PlaySmoothChangeValueWhileCondition(
			this MonoBehaviour context,
			Action lerp,
			bool condition,
			Action endCallback = null,
			Action startCallback = null) =>
			context.StartCoroutine(SmoothChangeValue(lerp, condition, endCallback, startCallback));

		private static IEnumerator SmoothChangeValue(Action lerp, bool condition, Action endCallback, Action startCallback)
		{
			startCallback?.Invoke();

			while (condition)
			{
				lerp();

				yield return new WaitForFixedUpdate();
			}

			endCallback?.Invoke();
		}

		private static IEnumerator Wait(Coroutine routine, Action endCallback)
		{
			yield return routine;

			endCallback();
		}

		private static IEnumerator Wait(float delay, Action endCallback)
		{
			yield return new WaitForSeconds(delay);

			endCallback();
		}
	}
}