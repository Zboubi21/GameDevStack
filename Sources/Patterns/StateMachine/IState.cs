namespace GameDevStack.Patterns
{
    public interface IState
    {
        bool CanEnter();
        bool CanExit();

        void Enter();
        void Exit();

        void FixedUpdate();
        void Update();
        void LateUpdate();

        //bool TryGetNextState(out string state);

        //void GetNextFixedState();
        //void GetNextState();
        //void GetNextLateState();
    }
}