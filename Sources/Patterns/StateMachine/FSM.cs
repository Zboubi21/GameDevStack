using System.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameDevStack.Patterns
{
    [Serializable]
    public class FSM<Enum>
    {
        /************
        * Constants *
        ************/
        private const string STATE_COUNT_ERROR = "You do not have the same state enum count and state script count!";
        private const int MAX_LAST_STATES_COUNT = 10;
        private const string SET_MAX_LAST_STATES_COUNT = "You cannot set a count < 1!";

        /*****************
        * SerializeField *
        *****************/
        [Header("Debug")]
        [SerializeField] private string m_CurrentStateString;
        [SerializeField] private string m_LastStateString;

        /**********
        * Private *
        **********/
        private Dictionary<Enum, IState> m_States = new Dictionary<Enum, IState>();
        private IState m_CurrentIState = null;
        private Enum m_CurrentState = default;
        private IState m_LastIState = null;
        private Enum m_LastState = default;
        private bool m_IsPlaying = false;
        private List<Enum> m_LastStates = new List<Enum>();

        /**********
        * Getters *
        **********/
        public Dictionary<Enum, IState> States => m_States;
        public IState CurrentIState => m_CurrentIState;
        public Enum CurrentState => m_CurrentState;
        //public Enum LastState => m_LastIState == null ? default : m_LastState;
        public string CurrentStateString => m_CurrentStateString;
        public IState LastIState => m_LastIState;
        public string LastStateString => m_LastStateString;
        public bool IsPlaying => m_IsPlaying;
        public List<Enum> LastStates => m_LastStates;
        private int m_MaxLastStatesCount = -1;

        public bool TryGetLastState(out Enum lastState)
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
            List<Enum> stateEnum = System.Enum.GetValues(typeof(Enum)).Cast<Enum>().ToList();

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
            SetCurrentState(state);
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
        private void SetLastState(Enum state)
        {
            m_LastIState = m_States[state];
            m_LastState = state;
            //m_LastStateString = m_LastState.ToString();
            m_LastStateString = m_LastIState.ToString();
            AddLastState(state);
        }

        private void SetCurrentState(Enum state)
        {
            m_CurrentIState = m_States[state];
            m_CurrentState = state;
            //m_CurrentStateString = m_CurrentState.ToString();
            m_CurrentStateString = m_CurrentIState.ToString();
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

        private void AddLastState(Enum lastState)
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