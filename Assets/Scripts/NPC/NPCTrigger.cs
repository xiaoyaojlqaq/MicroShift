using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTrigger : MonoBehaviour
{
    NPCControl pCControl;
    // Start is called before the first frame update
    void Start()
    {
        pCControl=GetComponentInParent<NPCControl>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        pCControl.triggeredEnterOB = collision;
        pCControl.triggeredExistOB = null;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        pCControl.triggeredExistOB = collision;
        pCControl.triggeredEnterOB = null;
    }
}
