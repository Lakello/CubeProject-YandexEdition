using System;
using System.Collections;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace LeadTools.Extensions
{
	public static class Value
	{
		public static async UniTask SmoothChange(
			Action<float> lerp,
			float totalTime,
			CancellationToken cancellationTokenSource)
		{
			var currentTime = 0f;

			while (cancellationTokenSource.IsCancellationRequested == false && currentTime <= totalTime)
			{
				var normalTime = currentTime / totalTime;

				lerp(normalTime);

				currentTime += Time.fixedDeltaTime;

				await UniTask.WaitForFixedUpdate(cancellationTokenSource);
			}
		}
	}
}