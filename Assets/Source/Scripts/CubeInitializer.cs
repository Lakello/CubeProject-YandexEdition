using System;
using System.Collections.Generic;
using CubeProject.PlayableCube;
using LeadTools.StateMachine;
using Source.Scripts.Game;
using Source.Scripts.Game.Level;
using Source.Scripts.Game.tateMachine;
using Source.Scripts.Game.tateMachine.States;
using UnityEngine;

namespace CubeProject
{
	public class CubeInitializer : MonoBehaviour
	{
		[SerializeField] private Player _playerPrefab;

		private bool _isInitialized;

		public Player PlayerInstance { get; private set; }
		public Cube Cube { get; private set; }
		public IStateMachine<CubeStateMachine> CubeStateMachine { get; private set; }

		public void Init()
		{
			if (_isInitialized)
				return;
			
			var spawnPoint = FindObjectOfType<SpawnPoint>();
			
			PlayerInstance = Instantiate(
				_playerPrefab,
				spawnPoint.transform.position,
				Quaternion.identity);

			CubeStateMachine = new CubeStateMachine(
				() => new Dictionary<Type, State<CubeStateMachine>>
				{
					[typeof(ControlState)] = new ControlState(),
					[typeof(DieState)] = new DieState(),
					[typeof(FallingToAbyssState)] = new FallingToAbyssState(),
					[typeof(FallingToGroundState)] = new FallingToGroundState(),
					[typeof(PushState)] = new PushState(),
					[typeof(TeleportState)] = new TeleportState(),
				});

			Cube = PlayerInstance.Cube;
			Cube.Component.Init(CubeStateMachine);

			CubeStateMachine.EnterIn<ControlState>();
		}
	}
}