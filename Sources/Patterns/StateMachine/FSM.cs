using System.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameDevStack.Patterns
{
    [Serializable]
    public class FSM<T> where T : Enum
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
        private Dictionary<T, IState> m_States = new Dictionary<T, IState>();
        private IState m_CurrentIState = null;
        private T m_CurrentState = default;
        private IState m_LastIState = null;
        private T m_LastState = default;
        private bool m_IsPlaying = false;
        private List<T> m_LastStates = new List<T>();

        /**********
        * Getters *
        **********/
        public Dictionary<T, IState> States => m_States;
        public IState CurrentIState => m_CurrentIState;
        public T CurrentState => m_CurrentState;
        //public T LastState => m_LastIState == null ? default : m_LastState;
        public string CurrentStateString => m_CurrentStateString;
        public IState LastIState => m_LastIState;
        public string LastStateString => m_LastStateString;
        public bool IsPlaying => m_IsPlaying;
        public List<T> LastStates => m_LastStates;
        private int m_MaxLastStatesCount = -1;

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

        /*****************
        * Initialization *
        *****************/
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

        // Pas sûr que ce soit une bonne idée/utile de pouvoir faire ça "en cours de jeu"
        public void OverrideState(T state, IState newState)
        {
            m_States[state] = newState;

            if (CurrentState.ToString() == state.ToString())
                SetCurrentState(state);
            if (TryGetLastState(out T lastState) && lastState.ToString() == state.ToString())
                SetLastState(state);
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

        public void ChangeState(T state)
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
        private void SetLastState(T state)
        {
            m_LastIState = m_States[state];
            m_LastState = state;
            //m_LastStateString = m_LastState.ToString();
            m_LastStateString = m_LastIState.ToString();
            AddLastState(state);
        }

        private void SetCurrentState(T state)
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

        private void AddLastState(T lastState)
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

        /**********
        * Helpers *
        **********/
        public static List<IState> ReplaceState(List<IState> states, IState oldState, IState newState)
        {
            List<IState> newStates = new List<IState>(states);
            for (int i = 0; i < states.Count; i++)
            {
                if (states[i].GetType() == oldState.GetType())
                {
                    newStates[i] = newState;
                    return newStates;
                }
            }
            return newStates;
        }

        public static List<IState> ReplaceState(List<IState> states, T state, IState newState)
        {
            List<IState> newStates = new List<IState>(states);
            List<T> stateEnum = Enum.GetValues(typeof(T)).Cast<T>().ToList();
            int index = stateEnum.IndexOf(state);
            newStates[index] = newState;
            return newStates;
        }
    }

    //public class FSMUtilities<T, U> where U : Enum
    public class FSMUtilities
    {
        public static List<T> ReplaceType<T, U>(List<T> states, U state, T newState) where U : Enum
        {
            List<T> newStates = new List<T>(states);
            List<U> stateEnum = Enum.GetValues(typeof(U)).Cast<U>().ToList();
            int index = stateEnum.IndexOf(state);
            if (index != -1)
                newStates[index] = newState;
            return newStates;
        }
    }
}