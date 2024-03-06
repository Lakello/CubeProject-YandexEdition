using CubeProject.PlayableCube;
using UnityEngine;

namespace Source.Scripts.Game
{
	public class Player : MonoBehaviour
	{
		[SerializeField] private Cube _cube;

		public Cube Cube => _cube;
	}
}