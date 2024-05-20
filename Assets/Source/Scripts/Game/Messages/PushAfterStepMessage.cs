using UnityEngine;

namespace Source.Scripts.Game.Messages
{
	public class PushAfterStepMessage
	{
		public readonly Vector3 Direction;

		public PushAfterStepMessage(Vector3 direction) =>
			Direction = direction;
	}
}