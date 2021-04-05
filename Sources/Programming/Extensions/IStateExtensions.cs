using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace GameDevStack.Patterns
{
    public static class IStateExtensions
    {
        public static void ReplaceState<IState, T>(this List<IState> states, T targetState, IState newState) where T : Enum
        {
            List<T> stateEnum = Enum.GetValues(typeof(T)).Cast<T>().ToList();
            states.ReplaceIState(targetState, newState, stateEnum);
        }

        public static void ReplaceStates<IState, T>(this List<IState> states, List<T> targetStates, List<IState> newStates) where T : Enum
        {
            if (targetStates.Count != newStates.Count)
                Debug.LogError("The targetStates.Count and the newStates.Count are not the same!");

            if (states.Count < targetStates.Count)
                Debug.LogError("The states.Count can't be smaller than targetStates.Count and newStates.Count!");

            List<T> stateEnum = Enum.GetValues(typeof(T)).Cast<T>().ToList();

            for (int i = 0, l = targetStates.Count; i < l; i++)
            {
                states.ReplaceIState(targetStates[i], newStates[i], stateEnum);
            }
        }

        private static void ReplaceIState<IState, T>(this List<IState> states, T targetState, IState newState, List<T> stateEnum) where T : Enum
        {
            int index = stateEnum.IndexOf(targetState);
            states[index] = newState;
        }
    }
}