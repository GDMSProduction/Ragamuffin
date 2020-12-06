namespace FSGDN.StateMachine
{
    public interface StateInterface
    {
        void Initialize();

        void Enter();

        void Execute();
        void PhysicsExecute();
        void PostExecute();

        void Exit();

        void OnAnimatorIK(int layerIndex);

        bool isActive { get; }

        T GetMachine<T>() where T : MachineInterface;
    }
}