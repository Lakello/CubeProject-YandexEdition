using System;
using CubeProject.LeadTools.UI;
using DG.Tweening;
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

			Debug.Log($"Enter {_window.WindowType}");
			_window.gameObject.SetActive(true);
			
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
				Debug.Log($"Hide");
				
				if (_window != null)
					_window.gameObject.SetActive(false);
			}
		}

		private void PlayAnimations(AnchorAnimatorState state, Action completeCallback = null)
		{
			if (_sequence != null)
				_sequence.onKill = null;

			_sequence = DOTween.Sequence().Pause();

			var sequences = _window.Animations.CreateAnimations(state);

			if (sequences == null || sequences.Length < 1)
			{
				completeCallback?.Invoke();
				return;
			}
			
			_sequence.Append(sequences[0]);
			
			if (sequences.Length > 1)
			{
				for (int i = 1; i < sequences.Length; i++)
					_sequence.Join(sequences[i]);
			}

			_sequence.OnKill(() =>
			{
				Debug.Log("Kill");
				completeCallback?.Invoke();
			});
			_sequence.Play();
		}
	}
}