using System;
using UnityEngine;

namespace Game.Player
{
	[Serializable]
	public class TeleporterData
	{
		[SerializeField] private AnimationCurve _scaleCurve;
		[SerializeField] private AnimationCurve _heightCurve;
		[SerializeField] private float _animationTime;

		public AnimationCurve ScaleCurve => _scaleCurve;
		public AnimationCurve HeightCurve => _heightCurve;
		public float AnimationTime => _animationTime;
	}
}