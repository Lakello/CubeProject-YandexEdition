using System;
using UnityEngine;

namespace LeadTools.Other
{
	[Serializable]
	public struct FloatRange
	{
		[SerializeField] private float _min;
		[SerializeField] private float _max;

		public FloatRange(float min, float max)
		{
			_min = min;
			_max = max;
		}

		public float Min => _min;

		public float Max => _max;
	}
}