using System;
using CubeProject.Game.Player.CubeService;
using CubeProject.Game.Player.CubeService.Messages;
using LeadTools.Common;
using LeadTools.Extensions;
using Reflex.Attributes;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;

namespace CubeProject.Game.Level.Push
{
	public class Pusher : MonoBehaviour
	{
		[SerializeField] private bool _isAnyDirection;
		[SerializeField] [HideIf(nameof(_isAnyDirection))] private DirectionType _direction;

		private PushStateHandler _stateHandler;
		private IPushHandler _pushHandler;
		private Transform _cubeTransform;
		private LayerMask _groundMask;
		private CompositeDisposable _disposable;
		private Vector3 _currentDirection;

		public event Action Pushed;

		[Inject]
		private void Inject(CubeComponent cubeComponent, PushStateHandler stateHandler, MaskHolder holder)
		{
			_stateHandler = stateHandler;
			_cubeTransform = cubeComponent.transform;
			_groundMask = holder.GroundMask;
		}

		private void Awake()
		{
			gameObject.GetComponentElseThrow(out _pushHandler);

			MessageBroker.Default
				.Receive<M_MoveDirectionChanged>()
				.Subscribe(message => _currentDirection = message.Data)
				.AddTo(this);
		}

		private void OnEnable() =>
			_pushHandler.Pushing += OnPushing;

		private void OnDisable()
		{
			_disposable?.Dispose();
			_pushHandler.Pushing -= OnPushing;
		}

		private void OnPushing()
		{
			_stateHandler.Pushing(this);
			Push();
		}

		private void Push()
		{
			_disposable = new CompositeDisposable();

			MessageBroker.Default
				.Publish(new M_PushAfterStep(() =>
				{
					MessageBroker.Default
						.Receive<M_StepEnded>()
						.Subscribe(_ => OnStepEnded())
						.AddTo(_disposable);

					return GetDirection();
				}));
		}

		private void OnStepEnded()
		{
			_disposable?.Dispose();
			Pushed?.Invoke();
		}

		private Vector3 GetDirection()
		{
			Vector3 direction;

			if (_isAnyDirection)
			{
				direction = _currentDirection;

				if (_cubeTransform.IsThereFreeSeat(ref direction, _groundMask) is false)
					throw new ArgumentException("Invalid direction");
			}
			else
			{
				direction = _direction.Value;
			}

			return direction;
		}
	}
}