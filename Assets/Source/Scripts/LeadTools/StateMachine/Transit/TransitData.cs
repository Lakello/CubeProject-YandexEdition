using System;

namespace LeadTools.StateMachine
{
	public class TransitData
	{
		public ITransitSubject Subject;
		public Action Observer;
	}
}