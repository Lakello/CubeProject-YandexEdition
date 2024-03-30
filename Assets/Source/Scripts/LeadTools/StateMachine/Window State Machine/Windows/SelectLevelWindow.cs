using System;
using LeadTools.StateMachine.States;

namespace LeadTools.StateMachine.Windows
{
	public class SelectLevelWindow : Window
	{
		public override Type WindowType => typeof(SelectLevelWindowState);
	}
}