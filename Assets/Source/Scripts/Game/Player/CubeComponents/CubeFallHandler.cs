using System;
using System.Collections;
using Cinemachine;
using CubeProject.PlayableCube.Movement;
using CubeProject.PlayableCube;
using LeadTools.Extensions;
using Reflex.Attributes;
using Source.Scripts.Game;
using UnityEngine;

namespace CubeProject.PlayableCube
{
	public class CubeFallHandler : MonoBehaviour
	{
		[SerializeField] private float _delayStopCameraFollow = 0.3f;
		[SerializeField] private GroundChecker _groundChecker;
		[SerializeField] private float _checkDistance = 0.1f;
		[SerializeField] private float _speedFall;
		[SerializeField] private float _speedFallMultiplier = 1.1f;
		[SerializeField] private Vector3 _offset = new Vector3(0, 0.5f, 0);

		private Cube _cube;
		private CubeMoveService _moveService;
		private CubeStateHandler _stateHandler;
		private CubeBecameVisible _became;
		private Coroutine _fallCoroutine;
		private CinemachineVirtualCamera _virtualCamera;

		private bool IsGrounded => _groundChecker.IsGround(_checkDistance, out _);

		private Vector3 Position
		{
			get => transform.position - _offset;
			set => transform.position = _offset + value;
		}

		[Inject]
		private void Inject(CinemachineVirtualCamera virtualCamera, Cube cube, MaskHolder maskHolder)
		{
			_groundChecker.Init(maskHolder);
			
			_virtualCamera = virtualCamera;

			_cube = cube;
			_moveService = _cube.ComponentsHolder.MoveService;
			_stateHandler = _cube.ComponentsHolder.StateHandler;
			_became = _cube.ComponentsHolder.BecameVisible;

			_moveService.StepEnded += OnStepEnded;
		}

		private void Start() =>
			TryFall();

		private void OnDisable() =>
			_moveService.StepEnded -= OnStepEnded;

		public bool TryFall()
		{
			if (IsGrounded)
			{
				return false;
			}
			else
			{
				Fall();
				return true;
			}
		}

		private void OnStepEnded() =>
			TryFall();

		private bool CanFallToGround(out float groundPositionY) =>
			_groundChecker.IsGround(Mathf.Infinity, out groundPositionY);

		private void Fall()
		{
			_stateHandler.EnterIn(CubeState.Falling);

			this.StopRoutine(_fallCoroutine);

			if (CanFallToGround(out var groundPositionY))
			{
				FallIntoGround(groundPositionY);
			}
			else
			{
				FallIntoAbyss();
			}
		}

		private void FallIntoAbyss()
		{
			this.WaitTime(_delayStopCameraFollow, () =>
			{
				//_virtualCamera.Follow = null;
				_virtualCamera.LookAt = null;
			});

			_fallCoroutine = StartCoroutine(Fall(
				(delta) =>
				{
					var position = Position;
					position.y -= delta;

					return position;
				},
				() => _became.IsVisible,
				_cube.Kill));
		}

		private void FallIntoGround(float groundPositionY)
		{
			_fallCoroutine = StartCoroutine(Fall(
				(delta) =>
				{
					var position = Position;

					if (position.y - delta < groundPositionY)
					{
						position.y = groundPositionY;
					}
					else
					{
						position.y -= delta;
					}

					return position;
				},
				() => _groundChecker.IsGround(_checkDistance, out _) is false && groundPositionY < Position.y));

			this.WaitRoutine(_fallCoroutine, () => _stateHandler.EnterIn(CubeState.Normal));
		}

		private IEnumerator Fall(Func<float, Vector3> calculatePosition, Func<bool> whileCondition, Action endCallback = null)
		{
			var wait = new WaitForFixedUpdate();
			var speed = _speedFall;

			while (whileCondition())
			{
				var delta = (speed *= _speedFallMultiplier) * Time.fixedDeltaTime;

				Position = calculatePosition(delta);

				yield return wait;
			}

			endCallback?.Invoke();
		}
	}
}