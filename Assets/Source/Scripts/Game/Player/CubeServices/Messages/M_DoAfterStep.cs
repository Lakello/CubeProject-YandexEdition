using System;

namespace CubeProject.Game.Player.CubeService.Messages
{
	public class M_DoAfterStep
	{
		public readonly Action Action;

		public M_DoAfterStep(Action action) =>
			Action = action;
	}
}