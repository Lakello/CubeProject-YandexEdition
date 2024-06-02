using System.Collections.Generic;
using Sirenix.Utilities;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CubeProject.SO
{
	[CreateAssetMenu(menuName = "Portal/new PortalColorData", fileName = "newPortalColorData")]
	public class PortalColorData : ScriptableObject
	{
		[SerializeField] [Range(-100, 100)] private float _dissolveAmountShow;
		[SerializeField] [Range(-10, 10)] private float _intensity;
		[SerializeField] private float _durationAnimation;
		[SerializeField] private Color[] _colors;

		private Queue<Color> _colorQueue;

		public float DissolveAmountShow => _dissolveAmountShow;
		public float Intensity => _intensity;
		public float DurationAnimation => _durationAnimation;

		public void ResetColors()
		{
			_colorQueue = new Queue<Color>();

			var colors = ShuffleColors();

			colors.ForEach(color => _colorQueue.Enqueue(color));
		}

		public Color GetColor()
		{
			if (_colorQueue.Count == 0)
			{
				Debug.LogError($"Required more colors");

				return new Color();
			}

			return _colorQueue.Dequeue();
		}

		private Color[] ShuffleColors()
		{
			var colors = (Color[])_colors.Clone();

			int n = colors.Length;

			while (n > 1)
			{
				n--;
				int k = Random.Range(0, n + 1);

				(colors[k], colors[n]) = (colors[n], colors[k]);
			}

			return colors;
		}
	}
}