using UnityEngine;

namespace CubeProject.Game.Player
{
	public class GroundChecker
	{
		private const float CheckDistance = 0.1f;
		
		private readonly RaycastHit[] _results = new RaycastHit[1];
		private readonly Vector3 _extents = new Vector3(0.45f, 0.45f, 0.45f);
		private readonly LayerMask _groundMask;
		private readonly Transform _defaultTransform;

		public GroundChecker(LayerMask groundMask, Transform defaultTransform)
		{
			_groundMask = groundMask;
			_defaultTransform = defaultTransform;
		}

		public bool IsGrounded() =>
			IsGrounded(_defaultTransform.position, CheckDistance, out _);

		public bool IsGrounded(Vector3 center, float checkDistance, out float groundPositionY)
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
				groundPositionY = _results[0].transform.position.y;

			return count > 0;
		}
	}
}