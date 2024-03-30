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

        public void InitTransition<TTargetState, TTargetMachine>(ITransitSubject transitSubject, TTargetMachine machine, Action observer = null)
            where TTargetMachine : StateMachine<TTargetMachine>
            where TTargetState : State<TTargetMachine>
        {
            var transition = new Transition<TTargetMachine, TTargetState>(machine);
            
            InitTransition(transitSubject, () =>
            {
                transition.Transit();
                observer?.Invoke();
            });
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

        public void Subscribe()
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