using System;
using CubeProject.Game;
using CubeProject.PlayableCube;
using LeadTools.Extensions;
using LeadTools.StateMachine;
using Reflex.Attributes;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Source.Scripts.Game.Level.Shield
{
	public class CubeShieldBehaviour : MonoBehaviour
	{
		[SerializeField] [MinMaxSlider(0, 10f)] private Vector2 _distanceRange;
		[SerializeField] [MinMaxSlider(0, 1f)] private Vector2 _fresnelPowerRange;
		[SerializeField] [MinMaxSlider(0.001f, 0.01f)] private Vector2 _displacementAmountRange;
		
		private ChargeHolder _chargeHolder;
		private IStateChangeable<GameStateMachine> _stateChangeable;
		private bool _isAcceptableState;
		private MeshRenderer _shieldMeshRenderer;
		private Coroutine _shieldCoroutine;
		
		[Inject]
		private void Inject(Cube cube, IStateChangeable<GameStateMachine> stateChangeable)
		{
			_chargeHolder = cube.ServiceHolder.ChargeHolder;

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
			throw new NotImplementedException();
		}

		private void TryChangeShieldState()
		{
			if (_chargeHolder.IsCharged && _isAcceptableState)
			{
				_shieldCoroutine = this.PlaySmoothChangeValueWhileCondition(
					() =>
					{
						
					},
					Vector3)
					
			}
		}
		
		private void SetListenStateChanged(bool isListen)
		{
			if (isListen)
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