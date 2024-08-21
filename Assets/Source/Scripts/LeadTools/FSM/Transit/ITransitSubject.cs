using System;

namespace LeadTools.FSM.Transit
{
	public interface ITransitSubject
	{
		public event Action StateTransiting;
	}
}