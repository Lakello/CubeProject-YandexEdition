using CubeProject.Game;
using CubeProject.PlayableCube.Movement;
using CubeProject.Player;
using LeadTools.Extensions;
using UnityEngine;

namespace CubeProject.PlayableCube
{
	[RequireComponent(typeof(CubeDissolveAnimation))]
	[RequireComponent(typeof(CubeStateHandler))]
	[RequireComponent(typeof(CubeMoveService))]
	[RequireComponent(typeof(ChargeHolder))]
	[RequireComponent(typeof(BoxCollider))]
	[RequireComponent(typeof(CubeFallHandler))]
	public class CubeComponentsHolder : MonoBehaviour
	{
		public CubeBecameVisible BecameVisible { get; private set; }

		public CubeDissolveAnimation DissolveAnimation { get; private set; }

		public CubeStateHandler StateHandler { get; private set; }

		public CubeMoveService MoveService { get; private set; }

		public ChargeHolder ChargeHolder { get; private set; }

		public BoxCollider SelfCollider { get; private set; }

		public CubeFallHandler FallHandler { get; private set; }

		private void Awake()
		{
			BecameVisible = gameObject.GetComponentInChildrenElseThrow<CubeBecameVisible>();
			DissolveAnimation = gameObject.GetComponentElseThrow<CubeDissolveAnimation>();
			StateHandler = gameObject.GetComponentElseThrow<CubeStateHandler>();
			MoveService = gameObject.GetComponentElseThrow<CubeMoveService>();
			ChargeHolder = gameObject.GetComponentElseThrow<ChargeHolder>();
			SelfCollider = gameObject.GetComponentElseThrow<BoxCollider>();
			FallHandler = gameObject.GetComponentElseThrow<CubeFallHandler>();
		}
	}
}