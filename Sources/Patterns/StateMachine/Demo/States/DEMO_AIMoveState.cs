using GameDevStack.Patterns;

namespace GameDevStack.Demos
{
    public class DEMO_AIMoveState : AdvancedState<DEMO_FSMController>, IState
    {
        public DEMO_AIMoveState(DEMO_FSMController fSMController) : base(fSMController) { }

        public bool CanEnter()
        {
            if (m_FSMController.IsMoving)
                return false;

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
        }

        public void LateUpdate()
        {
        }
    }
}