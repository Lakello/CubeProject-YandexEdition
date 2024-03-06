using System;
using CubeProject.Player;
using LeadTools.Extensions;
using UnityEngine;

namespace CubeProject.Game
{
	[Serializable]
	public class Teleporter
	{
		[SerializeField] private AnimationCurve _scaleAnimation;
		[SerializeField] private AnimationCurve _heightAnimation;
		[SerializeField] private float _animationTime;

		private Cube _cube;
		private Transform _origin;
		private Transform _targetPoint;
		private MonoBehaviour _mono;
		private Action _callPushing;

		private Vector3 OriginPosition => _origin.position;

		private Vector3 TargetPosition => _targetPoint.position;

		public void Init(
			MonoBehaviour mono,
			Cube cube,
			Transform origin,
			Transform targetPoint,
			Action callPushing)
		{
			_mono = mono;
			_cube = cube;

			_origin = origin;
			_targetPoint = targetPoint;
			_callPushing = callPushing;
		}

		public void Absorb(Action endCallback)
		{
			Animation(
				(time) => _scaleAnimation.Evaluate(time),
				(time) => _heightAnimation.Evaluate(time),
				endCallback);
		}

		public void Return()
		{
			_cube.transform.position = TargetPosition;

			Animation(
				(time) => 1 - _scaleAnimation.Evaluate(time),
				(time) => 1 - _heightAnimation.Evaluate(time),
				_callPushing);
		}

		private void Animation(Func<float, float> getScaleValue, Func<float, float> getHeightValue, Action endCallback)
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