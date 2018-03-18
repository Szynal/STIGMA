using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(NetworkIdentity))]
public class MPlayerAttacks : NetworkBehaviour
{
    /// <summary>
    /// Reference to Spell_1 (water ball)
    /// </summary>
    public GameObject waterBall;
    /// <summary>
    /// Reference to Spell_2 (water implosion)
    /// </summary>
    public GameObject waterImplosion;
    /// <summary>
    /// Check In which direction the player moves.
    /// </summary>
    public Transform diraction;
    /// <summary>
    /// Multiplier power od spells.
    /// </summary>
    public float spellPower = 1F;

    public float _NextSpell { get; private set; }
    /// <summary>
    /// cooldown [s]
    /// </summary>
    private float _Spell_1Rate = 1F;

    /// <summary>
    /// Reference to Collider2D - Idle
    /// </summary>
    [SerializeField] public Collider2D idleAttackCollider;
    /// <summary>
    /// Reference to Collider2D - Jump
    /// </summary>
    [SerializeField] public Collider2D jumpAttackCollider;
    private const float _WeaponAttackCast = 0.5F;
    /// <summary>
    ///  Check if player can attack
    /// </summary>
    public bool _CanAttack { get; private set; }
    /// <summary>
    /// Initialization
    /// </summary>
    private void Awake()
    {
        _CanAttack = true;
        idleAttackCollider.enabled = false;
        jumpAttackCollider.enabled = false;
    }
    /// <summary>
    ///   Send data from clients to the server
    /// </summary>
    [Command]
    public void CmdCastSpell1()
    {
        RpcCastSpell1();
    }
    /// <summary>
    /// Server sends to all clients that spell 1 is created
    /// </summary>
    [ClientRpc]
    public void RpcCastSpell1()
    {
        if (GetComponent<MPlayer>().CanCast() && waterBall.GetComponent<MSpell1>().costOfUseSpell * spellPower <= GetComponent<MPlayer>()._Mana)
        {
            waterBall.GetComponent<Transform>().localScale = this.GetComponent<Transform>().localScale;
            GetComponent<Animator>().SetTrigger("Shoot");
            _NextSpell = Time.time + _Spell_1Rate;
            GameObject waterBallInstance = Instantiate(waterBall, diraction.position, diraction.rotation, this.GetComponent<Transform>()); //Creating an spell - object clone. Clone inherits from GameMaster class (transform.parent) ;
            GetComponent<MPlayer>().TakeMana(waterBallInstance.GetComponent<MSpell1>().costOfUseSpell * spellPower);
        }

    }


    [ClientRpc]
    public void RpcSpell2()
    {
        if (GetComponent<MPlayer>().CanCast() && waterImplosion.GetComponent<MSpell2>().costOfUseSpell * spellPower <= GetComponent<MPlayer>()._Mana)
        {
            GetComponent<Animator>().SetTrigger("Cast");
            _NextSpell = Time.time + _Spell_1Rate;
            GameObject waterImplosionInstance = Instantiate(waterImplosion, diraction.position, diraction.rotation, this.GetComponent<Transform>()); //Creating an spell - object clone. Clone inherits from GameMaster class;
            GetComponent<MPlayer>().TakeMana(waterImplosionInstance.GetComponent<MSpell2>().costOfUseSpell * spellPower);
            //  GetComponent<MPlayer>()._MANA -= waterImplosion.GetComponent<MSpell2>().CostOfUseSpell * _SpellPower;  // Reduce mana points;
        }
    }

    [Command]
    public void CmdSpell2()
    {
        RpcSpell2();
    }
    /// <summary>
    /// Check if player has pulling out sword and can idle - attack  
    /// </summary>
    /// <returns> Activate/deactivate idle Attacking Collider</returns>
    private bool SetIdleAttackColider()
    {
        if (gameObject.GetComponent<MPlayer>()._PullOutSword == true && _CanAttack == true && GetComponent<MPlayerMovement>()._Grounded == true)
        {
            return true;
        }
        else return false;
    }
    /// <summary>
    /// Check if player has pulling out sword and can jump - attack  
    /// </summary>
    /// <returns>Activate/deactivate idle Attacking Collider</returns>
    private bool SetJumpAttackColider()
    {
        if (gameObject.GetComponent<MPlayer>()._PullOutSword == true && _CanAttack == true && GetComponent<MPlayerMovement>()._Grounded == false)
        {
            return true;
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


    public void SwordAttack()
    {
        if (_CanAttack == true && GetComponent<MPlayer>()._PullOutSword == true)
        {
            CmdSwordAttack(SetIdleAttackColider(), SetJumpAttackColider());
        }
    }


    IEnumerator Attack()
    {
        _CanAttack = false;
        yield return new WaitForSeconds(_WeaponAttackCast);
        _CanAttack = true;
        idleAttackCollider.enabled = false;
        jumpAttackCollider.enabled = false;
    }

    [Command]
    public void CmdSetSpellPower(float spellPower)
    {
        RpcSetSpellPower(spellPower);
    }

    [ClientRpc]
    public void RpcSetSpellPower(float spellPower)
    {
        this.spellPower = spellPower;
    }


}
