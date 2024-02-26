using UnityEngine;

namespace LeadTools.StateMachine
{ 
    public abstract class WindowState : State<WindowStateMachine>
    {
        private Window _window;

        public void Init(Window window) => 
            _window = window;

        public override void Enter() =>
            _window.gameObject.SetActive(true);

        public override void Exit()
        {
            if (_window != null)
                _window.gameObject.SetActive(false);
        }
    }
}