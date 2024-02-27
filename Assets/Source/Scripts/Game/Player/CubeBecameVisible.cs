using UnityEngine;

namespace CubeProject.Player
{
	public class CubeBecameVisible : MonoBehaviour
	{
		public bool IsVisible { get; private set; }

		private void OnBecameVisible() =>
			IsVisible = true;

		private void OnBecameInvisible() =>
			IsVisible = false;
	}
}