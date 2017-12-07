using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(NetworkIdentity))]
public class MPlayerAttacks : NetworkBehaviour
{
    public GameObject waterBall;       // Reference to Spell_1 (water ball)
    public GameObject waterImplosion;  // Reference to Spell_2 (water implosion)

    public Transform diraction;         // Check In which direction the player moves.

    [SyncVar] public float _SpellPower;
    public float _NextSpell { get; private set; }
    private float _Spell_1Rate = 1F;


    [SerializeField] public Collider2D idleAttackCollider;
    [SerializeField] public Collider2D jumpAttackCollider;
    private const float _WeaponAttackCast = 0.5F;
    public bool _CanAttack { get; private set; }

    private void Awake()
    {
        _CanAttack = true;
        idleAttackCollider.enabled = false;
        jumpAttackCollider.enabled = false;
    }

    [Command]
    public void CmdSpell1()
    {
        waterBall.GetComponent<Transform>().localScale = this.GetComponent<Transform>().localScale;
        GetComponent<Animator>().SetTrigger("Shoot");
        _NextSpell = Time.time + _Spell_1Rate;
        GameObject waterBallInstance = Instantiate(waterBall, diraction.position, diraction.rotation, this.GetComponent<Transform>()); //Creating an spell - object clone. Clone inherits from GameMaster class (transform.parent) ;
        GameManager dictionarySpell = new GameManager();
        NetworkServer.Spawn(waterBallInstance);
        RpcGetSpell1Id(waterBallInstance);

        // GetComponent<MPlayer>()._MANA -= waterBall.GetComponent<MultiplayerSpell_1>().costOfUseSpell * _SpellPower;  // Reduce mana points;
    }
    [ClientRpc]
    public void RpcGetSpell1Id(GameObject spell1)
    {
        spell1.GetComponent<MultiplayerSpell_1>().sourceID = GetComponent<NetworkIdentity>().netId.ToString();
    }


    [Client]
    public void Spell1()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        CmdSpell1();
    }


    [Command]
    public void CmdSpell2()
    {
        GetComponent<Animator>().SetTrigger("Cast");
        _NextSpell = Time.time + _Spell_1Rate;
        GameObject waterImplosionInstance = (GameObject)Instantiate(waterImplosion, diraction.position, diraction.rotation); //Creating an spell - object clone. Clone inherits from GameMaster class;
        NetworkServer.Spawn(waterImplosionInstance);
        RpcGetSpell2Id(waterImplosionInstance);

        //  GetComponent<MPlayer>()._MANA -= waterImplosion.GetComponent<MultiplayerSpell_2>().CostOfUseSpell * _SpellPower;  // Reduce mana points;
    }
    [ClientRpc]
    public void RpcGetSpell2Id(GameObject spell2)
    {
        spell2.GetComponent<MultiplayerSpell_2>().sourceID = GetComponent<NetworkIdentity>().netId.ToString();
    }


    [Client]
    public void Spell2()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        CmdSpell2();
    }

    private bool SetIdleAttackColider()
    {
        if (gameObject.GetComponent<MPlayer>()._PullOutSword == true && _CanAttack == true && GetComponent<MPlayerMovement>()._Grounded == true)
        {
            return true;  // activate idle Attacking Collider
        }
        else return false;
    }

    private bool SetJumpAttackColider()
    {
        if (gameObject.GetComponent<MPlayer>()._PullOutSword == true && _CanAttack == true && GetComponent<MPlayerMovement>()._Grounded == false)
        {
            return true;  // activate idle Attacking Collider
        }
        else return false;
    }

    [ClientRpc]
    public void RpcSwordAttack(bool idleActivator, bool jumpActivator)
    {

        idleAttackCollider.enabled = idleActivator;  // activate ilde attack colider
        jumpAttackCollider.enabled = jumpActivator; // activate jump attack colider

        GetComponent<Animator>().SetTrigger("Attacking");
        StartCoroutine(Attack());

    }



    [Command]
    public void CmdSwordAttack(bool idleActivator, bool jumpActivator)
    {
        RpcSwordAttack(idleActivator, jumpActivator);
    }


    IEnumerator Attack()
    {
        _CanAttack = false;
        yield return new WaitForSeconds(_WeaponAttackCast);
        _CanAttack = true;
        idleAttackCollider.enabled = false;
        jumpAttackCollider.enabled = false;
    }


}
