using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;

public class PlayZone_Function : MonoBehaviour
{

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == ("Player"))
        {
            PlatformerCharacter2D player = other.GetComponent<PlatformerCharacter2D>();
            player.take_HP(1000);
        }
        else if (other.tag == "Spells")
        {
            Destroy(other.gameObject);
        }


    }
}
