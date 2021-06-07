using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace GameDevStack.Patterns
{
    public static class IStateExtensions
    {
        public static void ReplaceState<IState, Enum>(this List<IState> states, Enum targetState, IState newState)
        {
            List<Enum> stateEnum = System.Enum.GetValues(typeof(Enum)).Cast<Enum>().ToList();
            states.ReplaceIState(targetState, newState, stateEnum);
        }

        public static void ReplaceStates<IState, Enum>(this List<IState> states, List<Enum> targetStates, List<IState> newStates)
        {
            if (targetStates.Count != newStates.Count)
                Debug.LogError("The targetStates.Count and the newStates.Count are not the same!");

            if (states.Count < targetStates.Count)
                Debug.LogError("The states.Count can't be smaller than targetStates.Count and newStates.Count!");

            List<Enum> stateEnum = System.Enum.GetValues(typeof(Enum)).Cast<Enum>().ToList();

            for (int i = 0, l = targetStates.Count; i < l; i++)
            {
                states.ReplaceIState(targetStates[i], newStates[i], stateEnum);
            }
        }

        private static void ReplaceIState<IState, Enum>(this List<IState> states, Enum targetState, IState newState, List<Enum> stateEnum)
        {
            int index = stateEnum.IndexOf(targetState) - 1; // Removed the Enum: None
            states[index] = newState;
        }
    }
}