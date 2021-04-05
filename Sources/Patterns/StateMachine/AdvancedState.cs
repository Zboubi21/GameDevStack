using System;

namespace GameDevStack.Patterns
{
    public class AdvancedState<T, U> where T : FSMMonoBehaviour<U> where U : Enum
    {
        protected T m_FSMController;

        public AdvancedState(T fSMController)
        {
            m_FSMController = fSMController;
        }
    }
}