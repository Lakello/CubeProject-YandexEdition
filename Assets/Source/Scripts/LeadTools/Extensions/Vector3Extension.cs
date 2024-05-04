using UnityEngine;

namespace LeadTools.Extensions
{
	public static class Vector3Extension
	{
		public static Vector3 SetAll(this Vector3 origin, float value)
		{
			origin.Set(value, value, value);

			return origin;
		}
	}
}