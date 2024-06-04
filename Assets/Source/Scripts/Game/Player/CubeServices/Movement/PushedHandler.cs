using LeadTools.StateMachine;
using Source.Scripts.Game.tateMachine;
using UnityEngine;

namespace CubeProject.PlayableCube.Movement
{
	public class PushedHandler
	{
		private readonly IStateChangeable<CubeStateMachine> _cubeStateChangeable;

		public PushedHandler(IStateChangeable<CubeStateMachine> cubeStateChangeable)
		{
			_cubeStateChangeable = cubeStateChangeable;
		}

		public void OnMoving(Vector3 direction)
		{
			
		}
	}
}