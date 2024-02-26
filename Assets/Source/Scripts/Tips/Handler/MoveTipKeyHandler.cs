using CubeProject.Player;
using CubeProject.Player.Movement;
using LeadTools.Extensions;
using Reflex.Attributes;
using UnityEngine;

namespace CubeProject.Tips
{
	public class MoveTipKeyHandler : TipKeyHandler
	{
		private CubeMoveHandler _handler;

		[Inject]
		protected void Inject(Cube cube)
		{
			_handler = cube.ComponentsHolder.MoveHandler;

			_handler.StepStarted += OnStepStarted;
			_handler.StepEnded += OnStepEnded;
		}

		private void OnDisable()
		{
			_handler.StepStarted -= OnStepStarted;
			_handler.StepEnded -= OnStepEnded;
		}

		private void OnStepEnded() =>
			OnStepStarted(Vector3.zero);

		private void OnStepStarted(Vector3 direction)
		{
			var tipKeyDirection = default(Vector3).GetDirectionFromEnum((int)TipKey.Data.Type);

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