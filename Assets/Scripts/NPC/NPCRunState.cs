using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCRunState : INPCState
{
    NPCControl control;
    public int direction;
    Coroutine coroutine;
    bool isChangeDir;
    public NPCRunState(NPCControl control)
    {
        this.control = control;
    }

    public void EnterState()
    {
        isChangeDir = false;
        coroutine = control.StartCoroutine(Timer());
        direction = ((float)Random.Range(-2, 2)) > 0 ? 1 : -1;
    }

    public void UpdateState()
    {
        if (control.DialogueTrigger())
        {
            control.SwitchState(StateType.Dialogue);
        }
        Run();
    }

    public void ExitState()
    {
        control.rigidbody.velocity = Vector2.zero;
        control.StopCoroutine(coroutine);
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(Random.Range(control.runTimeRange[0], control.runTimeRange[1]));
        control.SwitchState(StateType.Idle);
    }

    void Run()
    {
        if(control.transform.position.x < control.runRange[0].position.x)
        {
            isChangeDir = true;
            control.rigidbody.velocity = new Vector2(control.runSpeed, control.rigidbody.velocity.y);
        }
        else if(control.transform.position.x > control.runRange[1].position.x)
        {
            isChangeDir = true;
            control.rigidbody.velocity = new Vector2(-control.runSpeed, control.rigidbody.velocity.y);
        }
        else
        {
            if (!isChangeDir)
            {
                control.rigidbody.velocity = new Vector2(direction * control.runSpeed, control.rigidbody.velocity.y);
            }
        }
    }
}
