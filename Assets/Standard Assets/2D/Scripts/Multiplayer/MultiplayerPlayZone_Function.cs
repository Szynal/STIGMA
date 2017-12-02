using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;

public class MultiplayerPlayZone_Function : MonoBehaviour
{

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == ("Player"))
        {
            MultiplayerCharacter2D player = other.GetComponent<MultiplayerCharacter2D>();
           // player.CmdTake_HP(1000);
        }
        else if (other.tag == "Spells")
        {
            Destroy(other.gameObject);
        }


    }
}
