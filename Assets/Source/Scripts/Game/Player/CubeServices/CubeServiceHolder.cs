using CubeProject.Game;
using CubeProject.PlayableCube.Movement;
using LeadTools.Extensions;
using LeadTools.StateMachine;
using Source.Scripts.Game.tateMachine;
using UnityEngine;

namespace CubeProject.PlayableCube
{
	[RequireComponent(typeof(CubeDiedView))]
	[RequireComponent(typeof(CubeMoveService))]
	[RequireComponent(typeof(ChargeHolder))]
	[RequireComponent(typeof(BoxCollider))]
	[RequireComponent(typeof(CubeFallService))]
	public class CubeServiceHolder : MonoBehaviour
	{
		public BecameVisibleService BecameVisibleService { get; private set; }

		public CubeMoveService MoveService { get; private set; }

		public ChargeHolder ChargeHolder { get; private set; }

		public CubeFallService FallService { get; private set; }

		public IStateMachine<CubeStateMachine> StateMachine { get; private set; }

		private void Awake()
		{
			BecameVisibleService = gameObject.GetComponentInChildrenElseThrow<BecameVisibleService>();
			MoveService = gameObject.GetComponentElseThrow<CubeMoveService>();
			ChargeHolder = gameObject.GetComponentElseThrow<ChargeHolder>();
			FallService = gameObject.GetComponentElseThrow<CubeFallService>();
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