using System;
using LeadTools.Extensions;
using UnityEngine;

namespace LeadTools.Other
{
	[Serializable]
	public class HDRColorChanger
	{
		[SerializeField] private ShaderProperty _propertyName;
		[SerializeField] [Range(-10f, 10f)] private float _intensityChargedColor = 4;
		[SerializeField] [Range(-10f, 10f)] private float _intensityDischargedColor = 1;
		[SerializeField] private Color _chargedColor;
		[SerializeField] private Color _dischargedColor;
		[SerializeField] private MeshRenderer[] _bloomMeshRenderers;

		private Func<bool> _isChanged;

		public float IntensityChargedColor => _intensityChargedColor;

		public float IntensityDischargedColor => _intensityDischargedColor;

		public Color ChargedColor => _chargedColor;

		public Color DischargedColor => _dischargedColor;

		public void Init(ShaderProperty propertyName)
		{
			_propertyName = propertyName;
		}

		public void Init(Func<bool> isCharged) =>
			_isChanged = isCharged;

		public void ChangeColor() =>
			ChangeColor(GetIntensityColor(_isChanged()));

		public void ChangeColor(bool isCharged) =>
			ChangeColor(GetIntensityColor(isCharged));

		public void ChangeColor(Color intensityColor) =>
			SetColor(intensityColor);

		private Color GetIntensityColor(bool isChanged)
		{
			var (color, intensity) = isChanged
				? (_chargedColor, _intensityChargedColor)
				: (_dischargedColor, _intensityDischargedColor);

			return color.CalculateIntensityColor(intensity);
		}

		private void SetColor(Color targetColor)
		{
			foreach (var meshRenderer in _bloomMeshRenderers)
			{
				meshRenderer.material.SetColor(_propertyName.GetCurrentName(), targetColor);
			}
		}
	}
}