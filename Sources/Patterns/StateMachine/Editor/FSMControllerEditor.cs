using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using GameDevStack.Patterns;

namespace GameDevStack.Patterns
{
    [CustomEditor(typeof(FSMController), true)]
    public class FSMControllerEditor : Editor
    {
        private const string HELPBOX_NULL_LAST_STATES = "Your FSM do not contain last states!";
        private const string HELPBOX_FSM_NULL = "Your FSM is not initialized!";

        private static bool m_DataFlodout = false;
        private static bool m_LastStatesFlodout = false;

        private FSMController m_FSMMonoBehaviour;
        private FSM m_FSM;

        private void OnEnable()
        {
            m_FSMMonoBehaviour = (FSMController)target;
            m_FSM = m_FSMMonoBehaviour.FSM;
        }

        public override void OnInspectorGUI()
        {
            if (Application.isPlaying)
            {
                ShowFSMData();
                EditorGUILayout.Space();
            }
            base.OnInspectorGUI();
        }

        private void ShowFSMData()
        {
            m_DataFlodout = EditorGUILayout.Foldout(m_DataFlodout, "Debug State Machine");
            if (!m_DataFlodout) return;

            if (m_FSM == null)
            {
                EditorGUILayout.HelpBox(HELPBOX_FSM_NULL, MessageType.Warning);
                return;
            }

            GUI.enabled = false;

            EditorGUILayout.Toggle("Is Started", m_FSM.IsStarted);
            EditorGUILayout.Toggle("On Pause", m_FSM.OnPause);

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Initialized States");
            foreach (KeyValuePair<string, IState> kvp in m_FSM.States)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.TextField(kvp.Key.ToString());
                EditorGUILayout.TextField(kvp.Value.ToString());
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.Space();

            EditorGUILayout.TextField("Current Enum State", m_FSM.CurrentState.ToString());
            EditorGUILayout.TextField("Current State", m_FSM.CurrentIState.ToString());

            EditorGUILayout.Space();

            if (m_FSM.TryGetLastState(out string lastState))
                EditorGUILayout.TextField("Last Enum State", lastState.ToString());

            if (m_FSM.LastIState != null)
                EditorGUILayout.TextField("Last State", m_FSM.LastIState.ToString());

            EditorGUILayout.Space();

            ShowLastStates();

            GUI.enabled = true;
        }

        private void ShowLastStates()
        {
            m_LastStatesFlodout = EditorGUILayout.Foldout(m_LastStatesFlodout, "Last States");
            if (!m_LastStatesFlodout) return;

            if (m_FSM.LastStates.Count == 0)
                EditorGUILayout.HelpBox(HELPBOX_NULL_LAST_STATES, MessageType.Info);

            for (int i = m_FSM.LastStates.Count - 1; i >= 0; i--)
            {
                EditorGUILayout.TextField((m_FSM.LastStates.Count - i).ToString(), m_FSM.LastStates[i].ToString());
            }

        }
    }
}