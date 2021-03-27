namespace GameDevStack.Patterns
{
    public class AdvancedState<TOne, TTWo> where TOne : FSMMonoBehaviour<TTWo>
    {
        protected TOne m_FSMController;

        public AdvancedState(TOne fSMController)
        {
            m_FSMController = fSMController;
        }
    }
}