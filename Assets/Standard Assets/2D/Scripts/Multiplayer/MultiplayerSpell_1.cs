using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;
using UnityEngine.Networking;

public class MultiplayerSpell_1 : NetworkBehaviour
{

    [SerializeField] public int costOfUseSpell;
    [SerializeField] float demage;
    private float _SpellPower = 1F;
    private Animator _Spell_1Animator;
    [SerializeField] public float spell_1Speed = 15F;
    [SerializeField] public string sourceID;
    private float directionCheck;
    private void Start()
    {
        sourceID = transform.parent.GetComponent<NetworkIdentity>().netId.ToString();
        transform.gameObject.name = "Water Ball [Player " + sourceID + "]";
        _SpellPower = transform.parent.GetComponent<MPlayerAttacks>()._SpellPower;
        if (_SpellPower == 4F) _SpellPower = 3f;

        if (GetComponent<Transform>().transform.localScale == new Vector3(-1.0F, 1.0F, 1.0F)) // Fllip Spell (like player diraction)
        {
            directionCheck = -1;
        }
        else directionCheck = 1;

        transform.parent = transform.parent.transform.parent;
        demage = demage * _SpellPower;
        GetComponent<CircleCollider2D>().radius *= _SpellPower / 2;
        _Spell_1Animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (directionCheck == -1) // Fllip Spell (like player diraction)
        {
            GetComponent<Transform>().localScale = new Vector3(-_SpellPower, _SpellPower, _SpellPower);
            GetComponent<Rigidbody2D>().velocity = transform.right * spell_1Speed * (-1.0F);  // ijemna oś "x"
        }
        else if (directionCheck == 1)
        {
            GetComponent<Transform>().localScale = new Vector3(_SpellPower, _SpellPower, _SpellPower);
            GetComponent<Rigidbody2D>().velocity = transform.right * spell_1Speed;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == ("Platform")) // objiekt jest usównay w przypadku dotkniećia z ikoljaderem trtypu platforma
        {
            Destroy(gameObject);
        }
        else if (other.tag == ("Player"))       // w przypadku doknięcia playera spell _1 ma zabierać HP & być uówany. 
        {
            if (other.gameObject.GetComponent<NetworkIdentity>().netId.ToString() != sourceID)
            {
                //other.gameObject.GetComponent<MPlayer>().CmdTake_HP(demage);
                Destroy(gameObject);
            }
        }
    }

}
