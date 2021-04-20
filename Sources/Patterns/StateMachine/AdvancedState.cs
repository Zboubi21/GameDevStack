namespace GameDevStack.Patterns
{
    public class AdvancedState<FSMController>
    {
        protected FSMController m_FSMController;

        public AdvancedState(FSMController fSMController)
        {
            m_FSMController = fSMController;
        }
    }
}