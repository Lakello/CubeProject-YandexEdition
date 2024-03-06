using Cinemachine;
using CubeProject.PlayableCube.Movement;
using Reflex.Attributes;
using Source.Scripts.Game;
using Source.Scripts.Game.Level.Camera;
using UnityEngine;

namespace CubeProject.PlayableCube
{
	public class CubeFallService : MonoBehaviour
	{
		private readonly float _checkDistance = 0.1f;

		private GroundChecker _groundChecker;
		private CubeMoveService _moveService;
		private FallHandler _fallHandler;
		private TargetCameraHolder _targetCameraHolder;

		private bool IsGrounded => _groundChecker.IsGround(transform.position, _checkDistance, out _);

		[Inject]
		private void Inject(CinemachineVirtualCamera virtualCamera, Cube cube, MaskHolder maskHolder, TargetCameraHolder targetCameraHolder)
		{
			_groundChecker = new GroundChecker(maskHolder.GroundMask);
			_fallHandler = new FallHandler(this, cube, transform, _groundChecker, () => IsGrounded);
			_targetCameraHolder = targetCameraHolder;
			_fallHandler.AbyssFalling += _targetCameraHolder.ResetLookAt;
			
			_moveService = cube.ServiceHolder.MoveService;
			
			_moveService.StepEnded += OnStepEnded;
		}

		private void Start() =>
			TryFall();

		private void OnDisable()
		{
			_moveService.StepEnded -= OnStepEnded;
			_fallHandler.AbyssFalling -= _targetCameraHolder.ResetLookAt;
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