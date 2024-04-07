using UnityEngine;

namespace CubeProject.PlayableCube
{
	public class BecameVisibleService : MonoBehaviour
	{
		public bool IsVisible { get; private set; }

		private void OnBecameVisible() =>
			IsVisible = true;

		private void OnBecameInvisible() =>
			IsVisible = false;
	}
}