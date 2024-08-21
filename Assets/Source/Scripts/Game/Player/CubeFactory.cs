using System;
using CubeProject.Game.InputSystem;
using CubeProject.Game.Level.LevelPoint;
using CubeProject.Game.Level.LevelPoint.Messages;
using CubeProject.Game.Player.CubeService;
using CubeProject.Game.Player.CubeService.Shield;
using CubeProject.Game.Player.CubeService.Shield.States;
using CubeProject.Game.Player.FSM;
using CubeProject.Game.Player.FSM.States;
using CubeProject.SO;
using LeadTools.Extensions;
using LeadTools.FSM;
using Sirenix.Utilities;
using UniRx;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CubeProject.Game.Player
{
	public class CubeFactory : IDisposable
	{
		private readonly CubeData _data;
		private readonly SpawnPoint _spawnPoint;

		private CompositeDisposable _disposable;
		private IDisposable[] _disposables;

		public CubeFactory(CubeData data, SpawnPoint spawnPoint)
		{
			_data = data;
			_spawnPoint = spawnPoint;

			InitStateMachine();
			InitShieldStateMachine();

			CubeStateMachine.EnterIn<ControlState>();
		}

		public ShieldStateMachine ShieldStateMachine { get; private set; }

		public PlayerEntity PlayerEntityInstance { get; private set; }

		public CubeEntity CubeEntity { get; private set; }

		public IStateMachine<CubeStateMachine> CubeStateMachine { get; private set; }

		public void Create(IInputService inputService, MaskHolder maskHolder)
		{
			InitPlayerInstance();
			InitCube();
			InitCubeComponent();
			InitServices(inputService, maskHolder);

			_disposable = new CompositeDisposable();

			MessageBroker.Default
				.Receive<M_EndLevel>()
				.Subscribe(_ =>
				{
					CubeStateMachine.EnterIn<BlockControlState>();
				})
				.AddTo(_disposable);
		}

		public void Dispose()
		{
			Debug.Log("DIspose");
			_disposable?.Dispose();
			_disposables?.ForEach(disposable => disposable.Dispose());
		}

		private void InitPlayerInstance() =>
			PlayerEntityInstance = Object.Instantiate(
				_data.PlayerEntityPrefab,
				_spawnPoint.transform.position,
				Quaternion.identity);

		private void InitCube() =>
			CubeEntity = PlayerEntityInstance.CubeEntity;

		private void InitCubeComponent() =>
			CubeEntity.Component.Init(CubeStateMachine, _data);

		private void InitServices(IInputService inputService, MaskHolder maskHolder)
		{
			var moveService = new CubeMoveService(
				CubeEntity.Component.StateMachine,
				CubeEntity.transform,
				inputService,
				maskHolder,
				CubeEntity.Component.Data.RollSpeed,
				CubeEntity.gameObject.GetComponentElseThrow<BoxCollider>());

			var fallService = new CubeFallService(CubeEntity, maskHolder);
			var shieldService = new CubeShieldService(CubeEntity, ShieldStateMachine);
			var diedService = new CubeDiedService(CubeEntity, _spawnPoint);

			_disposables = new IDisposable[]
			{
				moveService, fallService, shieldService, diedService,
			};
		}

		private void InitStateMachine() =>
			CubeStateMachine = new CubeStateMachine()
				.AddState<ControlState>()
				.AddState<BlockControlState>()
				.AddState<DieState>()
				.AddState<FallingToAbyssState>()
				.AddState<FallingToGroundState>()
				.AddState<PushState>()
				.AddState<TeleportState>();

		private void InitShieldStateMachine() =>
			ShieldStateMachine = new ShieldStateMachine()
				.AddState<PlayState>()
				.AddState<StopState>();
	}
}