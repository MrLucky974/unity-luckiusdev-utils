
namespace LuckiusDev.Utils.FSM
{
    public abstract class BaseState
    {
        public string Name { get; private set; }
        public readonly StateMachine stateMachine;

        protected BaseState(string name, StateMachine stateMachine) {
            Name = name;
            this.stateMachine = stateMachine;
        }

        public virtual void Enter() { }
        public virtual void UpdateLogic() { }
        public virtual void UpdatePhysics() { }
        public virtual void Exit() { }
    }
}