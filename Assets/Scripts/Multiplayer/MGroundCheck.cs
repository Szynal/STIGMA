using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MGroundCheck : MonoBehaviour {

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Platform")
        {
            transform.parent.GetComponent<MPlayerMovement>()._Grounded = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Platform")
        {
            transform.parent.GetComponent<MPlayerMovement>()._Grounded = false;
            transform.parent.GetComponent<MPlayerMovement>()._JumpCounter = 0;
        }
    }

   
}
