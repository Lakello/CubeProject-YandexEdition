using System;
using LeadTools.FSM.WindowFSM.States;

namespace LeadTools.FSM.WindowFSM.Windows
{
	public class MenuWindow : Window
	{
		public override Type WindowType => typeof(MenuWindowState);
	}
}