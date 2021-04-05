using System.Collections.Generic;
using GameDevStack.Patterns;
using UnityEngine;

namespace GameDevStack.Demos
{
    public class DEMO_AIFSMController : DEMO_FSMController
    {
        protected override void InitializeFSM(List<IState> states, State defaultState, bool startPlaying = true)
        {
            defaultState = State.Move;

            // V1 : Court mais possible d'avoir des erreurs
            //states[1] = new DEMO_AIMoveState(this);

            // V2 : Long mais impossible d'avoir des erreurs
            //states = FSM<State>.ReplaceState(states, new DEMO_MoveState(this), new DEMO_AIMoveState(this));

            // V3 : Presque parfait (trouver un autre endroit un ranger la fonction : "ReplaceState")
            //states = FSM<State>.ReplaceState(states, State.Move, new DEMO_AIMoveState(this));

            // V4 : Parfait ? (le faire en extension plutôt ?)
            //states = FSMUtilities.ReplaceType(states, State.Move, new DEMO_AIMoveState(this));

            // V5 : Avec une extension
            //states.ReplaceState(State.Idle, new DEMO_AIIdleState(this));
            states.ReplaceState(State.Move, new DEMO_AIMoveState(this));

            // V6 : Version avancé pour override plusieurs states (un peu lourd, plutôt privilégier la V5)
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

        [ContextMenu("Override State")]
        private void OverrideState()
        {
            m_FSM.OverrideState(State.Move, new DEMO_AIMoveState(this));
        }
    }
}