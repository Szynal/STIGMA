using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets._2D;

public class MSpell2 : NetworkBehaviour
{

    private Animator _spell_2_Animator;

    [SerializeField] public int CostOfUseSpell;
    [SerializeField] float Demage;
    public string sourceID;
    private float _spellPower = 1F;

    private void Start()
    {
        sourceID = transform.parent.GetComponent<NetworkIdentity>().netId.ToString();
        _spell_2_Animator = GetComponent<Animator>();
        // _spellPower = transform.parent.GetComponent<MPlayerAttacks>()._SpellPower;
        transform.gameObject.name = "Water Imposion [Player " + sourceID + "]";
        transform.parent = transform.parent.transform.parent;
        if (_spellPower == 4F) _spellPower = 3f;

        Demage = Demage * _spellPower;
        GetComponent<Transform>().localScale = new Vector3(_spellPower, _spellPower, _spellPower);
        GetComponent<CircleCollider2D>().radius *= _spellPower / 2;
        StartCoroutine(Destroy());
    }
        

    private void Update()
    {
        CmdCastSpell_2();
    }


    [Command]
    public void CmdCastSpell_2()
    {
        RpcCastSpell_2();
    }

    [ClientRpc]
    public void RpcCastSpell_2()
    {
        StartCoroutine(CastSpell_2());
    }

    IEnumerator CastSpell_2()
    {
        yield return new WaitForSeconds(0.1F);
        gameObject.GetComponent<CircleCollider2D>().radius += 0.04F;
    }



    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(1.0F);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        MPlayer player = other.collider.GetComponent<MPlayer>();

        if (sourceID != player.GetComponent<MPlayer>().netId.ToString())
        {
            //player.CmdTake_HP(Demage);
        }
        if (sourceID == player.GetComponent<MPlayer>().netId.ToString())
        {
            Physics2D.IgnoreCollision(other.collider, GetComponent<Collider2D>(), true);
        }

    }
}
