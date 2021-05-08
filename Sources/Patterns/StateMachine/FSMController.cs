using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameDevStack.Patterns
{
    public class FSMController : MonoBehaviour
    {
        protected FSM m_FSM = null;
        public FSM FSM => m_FSM;

        protected virtual void InitializeFSM(List<IState> states, Enum defaultState, bool startPlaying = true)
        {
            m_FSM = new FSM(states, defaultState);

            if (startPlaying)
                m_FSM.Start();
        }

        public void StartFSM()
        {
            m_FSM.Start();
        }

        public void ChangeState(Enum state)
        {
            m_FSM.ChangeState(state);
        }

        protected virtual void FixedUpdate() => m_FSM.FixedUpdate();
        protected virtual void Update() => m_FSM.Update();
        protected virtual void LateUpdate()
        {
            m_FSM.LateUpdate();
            m_FSM.CheckChangeStateUpdate();
        }
    }
}