using UnityEngine;

namespace LeadTools.Common
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