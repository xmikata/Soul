using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Action
{
    Null,
    RollF,
    RollB,
    RollL,
    RollR,
    RollFR,
    RollFL,
    RollBR,
    RollBL,
    StepBack,
    Jump
}
public class InputBuffer : MonoBehaviour
{
    public static InputBuffer singleton;

    private float isInteractingTime;
    public float saveStartTime;
    public InputHandler inputHandler;
    public Action nextAction;
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        Timing(Time.unscaledDeltaTime);
        DetectInputs();
    }

    public void Initialize()
    {
        if (InputBuffer.singleton==null)
        {
            InputBuffer.singleton = this;
        }
        nextAction = Action.Null;
        //playerManager = this.GetComponent<PlayerManager>();
        inputHandler = this.GetComponent<InputHandler>();
        //playerMove = this.GetComponent<PlayerMoveScript>();
    }
    public void Timing(float delta)
    {
        
    }

    public void DetectInputs()
    {
        if (Input.GetKeyUp(KeyCode.Space)&&nextAction==Action.Null&&isInteractingTime>=saveStartTime)
        {
            if (Input.GetKey(KeyCode.W)|| Input.GetKey(KeyCode.A)|| Input.GetKey(KeyCode.S)|| Input.GetKey(KeyCode.D))
            {
                if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A)) nextAction = Action.RollFL;
                else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))nextAction = Action.RollFR;
                else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A)) nextAction = Action.RollBL;
                else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D)) nextAction = Action.RollBR;
                else if (Input.GetKey(KeyCode.W)) nextAction = Action.RollF;
                else if (Input.GetKey(KeyCode.A)) nextAction = Action.RollL;
                else if (Input.GetKey(KeyCode.S)) nextAction = Action.RollB;
                else if (Input.GetKey(KeyCode.D)) nextAction = Action.RollR;
            }
            else
            {
                nextAction = Action.StepBack;
            }
        }
    }
}
