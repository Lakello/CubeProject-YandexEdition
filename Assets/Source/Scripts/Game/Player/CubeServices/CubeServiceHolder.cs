using CubeProject.Game;
using CubeProject.PlayableCube.Movement;
using CubeProject.PlayableCube;
using LeadTools.Extensions;
using UnityEngine;

namespace CubeProject.PlayableCube
{
	[RequireComponent(typeof(CubeDissolveAnimation))]
	[RequireComponent(typeof(CubeStateHandler))]
	[RequireComponent(typeof(CubeMoveService))]
	[RequireComponent(typeof(ChargeHolder))]
	[RequireComponent(typeof(BoxCollider))]
	[RequireComponent(typeof(CubeFallService))]
	public class CubeServiceHolder : MonoBehaviour
	{
		public CubeBecameVisible BecameVisible { get; private set; }

		public CubeDissolveAnimation DissolveAnimation { get; private set; }

		public CubeStateHandler StateHandler { get; private set; }

		public CubeMoveService MoveService { get; private set; }

		public ChargeHolder ChargeHolder { get; private set; }

		public BoxCollider SelfCollider { get; private set; }

		public CubeFallService FallService { get; private set; }

		private void Awake()
		{
			BecameVisible = gameObject.GetComponentInChildrenElseThrow<CubeBecameVisible>();
			DissolveAnimation = gameObject.GetComponentElseThrow<CubeDissolveAnimation>();
			StateHandler = gameObject.GetComponentElseThrow<CubeStateHandler>();
			MoveService = gameObject.GetComponentElseThrow<CubeMoveService>();
			ChargeHolder = gameObject.GetComponentElseThrow<ChargeHolder>();
			SelfCollider = gameObject.GetComponentElseThrow<BoxCollider>();
			FallService = gameObject.GetComponentElseThrow<CubeFallService>();
		}
	}
}