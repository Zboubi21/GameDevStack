using System.Collections.Generic;
using UnityEngine;
using GameDevStack.Patterns;

namespace GameDevStack.Demos
{
    public enum State { None, Idle, Move }

    public class DEMO_FSMController : FSMController
    {
        [Space]
        [SerializeField] private bool m_PlayingAtInitialization = false;
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
            }, State.Idle, FSMUpdateType.EndingLateUpdate, m_PlayingAtInitialization);
        }

        protected override void Update()
        {
            base.Update();

            if (Input.GetKeyDown(m_IdleKeyCode))
                ChangeState(State.Idle);
            if (Input.GetKeyDown(m_MoveKeyCode))
                ChangeState(State.Move);
        }
    }
}