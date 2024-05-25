using UnityEngine;

namespace Source.Scripts.Game.Level.Shield
{
	public class ShieldData
	{
		public readonly Vector2 DistanceRange;
		public readonly Vector2 FresnelPowerRange;
		public readonly Vector2 DisplacementAmountRange;
		public readonly float DisplacementAmountHide;
		public readonly float HideShowDuration;

		public ShieldData(
			Vector2 distanceRange,
			Vector2 fresnelPowerRange,
			Vector2 displacementAmountRange,
			float displacementAmountHide,
			float hideShowDuration)
		{
			DistanceRange = distanceRange;
			FresnelPowerRange = fresnelPowerRange;
			DisplacementAmountRange = displacementAmountRange;
			DisplacementAmountHide = displacementAmountHide;
			HideShowDuration = hideShowDuration;
		}
	}
}