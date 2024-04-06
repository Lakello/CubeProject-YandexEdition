using System;
using CubeProject.Game;
using CubeProject.PlayableCube;
using LeadTools.Extensions;
using LeadTools.StateMachine;
using Reflex.Attributes;
using Sirenix.OdinInspector;
using Source.Scripts.Game.tateMachine.States;
using UnityEngine;

namespace Source.Scripts.Game.Level.Shield
{
	public class CubeShieldBehaviour : MonoBehaviour
	{
		private readonly Type[] _acceptableStates = 
		{
			typeof(ControlState), typeof(PushState),
		};
		
		[SerializeField] [MinMaxSlider(0, 10f)] private Vector2 _distanceRange;
		[SerializeField] [MinMaxSlider(0, 1f)] private Vector2 _fresnelPowerRange;
		[SerializeField] [MinMaxSlider(0.001f, 0.01f)] private Vector2 _displacementAmountRange;

		private ChargeHolder _chargeHolder;
		private IStateChangeable<GameStateMachine> _stateChangeable;
		private MeshRenderer _shieldMeshRenderer;
		private Coroutine _shieldCoroutine;
		private bool _isAcceptableState;
		private bool _isListenStates;

		[Inject]
		private void Inject(Cube cube, IStateChangeable<GameStateMachine> stateChangeable)
		{
			_chargeHolder = cube.Component.ChargeHolder;

			_chargeHolder.ChargeChanged += OnChargeChanged;
		}

		private void Awake()
		{
			gameObject.GetComponentInChildrenElseThrow(out _shieldMeshRenderer);
		}

		private void OnDisable()
		{
			_chargeHolder.ChargeChanged -= OnChargeChanged;
			SetListenStateChanged(false);
		}

		private void OnChargeChanged()
		{
			SetListenStateChanged(_chargeHolder.IsCharged);
			
			TryChangeShieldState();
		}

		private void OnStateChanged()
		{
			foreach (var state in _acceptableStates)
			{
				if (_stateChangeable.CurrentState == state)
				{
					_isAcceptableState = true;
					return;
				}
			}

			_isAcceptableState = false;
		}

		private void TryChangeShieldState()
		{
			if (_chargeHolder.IsCharged && _isAcceptableState)
			{
				_shieldCoroutine = this.PlaySmoothChangeValueWhileCondition(
					() =>
					{
						
					},
					)
					
			}
		}
		
		private void SetListenStateChanged(bool isListen)
		{
			if (_isListenStates == isListen)
				return;

			_isListenStates = isListen;
			
			if (_isListenStates)
			{
				_stateChangeable.StateChanged += OnStateChanged;
			}
			else
			{
				_stateChangeable.StateChanged -= OnStateChanged;
			}
		}
	}
}