using LeadTools.Extensions;
using LeadTools.Other;
using UnityEngine;

namespace CubeProject.Game
{
	public sealed class ChargeView : MonoBehaviour
	{
		private const string EmissionColor = "_EmissionColor";
		
		[SerializeField] private HDRColorChanger _hdrColorChanger;

		private IChargeable _chargeable;

		private void Awake()
		{
			gameObject.GetComponentElseThrow(out _chargeable);

			_hdrColorChanger.Init(() => _chargeable.IsCharged);
			_hdrColorChanger.Init(EmissionColor);
		}

		private void OnEnable()
		{
			_chargeable.ChargeChanged += OnChargeChanged;

			OnChargeChanged();
		}

		private void OnDisable() =>
			_chargeable.ChargeChanged -= OnChargeChanged;

		private void OnChargeChanged() =>
			_hdrColorChanger.ChangeColor();
	}
}