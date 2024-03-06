using System;
using CubeProject.PlayableCube;
using LeadTools.Extensions;
using UnityEngine;

namespace CubeProject.Game
{
	public class TeleportView
	{
		private readonly Cube _cube;
		private readonly MonoBehaviour _mono;
		private readonly Transform _origin;
		private readonly Transform _targetPoint;
		private readonly float _animationTime;

		public TeleportView(Cube cube, MonoBehaviour mono, Transform origin, Transform targetPoint, float animationTime)
		{
			_cube = cube;
			_mono = mono;
			_origin = origin;
			_targetPoint = targetPoint;
			_animationTime = animationTime;
		}
		
		private Vector3 OriginPosition => _origin.position;

		private Vector3 TargetPosition => _targetPoint.position;
		
		public void AnimationPlay(Func<float, float> getScaleValue, Func<float, float> getHeightValue, Action endCallback)
		{
			_mono.PlaySmoothChangeValue(
				(currentTime) =>
				{
					var scaleValue = getScaleValue(currentTime);
					_cube.transform.localScale = new Vector3(scaleValue, scaleValue, scaleValue);

					var heightValue = getHeightValue(currentTime);
					var position = Vector3.Lerp(OriginPosition, TargetPosition, heightValue);
					_cube.transform.position = position;
				},
				_animationTime,
				endCallback);
		}
	}
}