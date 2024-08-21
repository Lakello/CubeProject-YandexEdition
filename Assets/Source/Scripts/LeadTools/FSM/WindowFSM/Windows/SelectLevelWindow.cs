using System;
using LeadTools.FSM.WindowFSM.States;

namespace LeadTools.FSM.WindowFSM.Windows
{
	public class SelectLevelWindow : Window
	{
		public override Type WindowType => typeof(SelectLevelWindowState);
	}
}