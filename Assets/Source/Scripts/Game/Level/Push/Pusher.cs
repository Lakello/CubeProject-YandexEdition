using System;
using CubeProject.PlayableCube;
using CubeProject.PlayableCube.Movement;
using LeadTools.Extensions;
using Reflex.Attributes;
using Sirenix.OdinInspector;
using Source.Scripts.Game;
using UnityEngine;

namespace CubeProject.Tips
{
	public class Pusher : MonoBehaviour
	{
		[SerializeField] private bool _isAnyDirection;
		[SerializeField] [ShowIf(nameof(IsConcreteDirection))] private DirectionType _direction;

		private PushStateHandler _stateHandler;
		private CubeMoveService _moveService;
		private IPushHandler _pushHandler;
		private Cube _cube;
		private LayerMask _groundMask;

		public event Action Pushed;

		private bool IsConcreteDirection => _isAnyDirection is false;

		[Inject]
		private void Inject(Cube cube, PushStateHandler stateHandler, MaskHolder holder)
		{
			_stateHandler = stateHandler;
			_cube = cube;
			_moveService = _cube.Component.MoveService;
			_groundMask = holder.GroundMask;
		}

		private void Awake() =>
			gameObject.GetComponentElseThrow(out _pushHandler);

		private void OnEnable() =>
			_pushHandler.Pushing += OnPushing;

		private void OnDisable() =>
			_pushHandler.Pushing -= OnPushing;

		private void OnPushing()
		{
			_stateHandler.Pushing(this);
			Push();
		}

		private void Push()
		{
			_moveService.DoAfterMove(() =>
			{
				_moveService.StepEnded += OnStepEnded;

				_moveService.Push(GetDirection());
			});
		}

		private void OnStepEnded()
		{
			_moveService.StepEnded -= OnStepEnded;
			Pushed?.Invoke();
		}

		private Vector3 GetDirection()
		{
			Vector3 direction;

			if (_isAnyDirection)
			{
				direction = _moveService.CurrentDirection;

				if (_cube.IsThereFreeSeat(ref direction, _groundMask) is false)
				{
					Debug.LogError($"Invalid direction", _cube.gameObject);
				}
			}
			else
			{
				direction = _direction.Value;
			}

			return direction;
		}
	}
}