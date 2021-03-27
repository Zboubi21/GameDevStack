namespace GameDevStack.Patterns
{
    public interface IState
    {
        void Enter();
        void FixedUpdate();
        void Update();
        void LateUpdate();
        void Exit();
    }
}