Example usage of ByTheTale.StateMachine...

/*
	Create a class that inherits from ByTheTale.StateMachine.MachineBehaviour.

	This class is derived from the MonoBehaviour necessary to create a
	component to attach to a game object.

	You will need to override the abstract method 'AddStates()'.

	Inside this function you add states by state class name (e.g. SimpleState).

	Finally, set the initial state of the machine with a call to 
	'SetInitialState()'.

	Note: the state must be added before setting the initial state!
*/

public class ComponentMachine : ByTheTale.StateMachine.MachineBehaviour
{
    public override void AddStates()
    {
        AddState<SimpleState>();

        SetInitialState<SimpleState>();
    }
}

/*
	An example state class...

	The example state below show cases a simple usage of the state class.

	It derives from State (in the namespace 'ByTheTale.StateMachine').

	You must override the abstract method Execute().
*/

public class SimpleState : ByTheTale.StateMachine.State
{
    public override void Execute()
    {
        if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.Alpha1))
        {
            machine.ChangeState<SimpleState>();
        }
    }
}