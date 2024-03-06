using System;
using CubeProject.PlayableCube;
using CubeProject.PlayableCube.Movement;
using CubeProject.Tips;
using LeadTools.Extensions;
using LeadTools.NaughtyAttributes;
using Reflex.Attributes;
using Source.Scripts.Game;
using UnityEngine;

namespace CubeProject.Game
{
	[RequireComponent(typeof(ChargeConsumer))]
	public sealed class Portal : MonoBehaviour, IPushHandler
	{
		[SerializeField] private Transform _targetPoint;
		[SerializeField] private Portal _linkedPortal;
		[SerializeField] private TeleporterData _teleporterData;

		[ShowNonSerializedField] private bool _isActive;

		private bool _isBlocked;
		private Cube _cube;
		private CubeStateService _cubeStateService;
		private Teleporter _teleporter;
		private CubeMoveService _cubeMoveService;

		private ChargeConsumer _chargeConsumer;

		public event Action Pushing;

		[Inject]
		private void Inject(Cube cube, MaskHolder maskHolder)
		{
			_cube = cube;
			_cubeStateService = _cube.ServiceHolder.StateService;
			_cubeMoveService = _cube.ServiceHolder.MoveService;

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

			_cubeStateService.EnterIn(CubeState.Teleporting);

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