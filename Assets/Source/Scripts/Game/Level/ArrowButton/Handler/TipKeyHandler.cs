using System;
using CubeProject.Game;
using LeadTools.Extensions;
using UnityEngine;

namespace CubeProject.Tips
{
	[RequireComponent(typeof(TipKey))]
	public abstract class TipKeyHandler : MonoBehaviour, IChargeable
	{
		private TipKey _tipKey;

		public event Action ChargeChanged;

		public bool IsCharged { get; private set; }

		protected TipKey TipKey => _tipKey;

		private void Awake() =>
			gameObject.GetComponentElseThrow(out _tipKey);

		protected void TryRelease()
		{
			if (TipKey.TryRelease())
			{
				IsCharged = false;
				ChargeChanged?.Invoke();
			}
		}

		protected void TryPress()
		{
			if (TipKey.TryPress())
			{
				IsCharged = true;
				ChargeChanged?.Invoke();
			}
		}
	}
}