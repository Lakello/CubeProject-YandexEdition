using CubeProject.Game;
using CubeProject.PlayableCube.Movement;
using CubeProject.SO;
using LeadTools.Extensions;
using LeadTools.StateMachine;
using Source.Scripts.Game.Level.Shield;
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
		public ChargeHolder ChargeHolder { get; private set; }
		public CubeFallService FallService { get; private set; }
		public IStateMachine<CubeStateMachine> StateMachine { get; private set; }
		public TriggerDetector TriggerDetector { get; private set; }
		public CubeDiedView DiedView { get; private set; }
		public CubeData Data { get; private set; }
		public CubeShieldService ShieldService { get; private set; }

		private void Awake()
		{
			BecameVisibleBehaviour = gameObject.GetComponentInChildrenElseThrow<BecameVisibleBehaviour>();
			TriggerDetector = gameObject.GetComponentInChildrenElseThrow<TriggerDetector>();
			ChargeHolder = gameObject.GetComponentElseThrow<ChargeHolder>();
			DiedView = gameObject.GetComponentElseThrow<CubeDiedView>();
		}

		public void Init(IStateMachine<CubeStateMachine> stateMachine, CubeData data)
		{
			StateMachine ??= stateMachine;
			Data ??= data;
		}

		public void Init(CubeFallService fallService, CubeShieldService shieldService)
		{
			FallService ??= fallService;
			ShieldService ??= shieldService;
		}
	}
}