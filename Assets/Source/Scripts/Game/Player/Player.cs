using CubeProject.PlayableCube;
using UnityEngine;

namespace Source.Scripts.Game
{
	public class Player : MonoBehaviour
	{
		[SerializeField] private Cube _cube;
		[SerializeField] private Transform _follower;

		public Cube Cube => _cube;

		public Transform Follower => _follower;
	}
}