using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCIdleState : INPCState
{
    NPCControl control;
    Coroutine coroutine;

    public NPCIdleState(NPCControl control)
    {
        this.control = control;
    }
    
    public void EnterState()
    {
        control.animator.SetBool("isRun", false);
        control.rigidbody.velocity = Vector2.zero;
        coroutine = control.StartCoroutine(IdleStateStay());
    }

    public void UpdateState()
    {
        //if(control.DialogueTrigger())
        //{
        //    control.SwitchState(StateType.Dialogue);
        //}
    }

    public void ExitState()
    {
        control.StopCoroutine(coroutine);
    }
    
    IEnumerator IdleStateStay()
    {
        yield return new WaitForSeconds(Random.Range(control.idleTimeRange[0], control.idleTimeRange[1]));
        control.SwitchState(StateType.Run);
    }
}
