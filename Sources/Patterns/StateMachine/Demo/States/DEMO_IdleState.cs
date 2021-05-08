using GameDevStack.Patterns;

namespace GameDevStack.Demos
{
    //public class DEMO_IdleState : IState
    public class DEMO_IdleState : AdvancedState<DEMO_FSMController>, IState
    {
        //private DEMO_FSMController m_FSMController;

        //public DEMO_IdleState(DEMO_FSMController fSMController)
        //{
        //    m_FSMController = fSMController;
        //}

        public DEMO_IdleState(DEMO_FSMController fSMController) : base(fSMController) { }

        public bool CanEnter()
        {
            return true;
        }

        public bool CanExit()
        {
            return true;
        }

        public void Enter()
        {
        }

        public void Exit()
        {
        }

        public void FixedUpdate()
        {
        }

        public void Update()
        {
            if (m_FSMController.IsMoving)
                m_FSMController.ChangeState(State.Move);
        }

        public void LateUpdate()
        {
        }

        //public bool TryGetNextState(out string state)
        //{
        //    state = State.Idle.ToString();

        //    return false;
        //}

        //public bool TryGetNextState(out State state)
        //{
        //    state = State.Idle;

        //    return false;
        //}
    }
}