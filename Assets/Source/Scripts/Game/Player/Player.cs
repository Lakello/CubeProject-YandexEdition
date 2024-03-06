using CubeProject.PlayableCube;
using UnityEngine;

namespace Source.Scripts.Game
{
	public class Player : MonoBehaviour
	{
		[SerializeField] private Cube _cube;
		[SerializeField] private Transform _follow;

		public Cube Cube => _cube;

		public Transform Follow => _follow;
	}
}