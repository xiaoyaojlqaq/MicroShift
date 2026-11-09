using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.InputSystem;

public class NPCDialogueState : INPCState
{

    NPCControl control;
    public NPCDialogueState(NPCControl control)
    {
        this.control = control;
    }

    public void EnterState()
    {
        control.rigidbody.velocity = Vector2.zero;
        control.WaitDialogueImage.SetActive(true);
        //control.DialogueImage.SetActive(false);
        InputManager.Instance.InteractPerformed += WaitForDialogue;
    }

    public void UpdateState()
    {
        //if (control.triggeredExistOB != null && control.triggeredExistOB.CompareTag("Player"))
        //{
        //    control.SwitchState(StateType.Idle);
        //}
        if (!control.isEnterDialogue)
        {
            control.SwitchState(StateType.Idle);
        }
    }

    public void ExitState()
    {
        control.WaitDialogueImage.SetActive(false);
        control.DialogueImage.SetActive(false);
        control.isEnterDialogue = false;
        InputManager.Instance.InteractPerformed -= WaitForDialogue;
    }

    void WaitForDialogue(InputAction.CallbackContext ctx)
    {
        //bool isExDia = false;
        //isExDia = !isExDia;
        control.WaitDialogueImage.SetActive(false);
        //control.DialogueImage.SetActive(true);
        //if (!isExDia)
        //{
        //    control.SwitchState(StateType.Idle);
        //}
    }
}
