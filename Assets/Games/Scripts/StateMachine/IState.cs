namespace Game.StateMachines
{
    public interface IState
    {
        void OnStart();
        void OnUpdate(float deltaTime);
        void OnEnd();
    }
}