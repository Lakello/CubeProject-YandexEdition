using System;
using LeadTools.StateMachine.States;

namespace LeadTools.StateMachine.Windows
{
	public class LeaderboardWindow : Window
	{
		public override Type WindowType => typeof(LeaderboardWindowState);
	}
}