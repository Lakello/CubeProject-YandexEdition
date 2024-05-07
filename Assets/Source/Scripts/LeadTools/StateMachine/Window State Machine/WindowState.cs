using System;
using CubeProject.LeadTools.UI;
using DG.Tweening;
using Sirenix.Utilities;
using UnityEngine;

namespace LeadTools.StateMachine
{
	public abstract class WindowState : State<WindowStateMachine>
	{
		private Window _window;
		private Sequence _sequence;

		public void Init(Window window) =>
			_window = window;

		public override void Enter()
		{
			base.Enter();

			_window.gameObject.SetActive(true);

			Debug.Log($"Enter = {_window.Animations != null}");
			
			if (_window.Animations != null)
				PlayAnimations(AnchorAnimatorState.Target);
		}

		public override void Exit()
		{
			base.Exit();

			if (_window.Animations != null)
				PlayAnimations(AnchorAnimatorState.Initial, HideWindow);
			else
				HideWindow();

			return;

			void HideWindow()
			{
				if (_window != null)
					_window.gameObject.SetActive(false);
			}
		}

		private void PlayAnimations(AnchorAnimatorState state, Action completeCallback = null)
		{
			_sequence?.Kill();

			_sequence = DOTween.Sequence().Pause().Append();

			_window.Animations
				.CreateAnimations(state)
				.ForEach(sequence => _sequence.Join(sequence));

			_sequence.OnKill(() => completeCallback?.Invoke());
			_sequence.Play();
		}
	}
}