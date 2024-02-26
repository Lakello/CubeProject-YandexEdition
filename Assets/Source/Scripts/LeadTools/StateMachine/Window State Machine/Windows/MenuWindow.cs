using System;
using LeadTools.StateMachine.States;

namespace LeadTools.StateMachine.Windows
{
	public class MenuWindow : Window
	{
		public override Type WindowType => typeof(MenuWindowState);
	}
}