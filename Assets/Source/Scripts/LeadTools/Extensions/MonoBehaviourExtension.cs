using System;
using System.Collections;
using UnityEngine;

namespace LeadTools.Extensions
{
	public static class MonoBehaviourExtension
	{
		public static void StopRoutine(this MonoBehaviour context, Coroutine routine)
		{
			if (context == null)
				return;

			if (routine != null)
				context.StopCoroutine(routine);
		}

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
	}
}