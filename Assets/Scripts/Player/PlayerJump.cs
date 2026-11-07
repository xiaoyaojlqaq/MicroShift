using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    PlayerControl control;
    // Start is called before the first frame update
    void Start()
    {
        control = GetComponentInParent<PlayerControl>();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {

        control.isInGround = true;

    }
    private void OnCollisionExit2D(Collision2D collision)
    {

        control.isInGround = false;

    }
}
