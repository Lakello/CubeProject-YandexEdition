using System;
using UnityEngine;

namespace CubeProject.Game
{
	public sealed class ChargeHolder : MonoBehaviour, IChargeable
	{
		[SerializeField] private bool _isAlwaysCharged;
		[SerializeField] private bool _isStartCharged;
		
		public event Action ChargeChanged;

		public bool IsCharged { get; private set; }

		private void Start()
		{
			if (_isStartCharged || _isAlwaysCharged)
			{
				Charge();
			}
		}

		public void Charge() =>
			ChangeCharge(true);

		public void Defuse()
		{
			if (_isAlwaysCharged is false)
			{
				ChangeCharge(false);
			}
		}

		private void ChangeCharge(bool value)
		{
			IsCharged = value;
			ChargeChanged?.Invoke();
		}
	}
}