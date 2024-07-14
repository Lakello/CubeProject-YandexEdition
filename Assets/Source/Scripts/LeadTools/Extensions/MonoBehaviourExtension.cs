using System;
using System.Collections;
using System.Threading;
using Cysharp.Threading.Tasks;
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

		public static async UniTask SmoothChangeValue(
			Action<float> lerp,
			float totalTime,
			CancellationToken cancellationTokenSource)
		{
			var currentTime = 0f;

			while (currentTime <= totalTime)
			{
				var normalTime = currentTime / totalTime;

				lerp(normalTime);

				currentTime += Time.fixedDeltaTime;

				await UniTask.WaitForFixedUpdate(cancellationTokenSource);
			}
		}
	}
}