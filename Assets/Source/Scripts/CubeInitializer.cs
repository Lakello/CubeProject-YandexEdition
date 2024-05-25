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
using Source.Scripts.Game.Level.Camera;
using Source.Scripts.Game.Level.Shield;
using Source.Scripts.Game.tateMachine;
using Source.Scripts.Game.tateMachine.States;
using UnityEngine;

namespace CubeProject
{
	public class CubeInitializer : MonoBehaviour
	{
		[SerializeField] private Player _playerPrefab;
		[SerializeField] private CubeData _data;

		private IDisposable[] _disposables;
		private bool _isInitialized;
		private IInputService _inputService;
		private MaskHolder _maskHolder;
		private CubeShieldService _shieldService;

		public SpawnPoint SpawnPoint { get; private set; }
		public Player PlayerInstance { get; private set; }
		public Cube Cube { get; private set; }
		public IStateMachine<CubeStateMachine> CubeStateMachine { get; private set; }

		private void OnDisable()
		{
			_disposables?.ForEach(disposable => disposable.Dispose());
		}

		public void Init(
			IInputService inputService,
			MaskHolder maskHolder,
			IStateMachine<ShieldStateMachine> shieldStateMachine)
		{
			if (_isInitialized)
				return;

			_inputService = inputService;
			_maskHolder = maskHolder;

			InitSpawnPoint();
			InitPlayerInstance();
			InitCube();
			InitStateMachine();
			InitCubeComponent();
			InitServices(shieldStateMachine);

			CubeStateMachine.EnterIn<ControlState>();

			_isInitialized = true;
		}

		private void InitPlayerInstance() =>
			PlayerInstance = Instantiate(
				_playerPrefab,
				SpawnPoint.transform.position,
				Quaternion.identity);

		private void InitSpawnPoint() =>
			SpawnPoint = FindObjectOfType<SpawnPoint>();

		private void InitCube() =>
			Cube = PlayerInstance.Cube;

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

		private void InitCubeComponent() =>
			Cube.Component.Init(CubeStateMachine, _data);

		private void InitServices(IStateMachine<ShieldStateMachine> shieldStateMachine)
		{
			var moveService = new CubeMoveService(
				Cube.Component.StateMachine,
				Cube.transform,
				_inputService,
				_maskHolder,
				Cube.Component.Data.RollSpeed,
				Cube.gameObject.GetComponentElseThrow<BoxCollider>(),
				this);

			var fallService = new CubeFallService(Cube, _maskHolder);
			var shieldService = new CubeShieldService(Cube, shieldStateMachine);
			var diedService = new CubeDiedService(Cube, SpawnPoint);

			_disposables = new IDisposable[]
			{
				moveService, fallService, shieldService, diedService
			};
		}
	}
}