using System;
using CubeProject.Player;
using UnityEngine;

namespace CubeProject.Game
{
	public class UseObject : MonoBehaviour, IUsable
	{
		public event Action<Cube> TryUsing;

		public void TryUse(Cube cube) =>
			TryUsing?.Invoke(cube);
	}
}