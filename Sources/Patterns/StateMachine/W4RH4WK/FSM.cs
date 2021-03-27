// https://gist.github.com/W4RH4WK/5dea8f55532e0526da8b6e60c566c259

using System;
using System.Collections.Generic;
using System.Reflection;

namespace GenericFSM
{
	public class FSM<T> where T : struct, IConvertible
	{

		public T State { get; private set; }

		private const BindingFlags FLAGS = BindingFlags.NonPublic | BindingFlags.Instance;

		private IDictionary<T, MethodInfo> states = new Dictionary<T, MethodInfo>();
        private IDictionary<T, MethodInfo> transitions = new Dictionary<T, MethodInfo>();

        public FSM(T init)
		{
			if (!typeof(T).IsEnum)
			{
				throw new ArgumentException("T must be an enumeration");
			}

			// Cache state and transition functions
			foreach (T value in typeof(T).GetEnumValues())
			{
				var s = GetType().GetMethod(value.ToString() + "State", FLAGS);
				if (s != null)
				{
					states.Add(value, s);
				}

                var t = GetType().GetMethod(value.ToString() + "Transition", FLAGS);
                if (t != null)
                {
                    transitions.Add(value, t);
                }
            }

			State = init;
		}

        public void Transition(T next)
        {
            MethodInfo method;
            if (transitions.TryGetValue(next, out method))
            {
                method.Invoke(this, new object[] { State });
            }

            State = next;
        }

        public void StateDo()
		{
			MethodInfo method;
			if (states.TryGetValue(State, out method))
			{
				method.Invoke(this, null);
			}
		}
	}
}