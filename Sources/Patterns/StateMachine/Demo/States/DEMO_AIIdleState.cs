using GameDevStack.Patterns;

namespace GameDevStack.Demos
{
    public class DEMO_AIIdleState : AdvancedState<DEMO_FSMController>, IState
    {
        public DEMO_AIIdleState(DEMO_FSMController fSMController) : base(fSMController) { }

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
        }

        public void LateUpdate()
        {
        }
    }
}