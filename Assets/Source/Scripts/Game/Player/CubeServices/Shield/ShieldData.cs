using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CubeProject.Game.Player.CubeService.Shield
{
	[Serializable]
	public class ShieldData
	{
		[SerializeField] [MinMaxSlider(0, 10f)] private Vector2 _distanceRange;
		[SerializeField] [MinMaxSlider(0, 1f)] private Vector2 _fresnelPowerRange;
		[SerializeField] [MinMaxSlider(0.001f, 0.01f)] private Vector2 _displacementAmountRange;
		[SerializeField] [Range(-50, 50)] private float _fresnelPowerHide;
		[SerializeField] [Range(0, 2)] private float _hideShowDuration;

		public Vector2 DistanceRange => _distanceRange;
		public Vector2 FresnelPowerRange => _fresnelPowerRange;
		public Vector2 DisplacementAmountRange => _displacementAmountRange;
		public float FresnelPowerHide => _fresnelPowerHide;
		public float HideShowDuration => _hideShowDuration;
	}
}