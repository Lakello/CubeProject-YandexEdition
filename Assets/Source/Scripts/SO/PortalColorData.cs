using System;
using UnityEngine;

namespace CubeProject.SO
{
	[CreateAssetMenu(menuName = "Portal/new PortalColorData", fileName = "newPortalColorData")]
	public class PortalColorData : ScriptableObject
	{
		[SerializeField] [Range(0, 1)] private float _activeAlpha;
		[SerializeField] [Range(0, 1)] private float _inactiveAlpha;
		[SerializeField] private Color[] _colors;

		private int _currentColorIndex;
		
		public float ActiveAlpha => _activeAlpha;

		public float InactiveAlpha => _inactiveAlpha;

		public void ResetColorIndex()
		{
			_currentColorIndex = 0;
		}

		public Color GetColor()
		{
			if (_currentColorIndex >= _colors.Length)
			{
				Debug.LogError($"Required more colors");

				return new Color();
			}

			var color = _colors[_currentColorIndex];

			_currentColorIndex++;
			
			return color;
		}
	}
}