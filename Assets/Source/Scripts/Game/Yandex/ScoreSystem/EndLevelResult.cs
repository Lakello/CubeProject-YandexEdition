using LeadTools.StateMachine;
using LeadTools.StateMachine.States;
using Reflex.Attributes;
using System;
using UnityEngine;

namespace CubeProject.UI
{
    public class EndLevelResult : MonoBehaviour
    {
        private IStateChangeable<GameStateMachine> _stateMachine;
        public event Action LevelCompleted;

        [Inject]
        private void Inject(IStateChangeable<GameStateMachine> machine)
        {           
            _stateMachine = machine;
            _stateMachine.SubscribeTo<EndLevelState>(OnLevelEnd);
        }

        private void OnDisable()
        {
            _stateMachine.UnSubscribeTo<EndLevelState>(OnLevelEnd);
        }

        private void OnLevelEnd(bool isEntered)
        {
            if (isEntered)
            {              
                LevelCompleted?.Invoke();
            }
        }
    }
}