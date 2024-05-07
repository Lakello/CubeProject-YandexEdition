using System;
using System.Collections.Generic;
using Sirenix.Utilities;

namespace LeadTools.StateMachine
{
	public class TransitionInitializer<TMachine>
		where TMachine : StateMachine<TMachine>
	{
		private readonly TMachine _stateMachine;
		private readonly List<Subscription> _subscriptions = new List<Subscription>();

		public TransitionInitializer(TMachine stateMachine) =>
			_stateMachine = stateMachine;

		public TransitionInitializer<TMachine> InitTransition<TTargetState>(
			ITransitSubject transitSubject,
			Action observer = null)
			where TTargetState : State<TMachine>
		{
			var transition = new Transition<TMachine, TTargetState>(_stateMachine);

			InitTransition(transitSubject, observer, transition);

			return this;
		}
		
		public TransitionInitializer<TMachine> InitTransition<TTargetState>(
			IEnumerable<ITransitSubject> transitSubjects,
			Action observer = null)
			where TTargetState : State<TMachine>
		{
			var transition = new Transition<TMachine, TTargetState>(_stateMachine);

			transitSubjects.ForEach(subject => InitTransition(subject, observer, transition));

			return this;
		}

		public TransitionInitializer<TMachine> InitTransition(
			ITransitSubject transitSubject,
			Action observer)
		{
			_subscriptions.Add(new Subscription(transitSubject, observer));

			return this;
		}

		public TransitionInitializer<TMachine> Subscribe()
		{
			_subscriptions?
				.ForEach(
					subscription => subscription.TransitSubject.StateTransiting += subscription.Observer);

			return this;
		}

		public void Unsubscribe() =>
			_subscriptions?
				.ForEach(
					subscription => subscription.TransitSubject.StateTransiting += subscription.Observer);

		private void InitTransition<TTargetState, TTargetMachine>(
			ITransitSubject transitSubject,
			Action observer,
			Transition<TTargetMachine, TTargetState> transition)
			where TTargetMachine : StateMachine<TTargetMachine>
			where TTargetState : State<TTargetMachine>
		{
			InitTransition(transitSubject, () =>
			{
				transition.Transit();
				observer?.Invoke();
			});
		}
	}
}