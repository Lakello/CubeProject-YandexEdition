using System;
using CubeProject.PlayableCube.Movement;
using CubeProject.Player;
using Reflex.Attributes;
using UnityEngine;

namespace CubeProject.Tips
{
	public class TipKeyPusher : MonoBehaviour
	{
		[SerializeField] private DirectionType _direction;

		private PusherStateHandler _stateHandler;
		private CubeMoveService _moveService;

		public event Action Pushed;

		[Inject]
		private void Inject(Cube cube, PusherStateHandler stateHandler)
		{
			_stateHandler = stateHandler;
			_moveService = cube.ComponentsHolder.MoveService;
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out Cube _))
			{
				_stateHandler.Pushing(this);
				Push();
			}
		}

		private void Push()
		{
			_moveService.DoAfterMove(() =>
			{
				_moveService.StepEnded += OnPushed;

				_moveService.Push(_direction.Value);
			});
		}

		private void OnPushed()
		{
			_moveService.StepEnded -= OnPushed;
			Pushed?.Invoke();
		}
	}
}