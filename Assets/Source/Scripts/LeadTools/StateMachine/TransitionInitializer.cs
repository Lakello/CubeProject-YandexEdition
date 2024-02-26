using System;
using System.Collections.Generic;

namespace LeadTools.StateMachine
{
    public class TransitionInitializer<TMachine> where TMachine : StateMachine<TMachine>
    {
        private readonly TMachine _stateMachine;
        private readonly List<Subscription> _subscriptions = new List<Subscription>();

        public TransitionInitializer(TMachine stateMachine, out Action subscribe, out Action unsubscribe)
        {
            subscribe = Subscribe;
            unsubscribe = Unsubscribe;
            
            _stateMachine = stateMachine;
        }

        public void InitTransition<TTargetState>(ISubject subject, Action observer = null)
            where TTargetState : State<TMachine>
        {
            var transition = new Transition<TMachine, TTargetState>(_stateMachine);
            
            InitTransition(subject, () =>
            {
                transition.Transit();
                observer?.Invoke();
            });
        }

        public void InitTransition(ISubject subject, Action observer) =>
            _subscriptions.Add(new Subscription(subject, observer));

        private void Subscribe()
        {
            if (_subscriptions == null)
            {
                return;
            }

            foreach (var action in _subscriptions)
            {
                action.Subject.ActionEnded += action.Observer;
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
                subscription.Subject.ActionEnded -= subscription.Observer;
            }
        }
    }
}