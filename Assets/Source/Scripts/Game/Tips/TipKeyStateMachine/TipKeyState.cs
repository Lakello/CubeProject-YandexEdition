using System;
using LeadTools.StateMachine;

namespace CubeProject.Tips.FSM
{
	public abstract class TipKeyState : State<TipKeyStateMachine>
	{
		private readonly MoveKeyAnimation _animation;
		private readonly MoveKeyAnimationSettings _settings;

		protected TipKeyState(MoveKeyAnimation animation, MoveKeyAnimationSettings settings)
		{
			_animation = animation;
			_settings = settings;
		}

		public override void Enter()
		{
			base.Enter();
			_animation.Play(_settings);
		}

		public override void Exit()
		{
			base.Exit();
			_animation.Stop();
		}
	}
}