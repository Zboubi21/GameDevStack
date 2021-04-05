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

        private void SetLastState(T state)
        {
            m_LastIState = m_States[state];
            m_LastState = state;
            //m_LastStateString = m_LastState.ToString();
            m_LastStateString = m_LastIState.ToString();
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