using System;
using LeadTools.FSM.WindowFSM.States;

namespace LeadTools.FSM.WindowFSM.Windows
{
	public class PlayLevelWindow : Window
	{
		public override Type WindowType => typeof(PlayLevelWindowState);
	}
}