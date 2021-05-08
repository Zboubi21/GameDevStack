using System;
using System.Collections.Generic;
using GameDevStack.Patterns;

namespace GameDevStack.Demos
{
    public class DEMO_AIFSMController : DEMO_FSMController
    {
        protected override void InitializeFSM(List<IState> states, Enum defaultState, bool startPlaying = true)
        {
            //defaultState = State.Move;

            //states.ReplaceState(State.Idle, new DEMO_AIIdleState(this));
            states.ReplaceState(State.Move, new DEMO_AIMoveState(this));

            // Version avancé pour override plusieurs states
            //states.ReplaceStates(new List<State>
            //{
            //    State.Idle,
            //    State.Move
            //},
            //new List<IState>
            //{
            //    new DEMO_AIIdleState(this),
            //    new DEMO_AIMoveState(this)
            //}
            //);

            base.InitializeFSM(states, defaultState, startPlaying);
        }
    }
}