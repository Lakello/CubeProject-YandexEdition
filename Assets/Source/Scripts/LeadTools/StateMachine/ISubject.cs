using System;

namespace LeadTools.StateMachine
{
    public interface ISubject
    {
        public event Action ActionEnded;
    }
}