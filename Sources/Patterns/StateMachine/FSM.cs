using System.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameDevStack.Patterns
{
    public class FSM
    {
        /************
        * Constants *
        ************/
        private const string STATE_COUNT_ERROR = "You do not have the same state enum count and state script count!";
        private const int MAX_LAST_STATES_COUNT = 10;
        private const string SET_MAX_LAST_STATES_COUNT = "You cannot set a count < 1!";

        /**********
        * Private *
        **********/
        private Dictionary<string, IState> m_States = new Dictionary<string, IState>();
        private IState m_CurrentIState = null;
        private string m_CurrentState = default;
        private IState m_LastIState = null;
        private string m_LastState = default;
        private bool m_IsPlaying = false;
        private List<string> m_LastStates = new List<string>();

        /**********
        * Getters *
        **********/
        public Dictionary<string, IState> States => m_States;
        public IState CurrentIState => m_CurrentIState;
        public string CurrentState => m_CurrentState;
        //public string LastState => m_LastIState == null ? default : m_LastState;
        public IState LastIState => m_LastIState;
        public bool IsPlaying => m_IsPlaying;
        public List<string> LastStates => m_LastStates;
        private int m_MaxLastStatesCount = -1;

        public bool TryGetLastState(out string lastState)
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

        /*****************
        * Initialization *
        *****************/
        public FSM(List<IState> statesAdded, Enum defaultState)
        {
            string defaultStateName = defaultState.ToString();
            Array array = defaultState.GetType().GetEnumValues();
            List<string> states = System.Enum.GetValues(defaultState.GetType()).Cast<Enum>().ToList().ConvertAll(e => e.ToString());

            if (states.Count != statesAdded.Count)
                Debug.LogError(STATE_COUNT_ERROR);

            for (int i = 0; i < states.Count; i++)
            {
                m_States.Add(states[i], statesAdded[i]);
            }

            SetCurrentState(defaultStateName);
        }

        /*******
        * Core *
        *******/
        // Public
        public void Start()
        {
            if (m_IsPlaying) return;
            m_IsPlaying = true;

            EnterCurrentIState();
        }

        public void ChangeState(Enum state)
        {
            if (!m_IsPlaying) return;

            ExitCurrentIState();
            SetLastState(m_CurrentState);
            SetCurrentState(state.ToString());
        }

        public void Stop()
        {
            if (!m_IsPlaying) return;

            ExitCurrentIState();
            m_CurrentState = default;

            m_IsPlaying = false;
        }

        public void SetMaxLastStatesCount(int count)
        {
            if (count < 1)
                Debug.LogError(SET_MAX_LAST_STATES_COUNT);
            else
                m_MaxLastStatesCount = count;
        }

        // Private
        private void SetLastState(string state)
        {
            m_LastIState = m_States[state];
            m_LastState = state;
            AddLastState(state);
        }

        private void SetCurrentState(string state)
        {
            m_CurrentIState = m_States[state];
            m_CurrentState = state;
            EnterCurrentIState();
        }

        private void EnterCurrentIState()
        {
            if (!m_IsPlaying) return;

            m_CurrentIState.Enter();
            //Debug.Log(m_CurrentState + " Enter");
            Debug.Log(m_CurrentIState + " Enter");
        }

        private void ExitCurrentIState()
        {
            if (!m_IsPlaying) return;

            m_CurrentIState.Exit();
            if (m_CurrentIState != null)
                Debug.Log(m_CurrentIState + " Exit");
            //Debug.Log(m_CurrentState + " Exit");
        }

        private void AddLastState(string lastState)
        {
            int maxCount = m_MaxLastStatesCount == -1 ? MAX_LAST_STATES_COUNT : m_MaxLastStatesCount;
            if (m_LastStates.Count + 1 > maxCount)
                m_LastStates.RemoveAt(0);
            m_LastStates.Add(lastState);
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