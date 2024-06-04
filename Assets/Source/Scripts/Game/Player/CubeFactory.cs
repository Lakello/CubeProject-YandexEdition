using System;
using System.Collections.Generic;
using CubeProject.InputSystem;
using CubeProject.PlayableCube;
using CubeProject.PlayableCube.Movement;
using CubeProject.SO;
using LeadTools.Extensions;
using LeadTools.StateMachine;
using Sirenix.Utilities;
using Source.Scripts.Game;
using Source.Scripts.Game.Level;
using Source.Scripts.Game.Level.Shield;
using Source.Scripts.Game.Level.Shield.States;
using Source.Scripts.Game.tateMachine;
using Source.Scripts.Game.tateMachine.States;
using UnityEngine;

namespace CubeProject
{
	public class CubeFactory : IDisposable
	{
		private readonly CubeData _data;
		private readonly SpawnPoint _spawnPoint;

		private IDisposable[] _disposables;

		public ShieldStateMachine ShieldStateMachine { get; private set; }
		public Player PlayerInstance { get; private set; }
		public Cube Cube { get; private set; }
		public IStateMachine<CubeStateMachine> CubeStateMachine { get; private set; }

		public CubeFactory(CubeData data, SpawnPoint spawnPoint)
		{
			_data = data;
			_spawnPoint = spawnPoint;

			InitStateMachine();
			InitShieldStateMachine();
			
			CubeStateMachine.EnterIn<ControlState>();
		}

		public void Create(IInputService inputService, MaskHolder maskHolder)
		{
			InitPlayerInstance();
			InitCube();
			InitCubeComponent();
			InitServices(inputService, maskHolder);
		}
		
		public void Dispose() =>
			_disposables?.ForEach(disposable => disposable.Dispose());

		private void InitPlayerInstance() =>
			PlayerInstance = UnityEngine.Object.Instantiate(
				_data.PlayerPrefab,
				_spawnPoint.transform.position,
				Quaternion.identity);

		private void InitCube() =>
			Cube = PlayerInstance.Cube;

		private void InitCubeComponent() =>
			Cube.Component.Init(CubeStateMachine, _data);

		private void InitServices(IInputService inputService, MaskHolder maskHolder)
		{
			var moveService = new CubeMoveService(
				Cube.Component.StateMachine,
				Cube.transform,
				inputService,
				maskHolder,
				Cube.Component.Data.RollSpeed,
				Cube.gameObject.GetComponentElseThrow<BoxCollider>());

			var fallService = new CubeFallService(Cube, maskHolder);
			var shieldService = new CubeShieldService(Cube, ShieldStateMachine);
			var diedService = new CubeDiedService(Cube, _spawnPoint);

			_disposables = new IDisposable[]
			{
				moveService, fallService, shieldService, diedService
			};
		}

		private void InitStateMachine() =>
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

		private void InitShieldStateMachine() =>
			ShieldStateMachine = new ShieldStateMachine(
				() => new Dictionary<Type, State<ShieldStateMachine>>
				{
					[typeof(PlayState)] = new PlayState(),
					[typeof(StopState)] = new StopState(),
				});
	}
}