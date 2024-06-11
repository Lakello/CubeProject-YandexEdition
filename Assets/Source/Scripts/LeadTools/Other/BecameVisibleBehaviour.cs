using UnityEngine;

namespace CubeProject.Game.Player
{
	public class BecameVisibleBehaviour : MonoBehaviour
	{
		public bool IsVisible { get; private set; }

		private void OnBecameVisible() =>
			IsVisible = true;

		private void OnBecameInvisible() =>
			IsVisible = false;
	}
}