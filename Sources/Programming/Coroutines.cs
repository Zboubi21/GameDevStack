using System;
using System.Collections;
using UnityEngine;
using GameDevStack.Patterns;

namespace GameDevStack.Programming
{
    public class Coroutines : SingletonMonoBehaviour<Coroutines>
    {
        public static Coroutine InvokeWithDelay(Action action, float delay)
        {
            return Instance.StartCoroutine(CoroutineDelay(action, delay));
        }

        public static void StopInvoke(Coroutine coroutine)
        {
            if (coroutine != null)
                Instance.StopCoroutine(coroutine);
        }

        private static IEnumerator CoroutineDelay(Action action, float delay)
        {
            yield return new WaitForSeconds(delay);
            action?.Invoke();
        }
    }
}