using System;
using System.Collections.Generic;
using CubeProject.InputSystem;
using CubeProject.PlayableCube;
using CubeProject.PlayableCube.Movement;
using CubeProject.SO;
using LeadTools.Extensions;
using LeadTools.StateMachine;
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

		private bool _isInitialized;
		private IInputService _inputService;
		private MaskHolder _maskHolder;
		private TargetCameraHolder _targetCameraHolder;
		private CubeMoveService _moveService;
		private CubeFallService _fallService;
		private CubeShieldService _shieldService;
		
		public SpawnPoint SpawnPoint { get; private set; }
		public Player PlayerInstance { get; private set; }
		public Cube Cube { get; private set; }
		public IStateMachine<CubeStateMachine> CubeStateMachine { get; private set; }
		
		private void Start()
		{
			_fallService.TryFall();
		}

		public void Init(IInputService inputService, MaskHolder maskHolder, TargetCameraHolder targetCameraHolder)
		{
			if (_isInitialized)
				return;

			_inputService = inputService;
			_maskHolder = maskHolder;
			_targetCameraHolder = targetCameraHolder;
			
			InitSpawnPoint();
			InitPlayerInstance();
			InitCube();
			InitStateMachine();
			InitCubeComponent();
			InitServices(targetCameraHolder);
			InitCubeComponentServices();
			
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

		private void InitCubeComponentServices() =>
			Cube.Component.Init(_moveService, _fallService, _shieldService);
		
		private void InitServices(TargetCameraHolder targetCameraHolder)
		{
			_moveService = new CubeMoveService(
				Cube.Component.StateMachine,
				Cube.transform,
				_inputService,
				_maskHolder,
				Cube.Component.Data.RollSpeed,
				Cube.gameObject.GetComponentElseThrow<BoxCollider>(),
				this);

			_fallService = new CubeFallService(_moveService, Cube, _maskHolder, _targetCameraHolder, this);
			_shieldService = new CubeShieldService(Cube);
			_ = new CubeDiedService(Cube, SpawnPoint, targetCameraHolder);
		}
	}
}