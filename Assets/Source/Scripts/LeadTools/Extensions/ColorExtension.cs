using UnityEngine;

namespace LeadTools.Extensions
{
	public static class ColorExtension
	{
		public static Color CalculateIntensityColor(this Color origin, float intensity)
		{
			const float powF = 2f;

			var factor = Mathf.Pow(powF, intensity);

			origin.r *= factor;
			origin.g *= factor;
			origin.b *= factor;

			return origin;
		}
	}
}