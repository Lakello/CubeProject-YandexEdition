using CubeProject.Game;
using LeadTools.Extensions;
using LeadTools.Other;
using UnityEngine;

namespace CubeProject.Player
{
	public class CubeView : MonoBehaviour
	{
		private const string LightColor = "_LightColor";

		[SerializeField] private HDRColorChanger _hdrColorChanger;

		private ChargeHolder _chargeHolder;

		private void Awake()
		{
			gameObject.GetComponentElseThrow(out _chargeHolder);
			_hdrColorChanger.Init(() => _chargeHolder.IsCharged);
			_hdrColorChanger.Init(LightColor);
		}

		private void OnEnable()
		{
			_chargeHolder.ChargeChanged += _hdrColorChanger.ChangeColor;
			_hdrColorChanger.ChangeColor();
		}

		private void OnDisable() =>
			_chargeHolder.ChargeChanged -= _hdrColorChanger.ChangeColor;
	}
}