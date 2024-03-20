using System;
using System.Collections.Generic;

namespace LeadTools.StateMachine
{
    public class TransitionInitializer<TMachine>
        where TMachine : StateMachine<TMachine>
    {
        private readonly TMachine _stateMachine;
        private readonly List<Subscription> _subscriptions = new List<Subscription>();

        public TransitionInitializer(TMachine stateMachine, out Action subscribe, out Action unsubscribe)
        {
            subscribe = Subscribe;
            unsubscribe = Unsubscribe;
            
            _stateMachine = stateMachine;
        }
        
        public void InitTransition<TTargetState>(ITransitSubject transitSubject, Action observer = null)
            where TTargetState : State<TMachine>
        {
            var transition = new Transition<TMachine, TTargetState>(_stateMachine);
            
            InitTransition(transitSubject, () =>
            {
                transition.Transit();
                observer?.Invoke();
            });
        }

        public void InitTransition(ITransitSubject transitSubject, Action observer) =>
            _subscriptions.Add(new Subscription(transitSubject, observer));

        private void Subscribe()
        {
            if (_subscriptions == null)
            {
                return;
            }

            foreach (var action in _subscriptions)
            {
                action.TransitSubject.StateTransiting += action.Observer;
            }
        }

        private void Unsubscribe()
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