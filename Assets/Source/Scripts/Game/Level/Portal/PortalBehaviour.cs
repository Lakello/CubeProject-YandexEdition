using System;
using CubeProject.PlayableCube;
using CubeProject.PlayableCube.Movement;
using CubeProject.Tips;
using LeadTools.Extensions;
using LeadTools.NaughtyAttributes;
using LeadTools.StateMachine;
using Reflex.Attributes;
using Source.Scripts.Game;
using Source.Scripts.Game.tateMachine;
using Source.Scripts.Game.tateMachine.States;
using UnityEditor;
using UnityEngine;

namespace CubeProject.Game
{
	[RequireComponent(typeof(ChargeConsumer))]
	public sealed class PortalBehaviour : MonoBehaviour, IPushHandler
	{
		[ShowNonSerializedField] private bool _isActive;
		
		[SerializeField] private Transform _targetPoint;
		[SerializeField] private TeleporterData _teleporterData;
		[SerializeField] private PortalBehaviour _linkedPortal;

		private bool _isBlocked;
		private Cube _cube;
		private IStateMachine<CubeStateMachine> _cubeStateMachine;
		private Teleporter _teleporter;
		private CubeMoveService _cubeMoveService;
		private ChargeConsumer _chargeConsumer;
		
		public event Action Pushing;

		private bool CanLinkPortal =>
			_linkedPortal is not null
			&& _linkedPortal._linkedPortal is null;

		private void OnValidate()
		{
			if (_linkedPortal == this)
			{
				_linkedPortal = null;
			}
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
		private void Inject(Cube cube, MaskHolder maskHolder)
		{
			_cube = cube;
			_cubeMoveService = _cube.ServiceHolder.MoveService;
			_cubeStateMachine = _cube.ServiceHolder.StateMachine;

			_teleporter = new Teleporter(this, cube, transform, _targetPoint, () => Pushing?.Invoke(), _teleporterData);
		}

		private void Awake() =>
			gameObject.GetComponentElseThrow(out _chargeConsumer);

		private void OnEnable() =>
			_chargeConsumer.ChargeChanged += OnChargeChanged;

		private void OnDisable() =>
			_chargeConsumer.ChargeChanged -= OnChargeChanged;

		private void OnTriggerEnter(Collider other)
		{
			if (_isBlocked)
			{
				return;
			}

			if (_linkedPortal is null)
			{
				return;
			}

			if (other.TryGetComponent(out Cube _) is false)
			{
				return;
			}

			if (_isActive is false || _linkedPortal._isActive is false)
			{
				return;
			}

			_linkedPortal.Block();

			_cubeStateMachine.EnterIn<TeleportState>();

			_cubeMoveService.DoAfterMove(
				() => _teleporter.Absorb(
					() => _linkedPortal._teleporter.Return()));
		}

		private void OnTriggerExit(Collider other)
		{
			if (other.TryGetComponent(out Cube _))
			{
				_isBlocked = false;
			}
		}

		private void OnChargeChanged() =>
			_isActive = _chargeConsumer.IsCharged;

		private void Block() =>
			_isBlocked = true;
	}
}