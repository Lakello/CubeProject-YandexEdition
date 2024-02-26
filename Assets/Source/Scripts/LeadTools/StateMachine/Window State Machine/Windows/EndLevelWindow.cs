using System;
using LeadTools.StateMachine.States;

namespace LeadTools.StateMachine.Windows
{
	public class EndLevelWindow : Window
	{
		public override Type WindowType => typeof(EndLevelWindowState);
	}
}