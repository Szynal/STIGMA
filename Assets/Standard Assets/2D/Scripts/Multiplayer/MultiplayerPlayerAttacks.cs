using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;
using UnityEngine.Networking;

[RequireComponent(typeof(MultiplayerCharacter2D))]
public class MultiplayerPlayerAttacks : NetworkBehaviour
{
    [SerializeField] public Collider2D idleAttackCollider;
    [SerializeField] public Collider2D jumpAttackCollider;
    [SerializeField] public Animator animator;
    [SerializeField] private Transform _GroundCheck;
    [SerializeField] private LayerMask _WhatIsGround;                  // A mask determining what is ground to the character

    private Animation _Animation;

    private const float _AttackCast = 0.5F;
    const float _GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
    const float _CeilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up

    public bool _CanAttack { get; private set; }
    private bool _Grounded;            // Whether or not the player is grounded.

    private void Start()
    {
        _CanAttack = true;
        _GroundCheck = transform.Find("GroundCheck");
        _Animation = gameObject.GetComponent<Animation>();
    }

    private void Awake()
    {
        idleAttackCollider.enabled = false;
        jumpAttackCollider.enabled = false;
    }

    private void FixedUpdate()
    {
        _Grounded = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_GroundCheck.position, _GroundedRadius, _WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                _Grounded = true;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (_CanAttack == true)
        {
            idleAttackCollider.enabled = false;
            jumpAttackCollider.enabled = false;
        }
    }

    public void Attack()
    {
        if (_Grounded == true) idleAttackCollider.enabled = true;  // activate idle Attacking Collider
        if (_Grounded == false) jumpAttackCollider.enabled = true; // activate jump attack colider
        animator.GetComponent<Animator>().SetTrigger("Attacking");

        StartCoroutine(CanAttack());
    }
    IEnumerator CanAttack()
    {
        _CanAttack = false;
        yield return new WaitForSeconds(_AttackCast);
        _CanAttack = true;
    }

}
