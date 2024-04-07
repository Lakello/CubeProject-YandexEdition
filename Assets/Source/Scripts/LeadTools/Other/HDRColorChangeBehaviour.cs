using CubeProject.Game;
using LeadTools.Extensions;
using LeadTools.Other;
using UnityEngine;

namespace CubeProject.PlayableCube
{
	public class HDRColorChangeBehaviour : MonoBehaviour
	{
		[SerializeField] private HDRColorChanger[] _changers;

		private IChargeable _chargeable;

		private void Awake()
		{
			gameObject.GetComponentElseThrow(out _chargeable);

			foreach (var changer in _changers)
			{
				changer.Init(() => _chargeable.IsCharged);
			}
		}

		private void OnEnable()
		{
			foreach (var changer in _changers)
			{
				_chargeable.ChargeChanged += changer.ChangeColor;
				changer.ChangeColor();
			}
		}

		private void OnDisable()
		{
			foreach (var changer in _changers)
			{
				_chargeable.ChargeChanged -= changer.ChangeColor;
			}
		}
	}
}