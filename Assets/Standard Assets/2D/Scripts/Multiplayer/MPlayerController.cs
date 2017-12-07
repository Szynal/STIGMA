using System;
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
    [SerializeField] public float _JumpForce;
    // Component caching
    private MPlayerMovement _Movment;


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
            GetComponent<MPlayerAttacks>().Spell1();
        }

        if (Input.GetButtonDown(_Skill2))
        {
            GetComponent<MPlayerAttacks>().Spell2();
        }

        if (Input.GetButtonDown(_WeaponAttack))
        {         
            

        }

    }
}
