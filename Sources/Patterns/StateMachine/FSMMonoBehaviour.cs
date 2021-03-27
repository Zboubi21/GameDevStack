using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameDevStack.Patterns
{
    [Serializable]
    public class FSMMonoBehaviour<T> : MonoBehaviour
    {
        [SerializeField] protected FSM<T> m_FSM = null;

        // Debug
        [SerializeField] private bool m_PlayingAtStart = false;
        [Space]
        [SerializeField] private string m_CurrentStateString = null;
        [SerializeField] private string m_LastStateString = null;
        [SerializeField] private bool m_IsPlaying = false;

        protected void SetupStateMachine(List<IState> states, T defaultState)
        {
            m_FSM = new FSM<T>(states, defaultState);

            if (m_PlayingAtStart)
                m_FSM.Start();
        }

        public void ChangeState(T state)
        {
            m_FSM.ChangeState(state);
        }

        protected virtual void FixedUpdate() => m_FSM.FixedUpdate();
        protected virtual void Update() => m_FSM.Update();
        protected virtual void LateUpdate()
        {
            m_FSM.LateUpdate();

            m_CurrentStateString = m_FSM.m_CurrentStateString;
            m_LastStateString = m_FSM.m_LastStateString;
            m_IsPlaying = m_FSM.IsPlaying;
        }
    }
}