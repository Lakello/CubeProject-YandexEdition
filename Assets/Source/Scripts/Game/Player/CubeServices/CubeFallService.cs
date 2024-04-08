using System;
using CubeProject.PlayableCube.Movement;
using Source.Scripts.Game;
using Source.Scripts.Game.Level.Camera;
using UnityEngine;

namespace CubeProject.PlayableCube
{
	public class CubeFallService : IDisposable
	{
		private const float CheckDistance = 0.1f;

		private readonly GroundChecker _groundChecker;
		private readonly CubeMoveService _moveService;
		private readonly FallHandler _fallHandler;
		private readonly TargetCameraHolder _targetCameraHolder;
		private readonly Transform _transform;

		private bool IsGrounded => _groundChecker.IsGround(_transform.position, CheckDistance, out _);

		public CubeFallService(
			CubeMoveService moveService,
			Cube cube,
			MaskHolder maskHolder,
			TargetCameraHolder targetCameraHolder,
			MonoBehaviour mono)
		{
			_transform = cube.transform;
			_groundChecker = new GroundChecker(maskHolder.GroundMask);
			_fallHandler = new FallHandler(mono, cube, _transform, _groundChecker, () => IsGrounded);
			_targetCameraHolder = targetCameraHolder;
			_fallHandler.AbyssFalling += _targetCameraHolder.ResetTarget;

			_moveService = moveService;

			_moveService.StepEnded += OnStepEnded;
		}

		public void Dispose()
		{
			_moveService.StepEnded -= OnStepEnded;
			_fallHandler.AbyssFalling -= _targetCameraHolder.ResetTarget;
		}

		public bool TryFall()
		{
			if (IsGrounded)
			{
				return false;
			}
			else
			{
				_fallHandler.Play();

				return true;
			}
		}

		private void OnStepEnded() =>
			TryFall();
	}
}