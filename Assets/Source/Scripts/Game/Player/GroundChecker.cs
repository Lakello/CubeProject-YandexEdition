using System;
using UnityEngine;

namespace CubeProject.PlayableCube
{
	[Serializable]
	public class GroundChecker
	{
		private readonly RaycastHit[] _results = new RaycastHit[1];
		private readonly Vector3 _extents = new Vector3(0.45f, 0.45f, 0.45f);
		private readonly LayerMask _groundMask;
		
		public GroundChecker(LayerMask groundMask) =>
			_groundMask = groundMask;

		public bool IsGround(Vector3 center, float checkDistance, out float groundPositionY)
		{
			groundPositionY = 0f;

			var count = Physics.BoxCastNonAlloc(
				center,
				_extents,
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