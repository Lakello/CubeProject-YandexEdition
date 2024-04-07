using System;
using System.Collections.Generic;
using CubeProject.PlayableCube;
using LeadTools.StateMachine;
using Reflex.Attributes;
using Source.Scripts.Game.Level.Shield.States;
using UnityEngine;

namespace Source.Scripts.Game.Level.Shield
{
	public class ShieldView : MonoBehaviour
	{
		[SerializeField] private MeshRenderer _renderer;
		
		private Vector2 _distanceRange;
		private Vector2 _fresnelPowerRange;
		private Vector2 _displacementAmountRange;
		private IStateChangeable<ShieldStateMachine> _stateMachine;

		[Inject]
		private void Inject(Cube cube)
		{
			(_distanceRange, _fresnelPowerRange, _displacementAmountRange) = cube.Component.Data.GetShieldData;
			_stateMachine = cube.Component.ShieldService.StateMachine;

			_stateMachine.SubscribeTo<PlayState>(OnPlay);
			_stateMachine.SubscribeTo<StopState>(OnStop);
		}

		private void OnDisable()
		{
			_stateMachine.UnSubscribeTo<PlayState>(OnPlay);
			_stateMachine.UnSubscribeTo<StopState>(OnStop);
		}

		private void OnPlay(bool isEntered)
		{
			if (isEntered == false)
				return;
			
			
		}

		private void OnStop(bool isEntered)
		{
			
		}
	}
}