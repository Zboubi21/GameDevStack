using System;

namespace GameDevStack.Patterns
{
    public class AdvancedState<T, Enum> where T : FSMMonoBehaviour<Enum>
    {
        protected T m_FSMController;

        public AdvancedState(T fSMController)
        {
            m_FSMController = fSMController;
        }
    }
}