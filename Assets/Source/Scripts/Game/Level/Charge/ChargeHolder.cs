using System;
using UnityEngine;

namespace CubeProject.Game
{
	public sealed class ChargeHolder : MonoBehaviour, IChargeable
	{
		[SerializeField] private bool _isAlwaysCharged;
		[SerializeField] private bool _isStartCharged;

		public event Action ChargeChanged;

		public bool IsCharged => SelfPower != null;

		public Power SelfPower { get; private set; }

		private void Start()
		{
			if (_isStartCharged || _isAlwaysCharged)
			{
				SelfPower = new GameObject(nameof(Power)).AddComponent<Power>();
				SelfPower.Init(this);

				Charge(SelfPower);
			}
		}

		public void Charge(Power power) =>
			ChangeCharge(power);

		public void Defuse()
		{
			if (_isAlwaysCharged is false)
			{
				ChangeCharge();
			}
		}

		private void ChangeCharge(Power power = null)
		{
			SelfPower = power;
			ChargeChanged?.Invoke();
		}
	}
}