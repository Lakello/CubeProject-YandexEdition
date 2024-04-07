using CubeProject.Game;
using CubeProject.PlayableCube.Movement;
using CubeProject.SO;
using LeadTools.Extensions;
using LeadTools.StateMachine;
using Source.Scripts.Game.Level.Trigger;
using Source.Scripts.Game.tateMachine;
using UnityEngine;

namespace CubeProject.PlayableCube
{
	[RequireComponent(typeof(CubeDiedView))]
	[RequireComponent(typeof(ChargeHolder))]
	[RequireComponent(typeof(BoxCollider))]
	public class CubeComponent : MonoBehaviour
	{
		public BecameVisibleBehaviour BecameVisibleBehaviour { get; private set; }
		public CubeMoveService MoveService { get; private set; }
		public ChargeHolder ChargeHolder { get; private set; }
		public CubeFallService FallService { get; private set; }
		public IStateMachine<CubeStateMachine> StateMachine { get; private set; }
		public TriggerObserver TriggerObserver { get; private set; }
		public CubeDiedView DiedView { get; private set; }
		public CubeData Data { get; private set; }

		private void Awake()
		{
			BecameVisibleBehaviour = gameObject.GetComponentInChildrenElseThrow<BecameVisibleBehaviour>();
			ChargeHolder = gameObject.GetComponentElseThrow<ChargeHolder>();
			TriggerObserver = gameObject.GetComponentElseThrow<TriggerObserver>();
			DiedView = gameObject.GetComponentElseThrow<CubeDiedView>();
		}

		public void Init(IStateMachine<CubeStateMachine> stateMachine, CubeData data)
		{
			StateMachine ??= stateMachine;
			Data ??= data;
		}

		public void Init(CubeMoveService moveService, CubeFallService fallService)
		{
			MoveService ??= moveService;
			FallService ??= fallService;
		}
	}
}