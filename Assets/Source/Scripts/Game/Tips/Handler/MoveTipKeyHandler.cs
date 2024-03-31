using CubeProject.PlayableCube.Movement;
using CubeProject.PlayableCube;
using Reflex.Attributes;
using UnityEngine;

namespace CubeProject.Tips
{
	public class MoveTipKeyHandler : TipKeyHandler
	{
		private CubeMoveService _service;

		[Inject]
		protected void Inject(Cube cube)
		{
			_service = cube.ServiceHolder.MoveService;

			_service.StepStarted += OnStepStarted;
			_service.StepEnded += OnStepEnded;
		}

		private void OnDisable()
		{
			_service.StepStarted -= OnStepStarted;
			_service.StepEnded -= OnStepEnded;
		}

		private void OnStepStarted() =>
			HandleStep(_service.CurrentDirection);
		
		private void OnStepEnded() =>
			HandleStep(Vector3.zero);

		private void HandleStep(Vector3 direction)
		{
			var tipKeyDirection = TipKey.Data.Direction;

			if (direction == Vector3.zero || (direction.x != 0 && direction.z != 0) || direction != tipKeyDirection)
			{
				TryRelease();
			}
			else if (direction == tipKeyDirection)
			{
				TryPress();
			}
		}
	}
}