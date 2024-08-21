using System;
using UnityEngine;

namespace CubeProject.Game.Player.CubeService.Messages
{
	public class M_PushAfterStep
	{
		public readonly Func<Vector3> GetDirection;

		public M_PushAfterStep(Func<Vector3> getDirection) =>
			GetDirection = getDirection;
	}
}