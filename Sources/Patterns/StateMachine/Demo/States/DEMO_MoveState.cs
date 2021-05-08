using GameDevStack.Patterns;

namespace GameDevStack.Demos
{
    public class DEMO_MoveState : AdvancedState<DEMO_FSMController>, IState
    {
        public DEMO_MoveState(DEMO_FSMController fSMController) : base(fSMController) { }
        
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
            if (!m_FSMController.IsMoving)
                m_FSMController.ChangeState(State.Idle);
        }

        public void LateUpdate()
        {
        }
    }
}