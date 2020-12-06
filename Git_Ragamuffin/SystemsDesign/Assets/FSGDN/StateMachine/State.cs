// COMMENT TO SILENCE
#define FSGDN_STATEMACHINE_VERBOSE

namespace FSGDN.StateMachine
{
    [System.Serializable]
    public abstract class State : StateInterface
    {
        public virtual void Execute() { }
        public virtual void PhysicsExecute() { }
        public virtual void PostExecute() { }

        public virtual void OnAnimatorIK(int layerIndex) { }

        public virtual void Initialize()
        {
#if (FSGDN_STATEMACHINE_VERBOSE)
            UnityEngine.Debug.Log(machine.name + "." + GetType().Name + "::" + System.Reflection.MethodBase.GetCurrentMethod().Name + "()");
#endif // FSGDN_STATEMACHINE_VERBOSE
        }

        public virtual void Enter()
        {
#if (FSGDN_STATEMACHINE_VERBOSE)
            UnityEngine.Debug.Log(machine.name + "." + GetType().Name + "::" + System.Reflection.MethodBase.GetCurrentMethod().Name + "()");
#endif // FSGDN_STATEMACHINE_VERBOSE
        }

        public virtual void Exit()
        {
#if (FSGDN_STATEMACHINE_VERBOSE)
            UnityEngine.Debug.Log(machine.name + "." + GetType().Name + "::" + System.Reflection.MethodBase.GetCurrentMethod().Name + "()");
#endif // FSGDN_STATEMACHINE_VERBOSE
        }

        public T GetMachine<T>() where T : MachineInterface
        {
            try
            {
                return (T)machine;
            }
            catch (System.InvalidCastException e)
            {
                if (typeof(T) == typeof(MachineState) || typeof(T).IsSubclassOf(typeof(MachineState)))
                {
                    throw new System.Exception(machine.name + ".GetMachine() cannot return the type you requested!\tYour machine is derived from MachineBehaviour not MachineState!" + e.Message);
                }
                else if (typeof(T) == typeof(MachineBehaviour) || typeof(T).IsSubclassOf(typeof(MachineBehaviour)))
                {
                    throw new System.Exception(machine.name + ".GetMachine() cannot return the type you requested!\tYour machine is derived from MachineState not MachineBehaviour!" + e.Message);
                }
                else
                {
                    throw new System.Exception(machine.name + ".GetMachine() cannot return the type you requested!\n" + e.Message);
                }
            }
        }

        internal MachineInterface machine { get; set; }

        public bool isActive { get { return machine.IsCurrentState(GetType()); } }
    }
}