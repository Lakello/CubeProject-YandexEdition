using System;

namespace LeadTools.StateMachine
{
	public interface ITransitSubject
	{
		public event Action StateTransiting;
	}
}