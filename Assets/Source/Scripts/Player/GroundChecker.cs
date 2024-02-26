using System;
using UnityEngine;

namespace CubeProject.Player
{
	[Serializable]
	public class GroundChecker
	{
		private readonly RaycastHit[] _results = new RaycastHit[1];

		[SerializeField] private LayerMask _groundMask;
		[SerializeField] private BoxCollider _collider;

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