using System;
using CubeProject.PlayableCube;
using CubeProject.PlayableCube.Movement;
using CubeProject.Tips;
using LeadTools.Extensions;
using LeadTools.StateMachine;
using Reflex.Attributes;
using Sirenix.OdinInspector;
using Source.Scripts.Game;
using Source.Scripts.Game.Messages;
using Source.Scripts.Game.tateMachine;
using Source.Scripts.Game.tateMachine.States;
using UniRx;
using UnityEditor;
using UnityEngine;

namespace CubeProject.Game
{
	[RequireComponent(typeof(ChargeConsumer))]
	public sealed class PortalBehaviour : MonoBehaviour, IPushHandler
	{
		[ShowInInspector] private bool _isActive;

		[SerializeField] private Transform _targetPoint;
		[SerializeField] private TeleporterData _teleporterData;
		[SerializeField] private PortalBehaviour _linkedPortal;

		private bool _isBlocked;
		private IStateMachine<CubeStateMachine> _cubeStateMachine;
		private Teleporter _teleporter;
		private ChargeConsumer _chargeConsumer;
		private CompositeDisposable _disposable;

		public event Action Pushing;

		public PortalBehaviour LinkedPortal => _linkedPortal;

		private bool CanLinkPortal =>
			_linkedPortal is not null
			&& _linkedPortal._linkedPortal is null;

		private void OnValidate()
		{
			if (_linkedPortal == this)
				_linkedPortal = null;
		}

		#if UNITY_EDITOR
		[Button] [ShowIf(nameof(CanLinkPortal))]
		private void LinkPortal()
		{
			_linkedPortal._linkedPortal = this;
			EditorUtility.SetDirty(this);
			EditorUtility.SetDirty(_linkedPortal);
		}
		#endif

		[Inject]
		private void Inject(CubeComponent cubeComponent, MaskHolder maskHolder)
		{
			_cubeStateMachine = cubeComponent.StateMachine;

			_teleporter = new Teleporter(
				cubeComponent.transform,
				transform,
				_targetPoint,
				_teleporterData);
		}

		private void Awake()
		{
			gameObject.GetComponentElseThrow(out _chargeConsumer);
			
			OnChargeChanged();
		}

		private void OnEnable() =>
			_chargeConsumer.ChargeChanged += OnChargeChanged;

		private void OnDisable()
		{
			_disposable?.Dispose();
			_chargeConsumer.ChargeChanged -= OnChargeChanged;
		}

		private void OnTriggerEnter(Collider other)
		{
			if (_isBlocked)
				return;

			if (_linkedPortal is null)
				return;

			if (other.TryGetComponent(out Cube _) is false)
				return;

			if (_isActive is false || _linkedPortal._isActive is false)
				return;

			_linkedPortal.Block();

			_cubeStateMachine.EnterIn<TeleportState>();

			MessageBroker.Default
				.Publish(new DoAfterStepMessage(() =>
				{
					_disposable?.Dispose();
					
					_disposable = new CompositeDisposable();
					Observable.FromCoroutine(_teleporter.Absorb)
						.SelectMany(_linkedPortal._teleporter.Return)
						.Subscribe(_ => OnTeleportEnded())
						.AddTo(_disposable);
				}));
		}

		private void OnTriggerExit(Collider other)
		{
			if (other.TryGetComponent(out Cube _))
				_isBlocked = false;
		}

		private void OnTeleportEnded()
		{
			MessageBroker.Default
				.Publish(new CheckGroundMessage(isFall =>
				{
					if (isFall == false)
						Pushing?.Invoke();
				}));
		}

		private void OnChargeChanged() =>
			_isActive = _chargeConsumer.IsCharged;

		private void Block() =>
			_isBlocked = true;
	}
}