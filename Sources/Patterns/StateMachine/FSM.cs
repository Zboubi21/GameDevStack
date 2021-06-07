﻿using System.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameDevStack.Patterns
{
    /*******
    * Enum *
    *******/
    public enum FSMUpdateType
    {
        BeginingFixedUpdate,
        EndingFixedUpdate,
        BeginingUpdate,
        EndingUpdate,
        BeginingLateUpdate,
        EndingLateUpdate
    }

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
        private IState m_LastIState = null;

        private string m_DefaultState = null;
        private string m_CurrentState = null;
        private string m_LastState = null;
        private List<string> m_LastStates = new List<string>();

        private bool m_IsStarted = false;
        private bool m_OnPause = false;

        private FSMUpdateType m_UpdateType;

        /**********
        * Getters *
        **********/
        public Dictionary<string, IState> States => m_States;

        public IState CurrentIState => m_CurrentIState;
        public IState LastIState => m_LastIState;

        public string CurrentState => m_CurrentState;
        //public string LastState => m_LastIState == null ? default : m_LastState;
        //public string? LastState => m_LastIState == null ? null : m_LastState;
        public List<string> LastStates => m_LastStates;
        private int m_MaxLastStatesCount = -1;

        public bool IsStarted => m_IsStarted;
        public bool OnPause => m_OnPause;

        public bool TryGetLastState(out string lastState)
        {
            if (m_LastIState == null)
            {
                lastState = null;
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
        public FSM(List<IState> statesAdded, Enum defaultState, FSMUpdateType updateType)
        {
            string defaultStateName = defaultState.ToString();
            Array array = defaultState.GetType().GetEnumValues();
            List<string> states = System.Enum.GetValues(defaultState.GetType()).Cast<Enum>().ToList().ConvertAll(e => e.ToString());
            states.RemoveAt(0); // Removing the first state because it is just used to check null Enum

            if (states.Count != statesAdded.Count)
                Debug.LogError(STATE_COUNT_ERROR);

            for (int i = 0; i < states.Count; i++)
            {
                m_States.Add(states[i], statesAdded[i]);
            }

            m_UpdateType = updateType;

            m_DefaultState = defaultStateName;
            SetCurrentState(defaultStateName);
        }

        /*******
        * Core *
        *******/
        // Public API
        public void Start()
        {
            if (m_IsStarted) return;
            m_IsStarted = true;
            m_OnPause = false;

            EnterCurrentIState();
        }

        public void Stop()
        {
            if (!m_IsStarted) return;

            ExitCurrentIState();
            m_CurrentState = m_DefaultState;

            m_IsStarted = false;
            m_OnPause = false;
        }

        public void ChangeState(Enum state)
        {
            if (!m_IsStarted || m_OnPause) return;
            ExitCurrentIState();
            SetLastState(m_CurrentState);
            SetCurrentState(state.ToString());
        }

        public void Play()
        {
            if (!m_IsStarted || !m_OnPause) return;
            m_OnPause = false;
        }

        public void Pause()
        {
            if (!m_IsStarted || m_OnPause) return;
            m_OnPause = true;
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
            if (!m_IsStarted) return;

            m_CurrentIState.Enter();
            //Debug.Log(m_CurrentState + " Enter");
            Debug.Log(m_CurrentIState + " Enter");
        }

        private void ExitCurrentIState()
        {
            if (!m_IsStarted) return;

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
            if (!m_IsStarted || m_OnPause) return;

            if (m_UpdateType == FSMUpdateType.BeginingFixedUpdate)
                CheckChangeStateUpdate();

            m_CurrentIState?.FixedUpdate();

            if (m_UpdateType == FSMUpdateType.EndingFixedUpdate)
                CheckChangeStateUpdate();
        }
        public void Update()
        {
            if (!m_IsStarted || m_OnPause) return;

            if (m_UpdateType == FSMUpdateType.BeginingUpdate)
                CheckChangeStateUpdate();

            m_CurrentIState?.Update();

            if (m_UpdateType == FSMUpdateType.EndingUpdate)
                CheckChangeStateUpdate();
        }
        public void LateUpdate()
        {
            if (!m_IsStarted || m_OnPause) return;

            if (m_UpdateType == FSMUpdateType.BeginingLateUpdate)
                CheckChangeStateUpdate();

            m_CurrentIState?.LateUpdate();

            if (m_UpdateType == FSMUpdateType.EndingLateUpdate)
                CheckChangeStateUpdate();
        }

        private void CheckChangeStateUpdate()
        {
            if (!m_IsStarted || m_OnPause) return;

            Enum myEnum = m_CurrentIState.GetNextState();
            if (Convert.ToInt32(myEnum) != 0 )
                ChangeState(myEnum);
        }
    }
}