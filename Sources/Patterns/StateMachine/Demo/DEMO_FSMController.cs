using System.Collections.Generic;
using UnityEngine;
using GameDevStack.Patterns;

namespace GameDevStack.Demos
{
    public enum State { Idle, Move }

    public class DEMO_FSMController : FSMController
    {
        [Space]
        [SerializeField] private bool m_PlayingAtStart = false;
        [SerializeField] private KeyCode m_IdleKeyCode = default;
        [SerializeField] private KeyCode m_MoveKeyCode = default;
        [Space]
        [SerializeField] private bool m_IsMoving = false;

        public bool IsMoving => m_IsMoving;

        protected virtual void Awake()
        {
            InitializeFSM(new List<IState>
            {
                new DEMO_IdleState(this),
                new DEMO_MoveState(this)
            }, State.Idle, m_PlayingAtStart);
        }

        protected virtual void Start()
        {
            if (m_PlayingAtStart)
                StartFSM();
        }

        protected override void Update()
        {
            base.Update();

            if (Input.GetKeyDown(m_IdleKeyCode))
                ChangeState(State.Idle);
            if (Input.GetKeyDown(m_MoveKeyCode))
                ChangeState(State.Move);
        }

        [ContextMenu("Start SM")]
        private void StartSM()
        {
            m_FSM.Start();
        }

        [ContextMenu("Stop SM")]
        private void StopSM()
        {
            m_FSM.Stop();
        }
    }
}