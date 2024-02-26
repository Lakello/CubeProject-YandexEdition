using System;

namespace CubeProject.Game
{
	public interface IChargeable
	{
		public event Action ChargeChanged;

		public bool IsCharged { get; }
	}
}