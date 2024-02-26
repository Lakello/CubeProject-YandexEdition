using UnityEngine;

namespace LeadTools.Extensions
{
	public static class TransformExtension
	{
		public static void RotateToTarget(this Transform origin, Transform target)
		{
			var offset = target.transform.position - origin.position;
			offset.Set(offset.x, 0, offset.z);
			origin.transform.rotation = Quaternion.Euler(0f, Vector3.SignedAngle(Vector3.forward, offset, Vector3.up), 0f);
		}
	}
}