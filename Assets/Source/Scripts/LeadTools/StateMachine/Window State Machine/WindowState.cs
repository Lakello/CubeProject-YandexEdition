using CubeProject.LeadTools.UI;

namespace LeadTools.StateMachine
{
	public abstract class WindowState : State<WindowStateMachine>
	{
		private AnchorGroupAnimator _animator;
		private Window _window;

		public void Init(Window window)
		{
			_window = window;
			_animator = new AnchorGroupAnimator(_window.Animations);
		}

		public override void Enter()
		{
			base.Enter();

			TryShow();
		}

		public override void Exit()
		{
			base.Exit();

			TryHide();
		}

		private void TryShow()
		{
			if (_window == null)
				return;
			
			_window.gameObject.SetActive(true);

			_animator.PlayAnimations(AnchorAnimatorState.Target);
		}

		private void TryHide()
		{
			if (_window == null)
				return;
			
			_animator.PlayAnimations(
				AnchorAnimatorState.Initial,
				() => _window.gameObject.SetActive(false));
		}
	}
}