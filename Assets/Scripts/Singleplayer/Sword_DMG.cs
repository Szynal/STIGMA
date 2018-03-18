using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;
using System;

public class Sword_DMG : MonoBehaviour
{
    [SerializeField] private int _MinDMG;
    [SerializeField] private int _MaxDMG;
    System.Random random = new System.Random();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && other.GetComponent<PlatformerCharacter2D>().numberOfPlayer != transform.parent.GetComponent<PlatformerCharacter2D>().numberOfPlayer)
        {
            other.GetComponent<PlatformerCharacter2D>().take_HP(random.Next(_MinDMG, _MaxDMG));
        }
    }
}
