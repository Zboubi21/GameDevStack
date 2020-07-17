using System.Collections.Generic;
using UnityEngine;

namespace GameDevStack.Patterns
{
    [System.Serializable]
    public class StateMachine
    {
        [Header("Debug")]
        [SerializeField] public string m_CurrentStateString;
        [SerializeField] public string m_LastStateString;

    #region Private Variables
        private List<IState> m_States = null;
        private IState m_CurrentState = null;
        private int m_CurrentStateIndex;
        private IState m_LastState = null;
        private int m_LastStateIndex;
    #endregion

    #region Public Variables
        public List<IState> States => m_States;
        public IState CurrentState => m_CurrentState;
        public int CurrentStateIndex => m_CurrentStateIndex;
        public IState LastState => m_LastState;
        public int LastStateIndex => m_LastStateIndex;
    #endregion

    #region Initialization
        public void AddStates(List<IState> statesAdded)
        {
            if (m_States == null)
                m_States = new List<IState>();
            m_States.AddRange(statesAdded);
        }
    #endregion

    #region Manage States
        public void Start()
        {
            if (m_States != null && m_States.Count != 0)
                ChangeState(0);
        }
        public void Stop()
        {
            if (m_CurrentState != null)
            {
                m_CurrentState.Exit();
                m_CurrentState = null;
            }
        }

        public void ChangeState(int index)
        {
            if (index > m_States.Count - 1) { return; }

            m_CurrentState?.Exit();

            m_LastState = m_CurrentState;
            m_LastStateIndex = m_CurrentStateIndex;
            m_LastStateString = m_CurrentState?.GetType().Name;

            m_CurrentState = m_States[index];
            m_CurrentStateIndex = index;
            m_CurrentState.Enter();
            m_CurrentStateString = m_CurrentState.GetType().Name;
        }
    #endregion

    #region Update States
        public void FixedUpdate() { m_CurrentState?.FixedUpdate(); }
        public void Update() { m_CurrentState?.Update(); }
        public void LateUpdate() { m_CurrentState?.LateUpdate(); }
    #endregion

    #region Get States
        public bool CompareState(int stateIndex)
        {
            return m_States[stateIndex] == m_CurrentState;
        }

        public bool IsLastStateIndex(int index)
        {
            return m_LastState == m_States[index];
        }
    #endregion

    }
}