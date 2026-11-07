using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCControl : MonoBehaviour
{
    public float runSpeed;
    public float jumpForce;
    public GameObject WaitDialogueImage;
    public GameObject DialogueImage;
    [HideInInspector]public Rigidbody2D rigidbody;
    public Collider2D coll;
    public Collider2D trigger;
    Dictionary<StateType, INPCState> states;
    INPCState currState;

    public float[] idleTimeRange;
    public float[] runTimeRange;
    public Transform[] runRange;


    [HideInInspector]public Collision2D colledEnterOB;
    [HideInInspector]public Collision2D colledExistOB;
    [HideInInspector]public Collider2D triggeredEnterOB;
    [HideInInspector]public Collider2D triggeredExistOB;
    private void Awake()
    {
        states = new Dictionary<StateType, INPCState>()
        {
            {StateType.Idle, new NPCIdleState(this)},
            {StateType.Run, new NPCRunState(this)},
            {StateType.Dialogue,new NPCDialogueState(this) }
        };

        rigidbody = GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        currState = states[StateType.Idle];
        currState.EnterState();
    }

    // Update is called once per frame
    void Update()
    {
        DialogueTrigger();
        currState.UpdateState();
    }

    public void SwitchState(StateType newState)
    {
        currState.ExitState();
        currState = states[newState];
        currState.EnterState();
    }

    public bool DialogueTrigger()
    {
        if (triggeredEnterOB != null && triggeredEnterOB.CompareTag("Player"))
        {
            return true;
        }
        return false;
    }
}
public enum StateType
{
    Idle,

    Run,

    Dialogue,
}