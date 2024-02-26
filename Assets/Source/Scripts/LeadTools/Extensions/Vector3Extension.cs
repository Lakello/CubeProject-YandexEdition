using UnityEngine;

namespace LeadTools.Extensions
{
	public static class Vector3Extension
	{
		public static Vector3 GetDirectionFromEnum(this Vector3 _, int enumDirection)
		{
			return enumDirection switch
			{
				0 => Vector3.left,
				1 => Vector3.right,
				2 => Vector3.forward,
				3 => Vector3.back,
				_ => Vector3.zero
			};
		}
	}
}