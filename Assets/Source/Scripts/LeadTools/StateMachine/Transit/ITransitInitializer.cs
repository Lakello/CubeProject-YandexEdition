using System;
using CubeProject.SO;

namespace LeadTools.StateMachine
{
	public interface ITransitInitializer
	{
		public event Action<MenuWindowButton> Transiting;
	}
}