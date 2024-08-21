using System;
using LeadTools.FSM.WindowFSM.States;

namespace LeadTools.FSM.WindowFSM.Windows
{
	public class EndLevelWindow : Window
	{
		public override Type WindowType => typeof(EndLevelWindowState);
	}
}