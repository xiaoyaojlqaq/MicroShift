using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTrigger : MonoBehaviour
{
    PlayerControl playerControl;
    Action<InputAction.CallbackContext> action;
    // Start is called before the first frame update
    void Start()
    {
        playerControl = GetComponentInParent<PlayerControl>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("NPC") && !playerControl.isEnterDialogue)
        {
            Debug.Log("enter");
            playerControl.isEnterDialogue = true;
            NPCControl nPCControl = collision.GetComponentInParent<NPCControl>();
            DialogueManager.InitLocatization(nPCControl.localizationTable[nPCControl.currLocalizationTable], playerControl, nPCControl);
            nPCControl.SwitchState(StateType.Dialogue);
            nPCControl.isEnterDialogue = true;
            action = (InputAction.CallbackContext ctx) =>
            {
                DialogueManager.DialogueShow(nPCControl.localizationTable[nPCControl.currLocalizationTable], playerControl, nPCControl);
            };
            InputManager.Instance.InteractPerformed += action;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("NPC") && playerControl.isEnterDialogue)
        {
            collision.GetComponentInParent<NPCControl>().isEnterDialogue = false;
            playerControl.isEnterDialogue = false;
            InputManager.Instance.InteractPerformed -= action;
        }
    }
}
