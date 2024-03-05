using System;
using UnityEngine;

namespace CubeProject.Game
{
	[Serializable]
	public struct BarrierViewData
	{
		[SerializeField] private Color _color;
		[SerializeField] [Range(-10f, 10f)] private float _intensity;
		[SerializeField] [Range(0f, 1f)] private float _maskPower;
		[SerializeField] private AnimationCurve _animationMaskPower;
		[SerializeField] private float _changingStateDuration;

		public Color Color => _color;

		public float Intensity => _intensity;

		public float MaskPower => _maskPower;

		public AnimationCurve AnimationMaskPower => _animationMaskPower;

		public float ChangingStateDuration => _changingStateDuration;
	}
}