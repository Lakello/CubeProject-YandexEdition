using System;
using System.Collections;
using CubeProject.PlayableCube;
using LeadTools.Extensions;
using UnityEngine;

namespace CubeProject.Game
{
	public class TeleportView
	{
		private readonly Cube _cube;
		private readonly Transform _origin;
		private readonly Transform _targetPoint;
		private readonly float _animationTime;

		public TeleportView(
			Cube cube,
			Transform origin,
			Transform targetPoint,
			float animationTime)
		{
			_cube = cube;
			_origin = origin;
			_targetPoint = targetPoint;
			_animationTime = animationTime;
		}

		private Vector3 OriginPosition => _origin.position;
		private Vector3 TargetPosition => _targetPoint.position;

		public IEnumerator AnimationPlay(Func<float, float> getScaleValue, Func<float, float> getHeightValue)
		{
			return MonoBehaviourExtension.SmoothChangeValue(
				(currentTime) =>
				{
					var scaleValue = getScaleValue(currentTime);
					_cube.transform.localScale = new Vector3(scaleValue, scaleValue, scaleValue);

					var heightValue = getHeightValue(currentTime);
					var position = Vector3.Lerp(OriginPosition, TargetPosition, heightValue);
					_cube.transform.position = position;
				},
				_animationTime);
		}
	}
}