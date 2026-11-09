using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph;
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
        direction = (Random.Range(-1f, 1f)) >= 0 ? 1 : -1;
        control.animator.SetBool("isRun", true);
        isChangeDir = false;
        coroutine = control.StartCoroutine(Timer());
    }

    public void UpdateState()
    {
        //if (control.DialogueTrigger())
        //{
        //    control.SwitchState(StateType.Dialogue);
        //}
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
            control.transform.localScale = Vector2.one;
        }
        else if(control.transform.position.x > control.runRange[1].position.x)
        {
            isChangeDir = true;
            control.rigidbody.velocity = new Vector2(-control.runSpeed, control.rigidbody.velocity.y);
            control.transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            if (!isChangeDir)
            {
                control.rigidbody.velocity = new Vector2(direction * control.runSpeed, control.rigidbody.velocity.y);
                control.transform.localScale = new Vector3(direction, 1, 1);
            }
        }
    }
}
