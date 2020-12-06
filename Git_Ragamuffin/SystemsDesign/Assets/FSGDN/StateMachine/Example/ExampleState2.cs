using UnityEngine;
using FSGDN.StateMachine;

public class ExampleState2 : State
{
    /// <summary>
    /// StateInterface::Execute() is an abstract method where
    ///     the functionality of the state is performed.
    /// </summary>
    public override void Execute()
    {
        ExampleStateMachine exampleStateMachine = GetMachine<ExampleStateMachine>();

        Quaternion rotation = exampleStateMachine.transform.rotation;
        rotation *= Quaternion.Euler(0, -exampleStateMachine.rotationSpeed * Time.deltaTime, 0);
        exampleStateMachine.transform.rotation = rotation;

        Renderer renderer = exampleStateMachine.GetComponentInChildren<Renderer>();
        if (null != renderer)
        {
            renderer.material.color = Color.Lerp(Color.green, Color.cyan, Mathf.PingPong(Time.time, 1));
        }
    }

    public override void Enter()
    {
        base.Enter();

        ExampleStateMachine exampleStateMachine = GetMachine<ExampleStateMachine>();

        UnityEngine.UI.Button switchStatesButton = exampleStateMachine.switchStates;
        if (null != switchStatesButton)
        {
            UnityEngine.UI.Text buttonText = switchStatesButton.GetComponentInChildren<UnityEngine.UI.Text>();
            if (null != buttonText)
            {
                buttonText.text = "Go to\nthe First State!";
            }

            switchStatesButton.onClick.AddListener(GoToExampleState1);
        }
    }

    public override void Exit()
    {
        base.Exit();

        ExampleStateMachine exampleStateMachine = GetMachine<ExampleStateMachine>();

        UnityEngine.UI.Button switchStatesButton = exampleStateMachine.switchStates;
        if (null != switchStatesButton)
        {
            switchStatesButton.onClick.RemoveListener(GoToExampleState1);
        }
    }

    protected void GoToExampleState1()
    {
        ExampleStateMachine exampleStateMachine = GetMachine<ExampleStateMachine>();

        exampleStateMachine.ChangeState<ExampleState1>();
    }
}