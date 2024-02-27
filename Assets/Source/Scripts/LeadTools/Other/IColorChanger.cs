using UnityEngine;

namespace LeadTools.Other
{
	public interface IColorChanger
	{
		public float IntensityChargedColor { get; }

		public float IntensityDischargedColor { get; }

		public Color ChargedColor { get; }

		public Color DischargedColor { get; }

		public void ChangeColor();

		public void ChangeColor(bool isCharged);

		public void ChangeColor(Color intensityColor);
	}
}