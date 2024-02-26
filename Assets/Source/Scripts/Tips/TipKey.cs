using System;
using System.Collections.Generic;
using CubeProject.Tips.FSM;
using CubeProject.Tips.FSM.States;
using LeadTools.StateMachine;
using UnityEngine;

namespace CubeProject.Tips
{
	public class TipKey : MonoBehaviour
	{
		[SerializeField] private TipKeyData _data;
		[SerializeField] private MoveKeyAnimationSettings _upAnimationSettings;
		[SerializeField] private MoveKeyAnimationSettings _downAnimationSettings;

		private TipKeyStateMachine _stateMachine;

		public TipKeyData Data => _data;

		private void Awake()
		{
			var moveKeyAnimation = new MoveKeyAnimation(this);

			_upAnimationSettings.Init(transform);
			_downAnimationSettings.Init(transform);

			_stateMachine = new TipKeyStateMachine(() => new Dictionary<Type, State<TipKeyStateMachine>>
			{
				[typeof(PressState)] = new PressState(moveKeyAnimation, _downAnimationSettings),
				[typeof(ReleaseState)] = new ReleaseState(moveKeyAnimation, _upAnimationSettings),
			});

			_stateMachine.EnterIn<ReleaseState>();
		}

		public bool TryRelease()
		{
			if (_stateMachine.CurrentState.GetType() != typeof(ReleaseState))
			{
				_stateMachine.EnterIn<ReleaseState>();

				return true;
			}

			return false;
		}

		public bool TryPress()
		{
			if (_stateMachine.CurrentState.GetType() != typeof(PressState))
			{
				_stateMachine.EnterIn<PressState>();

				return true;
			}

			return false;
		}
	}
}