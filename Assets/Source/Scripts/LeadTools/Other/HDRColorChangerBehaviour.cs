using CubeProject.Game;
using LeadTools.Extensions;
using LeadTools.Other;
using UnityEngine;

namespace CubeProject.PlayableCube
{
	[RequireComponent(typeof(ChargeHolder))]
	public class HDRColorChangerBehaviour : MonoBehaviour
	{
		[SerializeField] private HDRColorChanger[] _changers;
		[SerializeField] private HDRColorChanger _energyColorChanger;
		[SerializeField] private HDRColorChanger _heartColorChanger;

		private ChargeHolder _chargeHolder;

		private void Awake()
		{
			gameObject.GetComponentElseThrow(out _chargeHolder);

			foreach (var changer in _changers)
			{
				changer.Init(() => _chargeHolder.IsCharged);
			}
		}

		private void OnEnable()
		{
			foreach (var changer in _changers)
			{
				_chargeHolder.ChargeChanged += changer.ChangeColor;
				changer.ChangeColor();
			}
		}

		private void OnDisable()
		{
			foreach (var changer in _changers)
			{
				_chargeHolder.ChargeChanged -= changer.ChangeColor;
			}
		}
	}
}