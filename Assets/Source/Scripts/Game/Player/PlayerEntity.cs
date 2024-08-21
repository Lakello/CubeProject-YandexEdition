using UnityEngine;

namespace CubeProject.Game.Player
{
	public class PlayerEntity : MonoBehaviour
	{
		[SerializeField] private CubeEntity _cubeEntity;
		[SerializeField] private Transform _follower;

		public CubeEntity CubeEntity => _cubeEntity;

		public Transform Follower => _follower;
	}
}