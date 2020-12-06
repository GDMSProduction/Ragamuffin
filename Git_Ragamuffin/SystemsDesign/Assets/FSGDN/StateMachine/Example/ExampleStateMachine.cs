using UnityEngine;

public class ExampleStateMachine : FSGDN.StateMachine.MachineBehaviour
{
    /// <summary>
    ///  MachineBehaviour::AddStates() is an abstract method that allows 
    ///     for the addition of new states in the machine.
    ///     
    /// Add a state as follows:
    ///     
    ///     AddState<ExampleState1>();
    ///     AddState<ExampleState2>();
    ///     
    /// After adding states you must set the state that the machine will 
    ///     start in (known as the initial state). This is done with the
    ///     following code:
    ///     
    ///             SetInitialState<ExampleState1>();
    /// </summary>
    public override void AddStates()
    {
        AddState<ExampleState1>();
        AddState<ExampleState2>();

        SetInitialState<ExampleState1>();
    }

    [Tooltip("How fast the cube spins.")]
    public float rotationSpeed = 3.37F;

    [Tooltip("Add a button here to switch states.")]
    public UnityEngine.UI.Button switchStates;
}