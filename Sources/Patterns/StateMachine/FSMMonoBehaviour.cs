using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameDevStack.Patterns
{
    [Serializable]
    public class FSMMonoBehaviour<T> : MonoBehaviour where T : Enum
    {
        [SerializeField] protected FSM<T> m_FSM = null;
        public FSM<T> FSM => m_FSM;

        protected virtual void InitializeFSM(List<IState> states, T defaultState, bool startPlaying = true)
        {
            m_FSM = new FSM<T>(states, defaultState);

            if (startPlaying)
                m_FSM.Start();
        }

        public void StartFSM()
        {
            m_FSM.Start();
        }

        public void ChangeState(T state)
        {
            m_FSM.ChangeState(state);
        }

        protected virtual void FixedUpdate() => m_FSM.FixedUpdate();
        protected virtual void Update() => m_FSM.Update();
        protected virtual void LateUpdate() => m_FSM.LateUpdate();
    }
}