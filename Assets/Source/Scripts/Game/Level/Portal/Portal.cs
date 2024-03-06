using System;
using CubeProject.PlayableCube;
using CubeProject.PlayableCube.Movement;
using CubeProject.Player;
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
		[SerializeField] private Teleporter _teleporter;

		[ShowNonSerializedField] private bool _isActive;

		private bool _isBlocked;
		private Cube _cube;
		private CubeStateHandler _cubeStateHandler;

		private CubeMoveService _cubeMoveService;

		private ChargeConsumer _chargeConsumer;

		public event Action Pushing;

		[Inject]
		private void Inject(Cube cube, MaskHolder maskHolder)
		{
			_cube = cube;
			_cubeStateHandler = _cube.ComponentsHolder.StateHandler;
			_cubeMoveService = _cube.ComponentsHolder.MoveService;

			_teleporter.Init(this, cube, transform, _targetPoint, () => Pushing?.Invoke());
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

			_cubeStateHandler.EnterIn(CubeState.Teleporting);

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