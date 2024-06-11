using System;
using UnityEngine;

namespace CubeProject.Game.Messages
{
	public class PushAfterStepMessage
	{
		public readonly Func<Vector3> GetDirection;

		public PushAfterStepMessage(Func<Vector3> getDirection) =>
			GetDirection = getDirection;
	}
}