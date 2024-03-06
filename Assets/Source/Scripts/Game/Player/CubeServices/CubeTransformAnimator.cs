using System;
using LeadTools.Extensions;
using UnityEngine;

namespace CubeProject.PlayableCube
{
	public class CubeTransformAnimator : MonoBehaviour
	{
		public void AnimateScale(AnimationCurve curve, float time, Action endCallback = null)
		{
			this.PlaySmoothChangeValue(
				(currentTime) =>
				{
					var scaleValue = curve.Evaluate(currentTime);

					var scale = new Vector3(scaleValue, scaleValue, scaleValue);

					transform.localScale = scale;
				},
				time,
				endCallback);
		}
	}
}