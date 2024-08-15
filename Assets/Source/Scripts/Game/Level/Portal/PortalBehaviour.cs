using System;
using CubeProject.Game.Messages;
using CubeProject.Game.PlayerStateMachine;
using CubeProject.Game.PlayerStateMachine.States;
using CubeProject.Tips;
using Cysharp.Threading.Tasks;
using LeadTools.Extensions;
using LeadTools.StateMachine;
using Reflex.Attributes;
using Sirenix.OdinInspector;
using UniRx;
using UnityEditor;
using UnityEngine;

namespace Game.Player
{
	[RequireComponent(typeof(ChargeConsumer))]
	public sealed class PortalBehaviour : MonoBehaviour, IPushHandler
	{
		private readonly M_CheckGround _checkGroundMessage = new M_CheckGround();
		
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

			if (other.TryGetComponent(out CubeEntity _) is false)
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

					ExecuteTeleport();
				}));
		}

		private void OnTriggerExit(Collider other)
		{
			if (other.TryGetComponent(out CubeEntity _))
				_isBlocked = false;
		}

		private async UniTaskVoid ExecuteTeleport()
		{
			var token = this.GetCancellationTokenOnDestroy();
			
			await _teleporter.Absorb(token);
			await _linkedPortal._teleporter.Return(token);
			
			OnTeleportEnded();
		}

		private void OnTeleportEnded()
		{
			MessageBroker.Default
				.Publish(_checkGroundMessage.SetData(isFall =>
				{
					if (isFall == false)
						_linkedPortal.Pushing?.Invoke();
				}));
		}

		private void OnChargeChanged() =>
			_isActive = _chargeConsumer.IsCharged;

		private void Block() =>
			_isBlocked = true;
	}
}