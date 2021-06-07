using System;
using GameDevStack.Patterns;

namespace GameDevStack.Demos
{
    public class DEMO_AIMoveState : State<DEMO_FSMController>
    {
        public DEMO_AIMoveState(DEMO_FSMController fSMController) : base(fSMController) { }

        public override void Enter()
        {
        }

        public override void Exit()
        {
        }

        public override void FixedUpdate()
        {
        }

        public override void Update()
        {
        }

        public override void LateUpdate()
        {
        }

        public override Enum GetNextState()
        {
            return State.None;
        }
    }
}