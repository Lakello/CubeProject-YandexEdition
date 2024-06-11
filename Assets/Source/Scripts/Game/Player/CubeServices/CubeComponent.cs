using CubeProject.Game.Level.Trigger;
using CubeProject.Game.PlayerStateMachine;
using CubeProject.SO;
using LeadTools.Extensions;
using LeadTools.StateMachine;
using UnityEngine;

namespace CubeProject.Game.Player
{
	[RequireComponent(typeof(CubeDiedView))]
	[RequireComponent(typeof(ChargeHolder))]
	[RequireComponent(typeof(BoxCollider))]
	public class CubeComponent : MonoBehaviour
	{
		public BecameVisibleBehaviour BecameVisibleBehaviour { get; private set; }
		public ChargeHolder ChargeHolder { get; private set; }
		public IStateMachine<CubeStateMachine> StateMachine { get; private set; }
		public TriggerDetector TriggerDetector { get; private set; }
		public CubeDiedView DiedView { get; private set; }
		public CubeData Data { get; private set; }

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
	}
}