namespace Game.StateMachines
{
    public abstract class StateMachine
    {
        public IState currentState = null;

        public virtual void OnUpdate(float deltaTime)
        {
            currentState?.OnUpdate(deltaTime);
        }

        public virtual void SetState(IState state)
        {
            currentState?.OnEnd();
            currentState = state;
            currentState?.OnStart();
        }
    }
}
