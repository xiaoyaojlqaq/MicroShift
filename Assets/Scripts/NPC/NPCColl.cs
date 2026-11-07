using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCColl : MonoBehaviour
{
    NPCControl pCControl;
    // Start is called before the first frame update
    void Start()
    {
        pCControl = GetComponentInParent<NPCControl>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        pCControl.colledEnterOB = collision;
        pCControl.colledExistOB = null;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        pCControl.colledExistOB = collision;
        pCControl.colledEnterOB = null;
    }
}
