using CubeProject.Game;
using CubeProject.PlayableCube.Movement;
using LeadTools.Extensions;
using LeadTools.StateMachine;
using Source.Scripts.Game.tateMachine;
using UnityEngine;

namespace CubeProject.PlayableCube
{
	[RequireComponent(typeof(CubeDiedView))]
	[RequireComponent(typeof(CubeStateService))]
	[RequireComponent(typeof(CubeMoveService))]
	[RequireComponent(typeof(ChargeHolder))]
	[RequireComponent(typeof(BoxCollider))]
	[RequireComponent(typeof(CubeFallService))]
	public class CubeServiceHolder : MonoBehaviour
	{
		public CubeBecameVisible BecameVisible { get; private set; }

		public CubeDiedView DiedView { get; private set; }

		public CubeMoveService MoveService { get; private set; }

		public ChargeHolder ChargeHolder { get; private set; }

		public BoxCollider SelfCollider { get; private set; }

		public CubeFallService FallService { get; private set; }

		public IStateChangeable<CubeStateMachine> StateMachine { get; private set; }

		private void Awake()
		{
			BecameVisible = gameObject.GetComponentInChildrenElseThrow<CubeBecameVisible>();
			DiedView = gameObject.GetComponentElseThrow<CubeDiedView>();
			MoveService = gameObject.GetComponentElseThrow<CubeMoveService>();
			ChargeHolder = gameObject.GetComponentElseThrow<ChargeHolder>();
			SelfCollider = gameObject.GetComponentElseThrow<BoxCollider>();
			FallService = gameObject.GetComponentElseThrow<CubeFallService>();
		}

		public void Init(IStateChangeable<CubeStateMachine> stateMachine)
		{
			if (StateMachine)
		}
	}
}