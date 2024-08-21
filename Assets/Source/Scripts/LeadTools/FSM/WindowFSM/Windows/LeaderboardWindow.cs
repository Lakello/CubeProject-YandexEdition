using System;
using LeadTools.FSM.WindowFSM.States;

namespace LeadTools.FSM.WindowFSM.Windows
{
	public class LeaderboardWindow : Window
	{
		public override Type WindowType => typeof(LeaderboardWindowState);
	}
}