using System;

namespace Game.Player
{
	public interface IChargeable
	{
		public event Action ChargeChanged;

		public bool IsCharged { get; }
	}
}