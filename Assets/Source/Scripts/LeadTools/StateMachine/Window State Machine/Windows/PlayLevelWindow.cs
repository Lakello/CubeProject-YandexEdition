using System;
using LeadTools.StateMachine.States;

namespace LeadTools.StateMachine.Windows
{
	public class PlayLevelWindow : Window
	{
		public override Type WindowType => typeof(PlayLevelWindowState);
	}
}