using CubeProject.PlayableCube;
using CubeProject.PlayableCube.Movement;
using Reflex.Attributes;
using UnityEngine;

namespace CubeProject.Game
{
	public class PowerUnitView : MonoBehaviour
	{
		private CubeMoveService _moveService;
		
		[Inject]
		private void Inject(Cube cube)
		{
			_moveService = cube.ServiceHolder.MoveService;
		}
	}
}