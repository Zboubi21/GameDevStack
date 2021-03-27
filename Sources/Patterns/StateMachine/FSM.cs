using System.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameDevStack.Patterns
{
    [Serializable]
    public class FSM<T>
    {
        /************
        * Constants *
        ************/
        private const string STATE_COUNT_ERROR = "You don't have the same state enum count and state script count!";

        /*****************
        * SerializeField *
        *****************/
        [Header("Debug")]
        [SerializeField] public string m_CurrentStateString;
        [SerializeField] public string m_LastStateString;

        /**********
        * Private *
        **********/
        private Dictionary<T, IState> m_States = new Dictionary<T, IState>();
        private IState m_CurrentIState = null;
        private T m_CurrentState = default;
        private IState m_LastIState = null;
        private T m_LastState = default;
        private bool m_IsPlaying = false;

        /*********
        * Getter *
        *********/
        public T CurrentState => m_CurrentState;
        //public T LastState => m_LastIState == null ? default : m_LastState;
        public bool IsPlaying => m_IsPlaying;

        public bool TryGetLastState(out T lastState)
        {
            if (m_LastIState == null)
            {
                lastState = default;
                return false;
            }
            else
            {
                lastState = m_LastState;
                return true;
            }
        }

        /**************
        * Constructor *
        **************/
        public FSM(List<IState> statesAdded, T defaultState)
        {
            List<T> stateEnum = Enum.GetValues(typeof(T)).Cast<T>().ToList();

            if (stateEnum.Count != statesAdded.Count)
                Debug.LogError(STATE_COUNT_ERROR);

            for (int i = 0; i < stateEnum.Count; i++)
            {
                m_States.Add(stateEnum[i], statesAdded[i]);
            }

            SetCurrentState(defaultState);
        }

        /*******
        * Core *
        *******/
        public void Start()
        {
            if (m_IsPlaying) return;
            m_IsPlaying = true;

            EnterCurrentIState();
        }

        public void ChangeState(T state)
        {
            if (!m_IsPlaying) return;

            ExitCurrentIState();

            m_LastIState = m_CurrentIState;
            m_LastState = m_CurrentState;
            m_LastStateString = m_CurrentState.ToString();

            SetCurrentState(state);
        }

        public void Stop()
        {
            if (!m_IsPlaying) return;

            ExitCurrentIState();
            m_CurrentState = default;

            m_IsPlaying = false;
        }

        private void SetCurrentState(T state)
        {
            m_CurrentIState = m_States[state];
            m_CurrentState = state;
            m_CurrentStateString = m_CurrentState.ToString();
            EnterCurrentIState();
        }

        private void EnterCurrentIState()
        {
            if (!m_IsPlaying) return;
            m_CurrentIState.Enter();
            Debug.Log(m_CurrentState + " Enter");
        }

        private void ExitCurrentIState()
        {
            if (!m_IsPlaying) return;
            m_CurrentIState.Exit();
            if (m_CurrentIState != null)
                Debug.Log(m_CurrentState + " Exit");
        }

        /*********
        * Update *
        *********/
        public void FixedUpdate()
        {
            if (!m_IsPlaying) return;
            m_CurrentIState?.FixedUpdate();
        }
        public void Update()
        {
            if (!m_IsPlaying) return;
            m_CurrentIState?.Update();
        }
        public void LateUpdate()
        {
            if (!m_IsPlaying) return;
            m_CurrentIState?.LateUpdate();
        }
    }
}