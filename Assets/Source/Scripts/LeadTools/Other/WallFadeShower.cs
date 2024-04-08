using Source.Scripts.Game.Level.Wall;
using UnityEngine;

namespace CubeProject.PlayableCube
{
	[RequireComponent(typeof(SphereCollider))]
	public class WallFadeShower : MonoBehaviour
	{
		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out WallFade wall))
			{
				wall.Show();
			}
		}

		private void OnTriggerExit(Collider other)
		{
			if (other.TryGetComponent(out WallFade wall))
			{
				wall.Hide();
			}
		}
	}
}