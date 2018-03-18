using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(MPlayerMovement))]
public class MPlayerController : MonoBehaviour
{
    [SerializeField] private String _Jump;              //   INPUT 
    [SerializeField] private String _GetAxis;           //   INPUT  
    [SerializeField] private String _PullOutWeapon;     //   INPUT  
    [SerializeField] private String _Skill1;            //   INPUT 
    [SerializeField] private String _Skill2;            //   INPUT 
    [SerializeField] private String _WeaponAttack;      //   INPUT 
    [SerializeField] private String _SpellPower_1;       //   INPUT 
    [SerializeField] private String _SpellPower_2;       //   INPUT 
    [SerializeField] private String _SpellPower_3;       //   INPUT 
    [SerializeField] private float _Spell_1_CD;
    [SerializeField] private float _Spell_2_CD;
    [SerializeField] public float _JumpForce;

    // Component caching
    private MPlayerMovement _Movment;

    private bool _Can_Cast_Spell_1 = true;
    private bool _Can_Cast_Spell_2 = true;


    void Start()
    {
        _Movment = GetComponent<MPlayerMovement>();
    }

    void Update()
    {
        /*
        if (PauseMenu.IsOn)
        {
            if (Cursor.lockState != CursorLockMode.None)
                Cursor.lockState = CursorLockMode.None;

            _Movment.Move(Vector3.zero);
            _Movment.Rotate(Vector3.zero);
            _Movment.RotateCamera(0f);

            return;
        }
       
        if (Cursor.lockState != CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
         */

        float xMovement = Input.GetAxis(_GetAxis);

        //Apply movement
        _Movment.Move(xMovement);

        Vector2 jumpVector = Vector2.zero;
        if (Input.GetButtonDown(_Jump))
        {
            jumpVector = Vector2.up * _JumpForce;

        }
        _Movment.ApplyJump(jumpVector);

        if (Input.GetButtonDown(_PullOutWeapon))
        {
            gameObject.GetComponent<MPlayer>().PullOutSword();
        }

        //     do zrobienia cooldown przy spellach 1,2 ect....
        if (Input.GetButtonDown(_Skill1))
        {
            if (_Can_Cast_Spell_1)
            {
                GetComponent<MPlayerAttacks>().CmdCastSpell1();
                StartCoroutine(CD_Spell1());
            }
        }

        if (Input.GetButtonDown(_Skill2))
        {
            if (_Can_Cast_Spell_2)
            {
                GetComponent<MPlayerAttacks>().CmdSpell2();
                StartCoroutine(CD_Spell2());
            }
        }

        if (Input.GetButtonDown(_WeaponAttack))
        {
            GetComponent<MPlayerAttacks>().SwordAttack();
        }

        if (Input.GetButtonDown(_SpellPower_1))
        {
            GetComponent<MPlayer>().SetSpellPower(1);
            GetComponent<MPlayerAttacks>().CmdSetSpellPower(1F);
        }
        if (Input.GetButtonDown(_SpellPower_2))
        {
            GetComponent<MPlayer>().SetSpellPower(2);
            GetComponent<MPlayerAttacks>().CmdSetSpellPower(2F);
        }
        if (Input.GetButtonDown(_SpellPower_3))
        {
            GetComponent<MPlayer>().SetSpellPower(3);
            GetComponent<MPlayerAttacks>().CmdSetSpellPower(3F);
        }
    }

    IEnumerator CD_Spell1()
    {
        _Can_Cast_Spell_1 = false;
        yield return new WaitForSeconds(_Spell_1_CD);
        _Can_Cast_Spell_1 = true;
    }

    IEnumerator CD_Spell2()
    {
        _Can_Cast_Spell_2 = false;
        yield return new WaitForSeconds(_Spell_2_CD);
        _Can_Cast_Spell_2 = true;
    }
}
