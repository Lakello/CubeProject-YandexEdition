using System;
using CubeProject.PlayableCube;
using CubeProject.PlayableCube.Movement;
using CubeProject.Player;
using Reflex.Attributes;
using Source.Scripts.Game;
using UnityEngine;

namespace CubeProject.Game
{
	public sealed class Portal : MonoBehaviour, IChargeable
	{
		[SerializeField] private Transform _targetPoint;
		[SerializeField] private Portal _linkedPortal;
		[SerializeField] private Teleporter _teleporter;

		private bool _isBlocked;
		private Cube _cube;
		private CubeStateHandler _cubeStateHandler;
		private CubeMoveService _cubeMoveService;

		public event Action ChargeChanged;

		public bool IsCharged { get; private set; }
		
		[Inject]
		private void Inject(Cube cube, MaskHolder maskHolder)
		{
			_cube = cube;
			_cubeStateHandler = _cube.ComponentsHolder.StateHandler;
			_cubeMoveService = _cube.ComponentsHolder.MoveService;

			_teleporter.Init(this, cube, transform, _targetPoint, maskHolder);
		}

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

			if (IsCharged is false || _linkedPortal.IsCharged is false)
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
		
		public void SetLinkedPortal(Portal portal)
		{
			if (portal == this
				|| portal == null
				|| portal == _linkedPortal)
			{
				return;
			}
			
			_linkedPortal = portal;
		}
		
		public void Active()
		{
			IsCharged = true;
			ChargeChanged?.Invoke();
		}

		public void DeActive()
		{
			IsCharged = false;
			ChargeChanged?.Invoke();
		}
        
		private void Block() =>
			_isBlocked = true;
	}
}