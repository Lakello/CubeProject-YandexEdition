using System;

namespace CubeProject.Game.Player
{
	public interface IChargeable
	{
		public event Action ChargeChanged;

		public bool IsCharged { get; }
	}
}