using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;

public class Spell_2 : MonoBehaviour
{

    private Animator _Spell_2_Animator;
    public GameMaster GM;
    public GameObject gameMaster;

    [SerializeField] int numberOfPlayer;
    [SerializeField] public int costOfUseSpell;
    [SerializeField] float demage;
    private float _SpellPower = 1F;

    private void Start()
    {
        gameMaster = transform.parent.gameObject;
        GM = gameMaster.GetComponent<GameMaster>();
        for (int i = 0; i < GM.GetComponent<GameMaster>().listSpell_2.Count; i++)
        {
            numberOfPlayer = GM.GetComponent<GameMaster>().listSpell_2[i];
            GM.GetComponent<GameMaster>().listSpell_2.RemoveAt(i);
        }

        _SpellPower = GM.transform.GetChild(numberOfPlayer).GetComponent<PlatformerCharacter2D>().spellPower;
        if (_SpellPower == 4F) _SpellPower = 3f;

        demage = demage * _SpellPower;
        GetComponent<Transform>().localScale = new Vector3(_SpellPower, _SpellPower, _SpellPower);
        GetComponent<CircleCollider2D>().radius *= _SpellPower / 2;

        StartCoroutine(Destroy());
    }
    private void Awake()
    {
        _Spell_2_Animator = GetComponent<Animator>();
    }
    private void Update()
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
        PlatformerCharacter2D player = other.collider.GetComponent<PlatformerCharacter2D>();
        if (GetComponent<Spell_2>().numberOfPlayer != player.GetComponent<PlatformerCharacter2D>().numberOfPlayer)
        {
            player.take_HP(demage);
        }
        if (GetComponent<Spell_2>().numberOfPlayer == player.GetComponent<PlatformerCharacter2D>().numberOfPlayer)
        {
            Physics2D.IgnoreCollision(other.collider, GetComponent<Collider2D>(), true);
        }
    }
}
