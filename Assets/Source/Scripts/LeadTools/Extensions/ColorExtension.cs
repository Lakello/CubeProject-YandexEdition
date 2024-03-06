using UnityEngine;

namespace LeadTools.Extensions
{
	public static class ColorExtension
	{
		public static Color CalculateIntensityColor(this Color origin, float intensity)
		{
			var factor = GetFactor(intensity);

			origin.r *= factor;
			origin.g *= factor;
			origin.b *= factor;

			return origin;
		}

		private static float GetFactor(float intensity)
		{
			const float powF = 2f;

			return Mathf.Pow(powF, intensity);
		}
	}
}