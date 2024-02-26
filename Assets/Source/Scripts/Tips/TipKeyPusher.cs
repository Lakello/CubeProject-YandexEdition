using System;
using CubeProject.Player;
using CubeProject.Player.Movement;
using LeadTools.Extensions;
using Reflex.Attributes;
using UnityEngine;

namespace CubeProject.Tips
{
	public class TipKeyPusher : MonoBehaviour
	{
		[SerializeField] private PushDirection _direction;

		private PusherStateHandler _stateHandler;
		private CubeMoveHandler _moveHandler;

		public event Action Pushed;

		[Inject]
		private void Inject(Cube cube, PusherStateHandler stateHandler)
		{
			_stateHandler = stateHandler;
			_moveHandler = cube.ComponentsHolder.MoveHandler;
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
			_moveHandler.DoAfterMove(() =>
			{
				_moveHandler.StepEnded += OnPushed;

				_moveHandler.Push(default(Vector3).GetDirectionFromEnum((int)_direction));
			});
		}

		private void OnPushed()
		{
			_moveHandler.StepEnded -= OnPushed;
			Pushed?.Invoke();
		}
	}
}