using System;

namespace Source.Scripts.Game.Messages
{
	public class CheckGroundMessage
	{
		public readonly Action<bool> Callback;

		public CheckGroundMessage(Action<bool> callback) =>
			Callback = callback;
	}
}