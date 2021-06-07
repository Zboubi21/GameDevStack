using System;

namespace GameDevStack.Patterns
{
    public abstract class State<FSMController> : IState
    {
        protected FSMController m_FSMController;

        public State(FSMController fSMController)
        {
            m_FSMController = fSMController;
        }

        public abstract void Enter();
        public abstract void Exit();

        public abstract void FixedUpdate();
        public abstract void Update();
        public abstract void LateUpdate();

        public abstract Enum GetNextState();
    }
}