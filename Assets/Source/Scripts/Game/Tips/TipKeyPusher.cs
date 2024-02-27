using System;
using CubeProject.PlayableCube.Movement;
using CubeProject.Player;
using LeadTools.Extensions;
using Reflex.Attributes;
using UnityEngine;

namespace CubeProject.Tips
{
	public class TipKeyPusher : MonoBehaviour
	{
		[SerializeField] private PushDirection _direction;

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

				_moveService.Push(default(Vector3).GetDirectionFromEnum((int)_direction));
			});
		}

		private void OnPushed()
		{
			_moveService.StepEnded -= OnPushed;
			Pushed?.Invoke();
		}
	}
}