using System;
using System.Collections.Generic;

namespace LeadTools.StateMachine
{
	public class TransitionInitializer<TMachine>
		where TMachine : StateMachine<TMachine>
	{
		private readonly TMachine _stateMachine;
		private readonly List<Subscription> _subscriptions = new List<Subscription>();

		public TransitionInitializer(TMachine stateMachine) =>
			_stateMachine = stateMachine;

		public TransitionInitializer<TMachine> InitTransition<TTargetState, TTargetMachine>(
			ITransitSubject transitSubject,
			TTargetMachine machine,
			Action observer = null)
			where TTargetMachine : StateMachine<TTargetMachine>
			where TTargetState : State<TTargetMachine>
		{
			var transition = new Transition<TTargetMachine, TTargetState>(machine);

			InitTransition(transitSubject, () =>
			{
				transition.Transit();
				observer?.Invoke();
			});

			return this;
		}

		public TransitionInitializer<TMachine> InitTransition<TTargetState>(
			ITransitSubject transitSubject,
			Action observer = null)
			where TTargetState : State<TMachine>
		{
			var transition = new Transition<TMachine, TTargetState>(_stateMachine);

			InitTransition(transitSubject, () =>
			{
				transition.Transit();
				observer?.Invoke();
			});

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
			if (_subscriptions != null)
			{
				foreach (var action in _subscriptions)
				{
					action.TransitSubject.StateTransiting += action.Observer;
				}
			}

			return this;
		}

		public void Unsubscribe()
		{
			if (_subscriptions == null)
			{
				return;
			}

			foreach (var subscription in _subscriptions)
			{
				subscription.TransitSubject.StateTransiting -= subscription.Observer;
			}
		}
	}
}