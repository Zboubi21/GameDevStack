using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameDevStack.Patterns
{
    public class FSMController : MonoBehaviour
    {
        /******************
        * Protected Field *
        ******************/
        protected FSM m_FSM = null;

        /*********
        * Getter *
        *********/
        public FSM FSM => m_FSM;

        /*****************
        * Initialization *
        *****************/
        protected virtual void InitializeFSM(List<IState> states, Enum defaultState, FSMUpdateType updateType = FSMUpdateType.EndingLateUpdate, bool playingAtInitialization = true)
        {
            m_FSM = new FSM(states, defaultState, updateType);

            if (playingAtInitialization)
                m_FSM.Start();
        }

        /*************
        * Public API *
        *************/
        [ContextMenu("Start")]
        public void StartFSM() => m_FSM.Start();
        [ContextMenu("Stop")]
        public void StopFSM() => m_FSM.Stop();

        [ContextMenu("Play")]
        public void PlayFSM() => m_FSM.Play();
        [ContextMenu("Pause")]
        public void PauseFSM() => m_FSM.Pause();

        public void ChangeState(Enum state) => m_FSM.ChangeState(state);

        /*********
        * Update *
        *********/
        protected virtual void FixedUpdate() => m_FSM.FixedUpdate();
        protected virtual void Update() => m_FSM.Update();
        protected virtual void LateUpdate() => m_FSM.LateUpdate();
    }
}