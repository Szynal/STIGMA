using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class PlatformerCharacter2D : MonoBehaviour
{
    [SerializeField] private LayerMask _WhatIsGround;   // A mask determining what is ground to the character
    [SerializeField] private String _PullOutWeapon;      // Needed for INPUT 
    [SerializeField] private String _Skill_1;            // Needed for INPUT 
    [SerializeField] private String _Skill_2;            // Needed for INPUT 
    [SerializeField] private String _SpellPower_1;       //   INPUT 
    [SerializeField] private String _SpellPower_2;       //   INPUT 
    [SerializeField] private String _SpellPower_3;       //   INPUT 

    [SerializeField] public int numberOfPlayer; // nedden to know source of spell casting 
    [SerializeField] private float _MANA;       //  magic points
    private float _AmountOfMana;
    private bool _OnRegenMagicPoints;

    [SerializeField] public float _HP; // hit points / health     AKCESOR !!!! WYMAGANY 
    private float _AmountOfHealth;

    public float spellPower;      /// AKCESOR !!!! WYMAGANY 

    [SerializeField] public GameObject hpBar;    // Reference to HP bar 
    [SerializeField] public GameObject manaBar;  // Reference to Mana bar 
    [SerializeField] GameMaster GM;                // Reference to the Game Master class. Access to the List<T>()...

    private Transform _CeilingCheck;   // A position marking where to check for ceilings
    private Transform _GroundCheck;    // A position marking where to check if the player is grounded.

    [SerializeField] private float _MaxSpeed = 10f;                    // The fastest the player can travel in the x axis.
    [SerializeField] private float _JumpForce = 400f;                  // Amount of force added when the player jumps.
    [Range(0, 1)] [SerializeField] private float _CrouchSpeed = .36f;  // Amount of maxSpeed applied to crouching movement. 1 = 100%

    const float _GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
    const float _CeilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up
    private float _NextSpell = 0;
    private float _Spell_1Rate = 1F;

    private Animator _Player_Animator;  // Reference to the player's animator component.
    private Rigidbody2D _Rigidbody2D;  // Reference to the player's _Rigidbody2D component.

    [SerializeField] private bool _AirControl = true;                 // Whether or not a player can steer while jumping;
    private bool _Grounded;            // Whether or not the player is grounded.
    private bool _FacingRight = true;  // For determining which way the player is currently facing.
    private bool _Jump;                 // Checks if player is jumping
    private int _JumpCount = 0;
    private bool _PullOutSword = false; // Checks if the sword is pulling out

    public GameObject waterBall;       // Reference to Spell_1 (water ball)
    public GameObject waterImplosion;  // Reference to Spell_2 (water implosion)
    public Transform diraction;         // Check In which direction the player moves.

    [SerializeField] public GameObject _HeroBar;

    private void Awake()
    {
        _AmountOfHealth = _HP;
        _AmountOfMana = _MANA;
        // Setting up references.
        _GroundCheck = transform.Find("GroundCheck");
        _CeilingCheck = transform.Find("CeilingCheck");
        _Player_Animator = GetComponent<Animator>();
        _Rigidbody2D = GetComponent<Rigidbody2D>();
        _HeroBar = GameObject.Find("Canvas").transform.GetChild(0).gameObject;
        _HeroBar.transform.GetChild(3).gameObject.SetActive(false);
        _HeroBar.transform.GetChild(4).gameObject.SetActive(false);


    }

    private void Update()
    {
        hpBar.GetComponent<Image>().fillAmount = (_HP / _AmountOfHealth);    // update hit points / health bar
        manaBar.GetComponent<Image>().fillAmount = (_MANA / _AmountOfMana); // update  magic points bar

        if (Input.GetButtonDown(_PullOutWeapon)) _PullOutSword = !_PullOutSword;
        _Player_Animator.SetBool("PullingOutSword", _PullOutSword);

        if (_Grounded)
        {
            _JumpCount = 0;
        }
        //      spellpower
        if (Input.GetButtonDown(_SpellPower_1))
        {
            spellPower = 1F;
            _HeroBar.transform.GetChild(2).gameObject.SetActive(true);
            _HeroBar.transform.GetChild(3).gameObject.SetActive(false);
            _HeroBar.transform.GetChild(4).gameObject.SetActive(false);
        }
        if (Input.GetButtonDown(_SpellPower_2))
        {
            spellPower = 2F;
            _HeroBar.transform.GetChild(2).gameObject.SetActive(true);
            _HeroBar.transform.GetChild(3).gameObject.SetActive(true);
            _HeroBar.transform.GetChild(4).gameObject.SetActive(false);
        }
        if (Input.GetButtonDown(_SpellPower_3))
        {
            spellPower = 4F;
            _HeroBar.transform.GetChild(2).gameObject.SetActive(true);
            _HeroBar.transform.GetChild(3).gameObject.SetActive(true);
            _HeroBar.transform.GetChild(4).gameObject.SetActive(true);
        }

        if (Input.GetButtonDown(_Skill_1) && Time.time > _NextSpell && _PullOutSword == false && _MANA >= waterBall.GetComponent<Spell_1>().costOfUseSpell * spellPower)
        {
            waterBall.GetComponent<Transform>().localScale = this.GetComponent<Transform>().localScale;
            _Player_Animator.GetComponent<Animator>().SetTrigger("Shoot");
            _NextSpell = Time.time + _Spell_1Rate;
            Instantiate(waterBall, diraction.position, diraction.rotation, transform.parent); //Creating an spell - object clone. Clone inherits from GameMaster class (transform.parent) ;
            GM.listSpell_1.Add(numberOfPlayer);
            _MANA -= waterBall.GetComponent<Spell_1>().costOfUseSpell * spellPower;  // Reduce mana points;
        }
        else if (Input.GetButtonDown(_Skill_2) && Time.time > _NextSpell && _PullOutSword == false && _MANA >= waterImplosion.GetComponent<Spell_2>().costOfUseSpell * spellPower)
        {
            _Player_Animator.GetComponent<Animator>().SetTrigger("Cast");
            _NextSpell = Time.time + _Spell_1Rate;
            Instantiate(waterImplosion, diraction.position, diraction.rotation, transform.parent); //Creating an spell - object clone. Clone inherits from GameMaster class;
            GM.listSpell_2.Add(numberOfPlayer);
            _MANA -= waterImplosion.GetComponent<Spell_2>().costOfUseSpell * spellPower;  // Reduce mana points;
        }

        // Regeneration of magic points ( +5 MANA per one second).
        if (_MANA < 100 && _OnRegenMagicPoints == false)
        {
            _OnRegenMagicPoints = true;
            StartCoroutine("RegenMagicPoint");
        }
    }

    private void FixedUpdate()
    {
        _Grounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_GroundCheck.position, _GroundedRadius, _WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                _Grounded = true;
            }
        }

        _Player_Animator.SetBool("Ground", _Grounded);
        _Player_Animator.SetBool("Jump", _Jump);
        _Player_Animator.SetFloat("vSpeed", _Rigidbody2D.velocity.y);
    }

    public void Move(float move, bool crouch, bool jump)
    {
        _Jump = jump;
        // If crouching, check to see if the character can stand up
        if (!crouch && _Player_Animator.GetBool("Crouch"))
        {
            // If the character has a ceiling preventing them from standing up, keep them crouching
            if (Physics2D.OverlapCircle(_CeilingCheck.position, _CeilingRadius, _WhatIsGround))
            {
                crouch = true;
            }
        }

        if (!_PullOutSword && _Player_Animator.GetBool("PullingOutSword"))
        {
            if (Physics2D.OverlapCircle(_CeilingCheck.position, _CeilingRadius, _WhatIsGround))
            {
                _PullOutSword = true;
            }
        }

        // Set whether or not the character is crouching in the animator
        _Player_Animator.SetBool("Crouch", crouch);
        _Player_Animator.SetBool("PullingOutSword", _PullOutSword);

        //only control the player if grounded or airControl is turned on
        if (_Grounded || _AirControl)
        {
            // Reduce the speed if crouching by the crouchSpeed multiplier
            move = (crouch ? move * _CrouchSpeed : move);

            // The Speed animator parameter is set to the absolute value of the horizontal input.
            _Player_Animator.SetFloat("Speed", Mathf.Abs(move));

            // Move the character
            _Rigidbody2D.velocity = new Vector2(move * _MaxSpeed, _Rigidbody2D.velocity.y);

            // If the input is moving the player right and the player is facing left...
            if (move > 0 && !_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (move < 0 && _FacingRight)
            {
                // ... flip the player.
                Flip();
            }
        }
        // If the player should jump...
        if (jump && _JumpCount < 1)
        {
            _JumpCount++;
            // Add a vertical force to the player.
            _Grounded = false;
            _Player_Animator.SetBool("Ground", false);
            _Rigidbody2D.AddForce(new Vector2(0f, _JumpForce - (_Rigidbody2D.mass * (_Rigidbody2D.velocity.y / Time.deltaTime))));
        }
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        _FacingRight = !_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void take_HP(float hp)
    {
        _HP -= hp;
    }

    IEnumerator RegenMagicPoint() // Regeneration of magic points ( +5 MANA per one second).
    {
        _MANA += 5;
        yield return new WaitForSeconds(1);
        _OnRegenMagicPoints = false;
    }
}

