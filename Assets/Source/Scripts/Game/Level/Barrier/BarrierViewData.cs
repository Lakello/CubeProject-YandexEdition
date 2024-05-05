using System;
using UnityEngine;

namespace CubeProject.Game
{
	[Serializable]
	public struct BarrierViewData
	{
		[SerializeField] private Gradient _gradient;
		[SerializeField] private AnimationCurve _maskPowerCurve;
		[SerializeField] private AnimationCurve _clipCurve;
		[SerializeField] private float _changingStateDuration;

		public Gradient Gradient => _gradient;
		public AnimationCurve MaskPowerCurve => _maskPowerCurve;
		public AnimationCurve ClipCurve => _clipCurve;
		public float ChangingStateDuration => _changingStateDuration;
	}
}