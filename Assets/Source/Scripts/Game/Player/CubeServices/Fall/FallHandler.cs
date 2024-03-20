using System;
using System.Collections;
using LeadTools.Extensions;
using LeadTools.StateMachine;
using Source.Scripts.Game.tateMachine;
using Source.Scripts.Game.tateMachine.States;
using UnityEngine;

namespace CubeProject.PlayableCube
{
	public class FallHandler
	{
		private readonly MonoBehaviour _mono;
		private readonly IStateMachine<CubeStateMachine> _cubeStateMachine;
		private readonly Transform _origin;
		private readonly GroundChecker _groundChecker;
		private readonly Cube _cube;
		private readonly BecameVisibleService _became;
		private readonly float _speedFall = 3;
		private readonly float _speedFallMultiplier = 1.1f;
		private readonly Vector3 _offset = new Vector3(0, 0.5f, 0);
		private readonly Func<bool> _isGrounded;

		private Coroutine _fallCoroutine;

		public event Action AbyssFalling;
		
		public FallHandler(MonoBehaviour mono, Cube cube, Transform origin, GroundChecker groundChecker, Func<bool> isGrounded)
		{
			_mono = mono;
			_cube = cube;
			_became = _cube.ServiceHolder.BecameVisibleService;
			_cubeStateMachine = _cube.ServiceHolder.StateMachine;
			_origin = origin;
			_groundChecker = groundChecker;

			_isGrounded = isGrounded;
		}
        
		private Vector3 Position
		{
			get => _origin.position - _offset;
			set => _origin.position = _offset + value;
		}
		
		public void Play()
		{
			_mono.StopRoutine(_fallCoroutine);

			if (CanFallToGround(out var groundPositionY))
			{
				_cubeStateMachine.EnterIn<FallingToGroundState>();
				
				FallIntoGround(groundPositionY);
			}
			else
			{
				_cubeStateMachine.EnterIn<FallingToAbyssState>();
				
				FallIntoAbyss();
			}
		}

		private bool CanFallToGround(out float groundPositionY) =>
			_groundChecker.IsGround(_origin.position, Mathf.Infinity, out groundPositionY);
		
		private void FallIntoAbyss()
		{
			AbyssFalling?.Invoke();

			_fallCoroutine = _mono.StartCoroutine(Execute(
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
			_fallCoroutine = _mono.StartCoroutine(Execute(
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
				() => _isGrounded() is false && groundPositionY < Position.y));

			_mono.WaitRoutine(_fallCoroutine, () => _cubeStateMachine.EnterIn<ControlState>());
		}

		private IEnumerator Execute(Func<float, Vector3> calculatePosition, Func<bool> whileCondition, Action endCallback = null)
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