using CubeProject.Game;
using LeadTools.Extensions;
using LeadTools.Other;
using UnityEngine;

namespace CubeProject.PlayableCube
{
	public class CubeView : MonoBehaviour
	{
		private const string HeartColorProperty = "_HeartColor";
		private const string EnergyColorProperty = "_EnergyProperty";

		[SerializeField] private HDRColorChanger _energyColorChanger;
		[SerializeField] private HDRColorChanger _heartColorChanger;

		private ChargeHolder _chargeHolder;

		private void Awake()
		{
			gameObject.GetComponentElseThrow(out _chargeHolder);
			
			InitColorChanger(_energyColorChanger, EnergyColorProperty);
			InitColorChanger(_heartColorChanger, HeartColorProperty);
		}

		private void OnEnable()
		{
			_chargeHolder.ChargeChanged += _energyColorChanger.ChangeColor;
			_chargeHolder.ChargeChanged += _heartColorChanger.ChangeColor;
			_energyColorChanger.ChangeColor();
		}

		private void OnDisable()
		{
			_chargeHolder.ChargeChanged -= _energyColorChanger.ChangeColor;
			_chargeHolder.ChargeChanged -= _heartColorChanger.ChangeColor;
		}

		private void InitColorChanger(HDRColorChanger changer, string property)
		{
			changer.Init(() => _chargeHolder.IsCharged);
			changer.Init(property);
		}
	}
}