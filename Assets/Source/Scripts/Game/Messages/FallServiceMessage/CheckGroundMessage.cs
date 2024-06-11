using System;

namespace CubeProject.Game.Messages
{
	public class CheckGroundMessage
	{
		public readonly Action<bool> Callback;

		public CheckGroundMessage(Action<bool> callback) =>
			Callback = callback;
	}
}