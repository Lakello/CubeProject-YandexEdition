using CubeProject.Player;
using CubeProject.Player.Movement;
using Reflex.Attributes;
using UnityEngine;

namespace CubeProject.Game
{
	public class Teleport : ChargeConsumer
	{
		[SerializeField] private AnimationCurve _startAnimation;
		[SerializeField] private AnimationCurve _endAnimation;
		[SerializeField] private float _animationTime;
		[SerializeField] private Teleport _linkedTeleport;
		[SerializeField] private Vector3 _offset = new Vector3(0, 0.5f, 0);

		private bool _isActive = true;
		private Cube _cube;
		private CubeStateHandler _cubeStateHandler;
		private CubeMoveHandler _cubeMoveHandler;
		private CubeTransformAnimator _cubeTransformAnimator;

		private Vector3 TargetPosition => _linkedTeleport.transform.position + _offset;
		
		[Inject]
		private void Inject(Cube cube)
		{
			_cube = cube;
			_cubeStateHandler = _cube.ComponentsHolder.StateHandler;
			_cubeMoveHandler = _cube.ComponentsHolder.MoveHandler;
			_cubeTransformAnimator = _cube.ComponentsHolder.TransformAnimator;
		}

		private void Disable() =>
			_isActive = false;
		
		private void OnTriggerEnter(Collider other)
		{
			if (_isActive is false)
			{
				return;
			}

			if (_linkedTeleport is null)
			{
				return;
			}

			if (other.TryGetComponent(out Cube _) is false)
			{
				return;
			}

			if (IsCharged is false || _linkedTeleport.IsCharged is false)
			{
				return;
			}

			_linkedTeleport.Disable();

			_cubeStateHandler.EnterIn(CubeState.Teleporting);

			_cubeMoveHandler.DoAfterMove(Beam);
		}

		private void Beam()
		{
			StartBeam();
			
			return;

			void StartBeam() =>
				_cubeTransformAnimator.AnimateScale(_startAnimation, _animationTime, StopBeam);

			void StopBeam()
			{
				_cube.transform.position = TargetPosition;

				_cubeTransformAnimator.AnimateScale(
					_endAnimation,
					_animationTime,
					() => _cubeStateHandler.EnterIn(CubeState.Normal));
			}
		}
		
		private void OnTriggerExit(Collider other)
		{
			if (other.TryGetComponent(out Cube _))
			{
				_isActive = true;
			}
		}
	}
}