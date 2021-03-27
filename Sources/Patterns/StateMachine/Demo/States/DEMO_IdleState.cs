using GameDevStack.Patterns;

namespace GameDevStack.Demos
{
    //public class DEMO_IdleState : IState
    public class DEMO_IdleState : AdvancedState<DEMO_FSMController, State>, IState
    {
        //protected T m_FSMController;
        //public DEMO_IdleState(FSMController fSMController)
        //{
        //    m_FSMController = fSMController;
        //}

        public DEMO_IdleState(DEMO_FSMController fSMController) : base(fSMController) { }

        public void Enter()
        {
        }

        public void Exit()
        {
        }

        public void FixedUpdate()
        {
        }

        public void LateUpdate()
        {
        }

        public void Update()
        {
        }
    }
}