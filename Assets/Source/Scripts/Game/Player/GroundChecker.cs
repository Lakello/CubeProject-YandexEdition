using System;
using Source.Scripts.Game;
using UnityEngine;

namespace CubeProject.PlayableCube
{
	[Serializable]
	public class GroundChecker
	{
		private readonly RaycastHit[] _results = new RaycastHit[1];

		[SerializeField] private BoxCollider _collider;

		private LayerMask _groundMask;

		public void Init(MaskHolder maskHolder) =>
			_groundMask = maskHolder.GroundMask;

		public bool IsGround(float checkDistance, out float groundPositionY)
		{
			groundPositionY = 0f;

			var count = Physics.BoxCastNonAlloc(
				_collider.bounds.center,
				new Vector3(0.45f, 0.45f, 0.45f),
				Vector3.down,
				_results,
				Quaternion.identity,
				checkDistance,
				_groundMask);

			if (count > 0)
			{
				groundPositionY = _results[0].transform.position.y;
			}

			return count > 0;
		}
	}
}