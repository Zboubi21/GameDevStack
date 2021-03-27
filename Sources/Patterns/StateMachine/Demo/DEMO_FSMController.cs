using System.Collections.Generic;
using UnityEngine;
using GameDevStack.Patterns;

namespace GameDevStack.Demos
{
    public enum State { Idle, Move }

    public class DEMO_FSMController : FSMMonoBehaviour<State>
    {
        [Space]
        [SerializeField] private KeyCode m_IdleKeyCode = default;
        [SerializeField] private KeyCode m_MoveKeyCode = default;

        private void Awake()
        {
            SetupStateMachine(new List<IState>
            {
                new DEMO_IdleState(this),
                new DEMO_MoveState(this)
            }, State.Move);

            CheckLastState();
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

        [ContextMenu("Check Last State")]
        private void CheckLastState()
        {
            if (m_FSM.TryGetLastState(out State lastState))
            {
                Debug.Log("TRUE: last state = " + lastState);
            }
            else
            {
                Debug.Log("FALSE: last state = " + lastState);
            }
        }
    }
}