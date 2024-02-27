using System;
using CubeProject.PlayableCube;
using CubeProject.PlayableCube.Movement;
using CubeProject.Player;
using CubeProject.Player.Movement;
using LeadTools.Extensions;
using Source.Scripts.Game;
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
		private CubeStateHandler _cubeStateHandler;
		private CubeMoveService _cubeMoveService;
		private Transform _origin;
		private Transform _targetPoint;
		private MonoBehaviour _mono;
		private LayerMask _groundMask;
		
		private Vector3 OriginPosition => _origin.position;

		private Vector3 TargetPosition => _targetPoint.position;
		
		public void Init(MonoBehaviour mono, Cube cube, Transform origin, Transform targetPoint, MaskHolder maskHolder)
		{
			_groundMask = maskHolder.GroundMask;
			
			_mono = mono;
			_cube = cube;
			_cubeStateHandler = cube.ComponentsHolder.StateHandler;
			_cubeMoveService = cube.ComponentsHolder.MoveService;

			_origin = origin;
			_targetPoint = targetPoint;
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
				TryPushCube);

			return;

			void TryPushCube()
			{
				var direction = _cubeMoveService.CurrentDirection;
				
				if (_cube.IsThereFreeSeat(ref direction, Push, _groundMask) is false)
				{
					Debug.LogError($"Invalid direction", _cube.gameObject);
				}
				
				return;

				void Push()
				{
					_cubeStateHandler.EnterIn(CubeState.Pushing);

					_cubeMoveService.Push(direction);

					_cubeMoveService.DoAfterMove(() =>
					{
						if (_cube.ComponentsHolder.FallHandler.TryFall() is false)
						{
							_cubeStateHandler.EnterIn(CubeState.Normal);
						}
					});
				}
			}
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