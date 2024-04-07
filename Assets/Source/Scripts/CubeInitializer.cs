using System;
using System.Collections.Generic;
using CubeProject.InputSystem;
using CubeProject.PlayableCube;
using CubeProject.PlayableCube.Movement;
using CubeProject.SO;
using LeadTools.Extensions;
using LeadTools.StateMachine;
using Reflex.Attributes;
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
		private CubeFallService _fallService;

		public SpawnPoint SpawnPoint { get; private set; }
		public Player PlayerInstance { get; private set; }
		public Cube Cube { get; private set; }
		public IStateMachine<CubeStateMachine> CubeStateMachine { get; private set; }

		[Inject]
		private void Inject(IInputService inputService, MaskHolder maskHolder, TargetCameraHolder targetCameraHolder)
		{
			var moveService = new CubeMoveService(
				Cube.Component.StateMachine,
				Cube.transform,
				inputService,
				maskHolder,
				Cube.Component.Data.RollSpeed,
				Cube.gameObject.GetComponentElseThrow<BoxCollider>(),
				this);

			_fallService = new CubeFallService(moveService, Cube, maskHolder, targetCameraHolder, this);
			
			Cube.Component.Init(moveService, _fallService);
			
			_ = new CubeDiedService(Cube, SpawnPoint, targetCameraHolder);
			_ = new CubeShieldService(Cube, PlayerInstance.gameObject.GetComponentInChildrenElseThrow<ShieldView>());
		}

		private void Start()
		{
			_fallService.TryFall();
		}

		public void Init()
		{
			if (_isInitialized)
				return;
			
			SpawnPoint = FindObjectOfType<SpawnPoint>();
			
			PlayerInstance = Instantiate(
				_playerPrefab,
				SpawnPoint.transform.position,
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
			Cube.Component.Init(CubeStateMachine, _data);

			CubeStateMachine.EnterIn<ControlState>();

			_isInitialized = true;
		}
	}
}