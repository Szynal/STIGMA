using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;
using System;
using UnityEngine.Networking;

public class MultiplayerSword_DMG : NetworkBehaviour
{
    [SerializeField] private int _MinDMG;
    [SerializeField] private int _MaxDMG;
    System.Random random = new System.Random();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && other.gameObject.GetInstanceID() != transform.parent.gameObject.GetInstanceID())
        {
         //   other.GetComponent<MultiplayerCharacter2D>().CmdTake_HP(random.Next(_MinDMG, _MaxDMG));
        }
    }
}
