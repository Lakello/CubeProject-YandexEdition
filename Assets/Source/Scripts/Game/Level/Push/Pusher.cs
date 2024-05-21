using System;
using CubeProject.PlayableCube;
using CubeProject.PlayableCube.Movement;
using LeadTools.Extensions;
using Reflex.Attributes;
using Sirenix.OdinInspector;
using Source.Scripts.Game;
using Source.Scripts.Game.Messages;
using UniRx;
using UnityEngine;

namespace CubeProject.Tips
{
	public class Pusher : MonoBehaviour
	{
		[SerializeField] private bool _isAnyDirection;
		[SerializeField] [ShowIf(nameof(IsConcreteDirection))] private DirectionType _direction;

		private PushStateHandler _stateHandler;
		private IPushHandler _pushHandler;
		private Transform _cubeTransform;
		private LayerMask _groundMask;
		private CompositeDisposable _disposable;
		private Vector3 _currentDirection;

		public event Action Pushed;

		private bool IsConcreteDirection => _isAnyDirection is false;

		[Inject]
		private void Inject(CubeComponent cubeComponent, PushStateHandler stateHandler, MaskHolder holder)
		{
			Debug.Log($"{nameof(Inject)} {nameof(Pusher)}");
			_stateHandler = stateHandler;
			_cubeTransform = cubeComponent.transform;
			_groundMask = holder.GroundMask;
		}

		private void Awake()
		{
			gameObject.GetComponentElseThrow(out _pushHandler);

			MessageBroker.Default
				.Receive<Message<Vector3, CubeMoveService>>()
				.Where(message => message.Id == MessageId.DirectionChanged)
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
				.Receive<Message<CubeMoveService>>()
				.Where(message => message.Id == MessageId.StepEnded)
				.Subscribe(_ => OnStepEnded())
				.AddTo(_disposable);
			
			MessageBroker.Default
				.Publish(new PushAfterStepMessage(GetDirection()));
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
				{
					Debug.LogError($"Invalid direction", _cubeTransform.gameObject);
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