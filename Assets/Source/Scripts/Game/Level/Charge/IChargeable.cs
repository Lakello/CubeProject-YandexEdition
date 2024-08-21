using System;

namespace CubeProject.Game.Level.Charge
{
	public interface IChargeable
	{
		public event Action ChargeChanged;

		public bool IsCharged { get; }
	}
}