using CubeProject.Game;
using CubeProject.Player.Movement;
using LeadTools.Extensions;
using UnityEngine;

namespace CubeProject.Player
{
	[RequireComponent(typeof(CubeDissolveAnimation))]
	[RequireComponent(typeof(CubeStateHandler))]
	[RequireComponent(typeof(CubeMoveHandler))]
	[RequireComponent(typeof(CubeTransformAnimator))]
	[RequireComponent(typeof(ChargeHolder))]
	[RequireComponent(typeof(BoxCollider))]
	public class CubeComponentsHolder : MonoBehaviour
	{
		private CubeBecameVisible _becameVisible;
		private CubeDissolveAnimation _dissolveAnimation;
		private CubeStateHandler _stateHandler;
		private CubeMoveHandler _moveHandler;
		private CubeTransformAnimator _transformAnimator;
		private ChargeHolder _chargeHolder;
		private BoxCollider _boxCollider;

		public CubeBecameVisible BecameVisible => _becameVisible;

		public CubeDissolveAnimation DissolveAnimation => _dissolveAnimation;

		public CubeStateHandler StateHandler => _stateHandler;

		public CubeMoveHandler MoveHandler => _moveHandler;

		public CubeTransformAnimator TransformAnimator => _transformAnimator;

		public ChargeHolder ChargeHolder => _chargeHolder;

		public BoxCollider SelfCollider => _boxCollider;

		private void Awake()
		{
			gameObject.GetComponentInChildrenElseThrow(out _becameVisible);
			gameObject.GetComponentElseThrow(out _dissolveAnimation);
			gameObject.GetComponentElseThrow(out _stateHandler);
			gameObject.GetComponentElseThrow(out _moveHandler);
			gameObject.GetComponentElseThrow(out _transformAnimator);
			gameObject.GetComponentElseThrow(out _chargeHolder);
			gameObject.GetComponentElseThrow(out _boxCollider);
		}
	}
}