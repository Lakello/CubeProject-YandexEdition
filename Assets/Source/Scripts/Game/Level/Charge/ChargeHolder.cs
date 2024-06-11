using System;
using LeadTools.NaughtyAttributes;
using UnityEngine;

namespace CubeProject.Game.Player
{
	public sealed class ChargeHolder : MonoBehaviour, IChargeable
	{
		[SerializeField] private bool _isAlwaysCharged;
		[SerializeField] private bool _isStartCharged;

		public event Action ChargeChanged;

		[ShowNativeProperty] public bool IsCharged { get; private set; }

		private void Start()
		{
			if (_isStartCharged || _isAlwaysCharged)
				Charge();
		}

		public void Charge() =>
			ChangeCharge(true);

		public void Defuse()
		{
			if (_isAlwaysCharged is false)
				ChangeCharge(false);
		}

		private void ChangeCharge(bool newCharge)
		{
			IsCharged = newCharge;
			ChargeChanged?.Invoke();
		}
	}
}