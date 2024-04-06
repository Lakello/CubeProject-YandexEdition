using CubeProject.Game;
using CubeProject.InputSystem;
using CubeProject.PlayableCube.Movement;
using CubeProject.SO;
using LeadTools.Extensions;
using LeadTools.StateMachine;
using Reflex.Attributes;
using Source.Scripts.Game;
using Source.Scripts.Game.Level.Trigger;
using Source.Scripts.Game.tateMachine;
using UnityEngine;

namespace CubeProject.PlayableCube
{
	[RequireComponent(typeof(CubeDiedView))]
	[RequireComponent(typeof(CubeMoveService))]
	[RequireComponent(typeof(ChargeHolder))]
	[RequireComponent(typeof(BoxCollider))]
	[RequireComponent(typeof(CubeFallService))]
	public class CubeComponent : MonoBehaviour
	{
		public BecameVisibleService BecameVisibleService { get; private set; }
		public CubeMoveService MoveService { get; private set; }
		public ChargeHolder ChargeHolder { get; private set; }
		public CubeFallService FallService { get; private set; }
		public IStateMachine<CubeStateMachine> StateMachine { get; private set; }
		public TriggerObserver TriggerObserver { get; private set; }

		private void Awake()
		{
			BecameVisibleService = gameObject.GetComponentInChildrenElseThrow<BecameVisibleService>();
			ChargeHolder = gameObject.GetComponentElseThrow<ChargeHolder>();
			FallService = gameObject.GetComponentElseThrow<CubeFallService>();
		}

		[Inject]
		private void Inject(IInputService inputService, MaskHolder maskHolder, CubeData cubeData)
		{
			MoveService = new CubeMoveService(
				StateMachine,
				transform,
				inputService,
				maskHolder,
				cubeData.RollSpeed,
				gameObject.GetComponentElseThrow<BoxCollider>(),
				this);
		}
		
		public void Init(IStateMachine<CubeStateMachine> stateMachine)
		{
			if (StateMachine != null)
			{
				return;
			}

			StateMachine = stateMachine;
		}
	}
}