using System;

namespace CubeProject.Game.Messages
{
	public class DoAfterStepMessage
	{
		public readonly Action Action;

		public DoAfterStepMessage(Action action) =>
			Action = action;
	}
}