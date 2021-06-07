namespace GameDevStack.Patterns
{
    public interface IState
    {
        void Enter();
        void Exit();

        void FixedUpdate();
        void Update();
        void LateUpdate();

        System.Enum GetNextState();
    }
}