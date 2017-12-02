using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
namespace UnityStandardAssets._2D
{
    public class MultiplayerCharacter2D : NetworkBehaviour
    {
        [SerializeField] private LayerMask m_WhatIsGround;   // A mask determining what is ground to the character

        public float _MANA { get; private set; }       //  magic points
        [SerializeField] public float showMana;
        private float _AmountOfMana;
        private bool _OnRegenMagicPoints;
        [SyncVar(hook = "FacingCallback")] public bool netFacingRight = true;
        public float _HP { get; private set; } // hit points / health    
        [SerializeField] public float showHp;
        private float _AmountOfHealth;

        [SyncVar] public float _SpellPower;

        [SerializeField] public GameObject Canvas;
        private GameObject _HpBar;    // Reference to HP bar 
        private GameObject _ManaBar;  // Reference to Mana bar 

        private Transform _CeilingCheck;   // A position marking where to check for ceilings
        private Transform _GroundCheck;    // A position marking where to check if the player is grounded.

        [SerializeField] private float _MaxSpeed = 10f;                    // The fastest the player can travel in the x axis.
        [SerializeField] private float _JumpForce;                  // Amount of force added when the player jumps.
        [Range(0, 1)] [SerializeField] private float _CrouchSpeed = .36f;  // Amount of maxSpeed applied to crouching movement. 1 = 100%

        const float _GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
        const float _CeilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up
        public float _NextSpell { get; private set; }
        private float _Spell_1Rate = 1F;

        private Animator _PlayerAnimator;  // Reference to the player's animator component.
        private NetworkAnimator _NetworkAnimator;
        private Rigidbody2D _Rigidbody2D;  // Reference to the player's _Rigidbody2D component.

        [SerializeField] private bool _AirControl = true;                 // Whether or not a player can steer while jumping;
        private bool _Grounded;            // Whether or not the player is grounded.
        private bool _FacingRight = true;  // For determining which way the player is currently facing.
        [SyncVar(hook = "JumpCallback")] public bool netJump = false;                // Checks if player is jumping
        private int _JumpCount = 0;
        public bool _PullOutSword { get; private set; }  // Checks if the sword is pulling out


        public GameObject waterBall;       // Reference to Spell_1 (water ball)
        public GameObject waterImplosion;  // Reference to Spell_2 (water implosion)
        public Transform diraction;         // Check In which direction the player moves.

        private string _netID;
        private MPlayer _player;

        private void Start()
        {
            // Setting up references.
            _PlayerAnimator = GetComponent<Animator>();
            _NetworkAnimator = GetComponent<NetworkAnimator>();
            _Rigidbody2D = GetComponent<Rigidbody2D>();
            _GroundCheck = transform.Find("GroundCheck");
            _CeilingCheck = transform.Find("CeilingCheck");
            _netID = GetComponent<NetworkIdentity>().netId.ToString();
            _player = GetComponent<MPlayer>();
            GameManager.RegisterPlayer(_netID, _player);
            Canvas = GameObject.Find("Canvas");
            _HP = 1000;
            _MANA = 100;
            _SpellPower = 1F;
            _HpBar = Canvas.transform.GetChild(0).transform.GetChild(0).gameObject;
            _ManaBar = Canvas.transform.GetChild(0).transform.GetChild(1).gameObject;
            _NextSpell = 0F;
            _PullOutSword = false;
            _AmountOfHealth = _HP;
            _AmountOfMana = _MANA;
        }

        private void Update()
        {
            if (!isLocalPlayer)
            {
                return;
            }

            showHp = _HP;
            showMana = _MANA;
            CmdUpdatePlayerBars();
            _PlayerAnimator.SetBool("PullingOutSword", _PullOutSword);

            if (_Grounded)
            {
                _JumpCount = 0;
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
            if (!isLocalPlayer)
            {
                return;
            }
            _Grounded = false;

            // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
            // This can be done using layers instead but Sample Assets will not overwrite your project settings.
            Collider2D[] colliders = Physics2D.OverlapCircleAll(_GroundCheck.position, _GroundedRadius, m_WhatIsGround);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
                {
                    _Grounded = true;
                }
            }

            _PlayerAnimator.SetBool("Ground", _Grounded);
            _PlayerAnimator.SetBool("Jump", netJump);
            _PlayerAnimator.SetFloat("vSpeed", _Rigidbody2D.velocity.y);
        }

        public void Move(float move , float jump)
        {
            if (!_PullOutSword && _PlayerAnimator.GetBool("PullingOutSword"))
            {
                if (Physics2D.OverlapCircle(_CeilingCheck.position, _CeilingRadius, m_WhatIsGround))
                {
                    _PullOutSword = true;
                }
            }
            _PlayerAnimator.SetBool("PullingOutSword", _PullOutSword);

            //only control the player if grounded or airControl is turned on
            if (_Grounded || _AirControl)
            {

               // Novement vector
               // Vector3 _velocity = (_movHorizontal + _movVertical) * speed;

                // The Speed animator parameter is set to the absolute value of the horizontal input.
                _PlayerAnimator.SetFloat("Speed", Mathf.Abs(move));

                // Move the character
                _Rigidbody2D.velocity = new Vector2(move * _MaxSpeed, _Rigidbody2D.velocity.y);
            }
        }
            
        [Command]
        public void CmdJump(bool jump)
        {
            netJump = jump;
            // If the player should jump...
            if (netJump && _JumpCount < 1)
            {
                _JumpCount++;
                // Add a vertical force to the player.
                _Grounded = false;
                _PlayerAnimator.SetBool("Ground", false);
                _Rigidbody2D.AddForce(new Vector2(0f, _JumpForce - (_Rigidbody2D.mass * (_Rigidbody2D.velocity.y / Time.deltaTime))));
            }
        }

        public void JumpCallback(bool jump)
        {
            netJump = jump;
            // If the player should jump...
            if (netJump && _JumpCount < 1)
            {
                _JumpCount++;
                // Add a vertical force to the player.
                _Grounded = false;
                _PlayerAnimator.SetBool("Ground", false);
                _Rigidbody2D.AddForce(new Vector2(0f, _JumpForce - (_Rigidbody2D.mass * (_Rigidbody2D.velocity.y / Time.deltaTime))));
            }
        }

        [Command]
        public void CmdFlip(bool facing)
        {
            netFacingRight = facing;
            if (netFacingRight)
            {
                Vector3 SpriteScale = transform.localScale;
                SpriteScale.x = 1;
                transform.localScale = SpriteScale;
            }
            else
            {
                Vector3 SpriteScale = transform.localScale;
                SpriteScale.x = -1;
                transform.localScale = SpriteScale;
            }
        }

        void FacingCallback(bool facing)
        {
            netFacingRight = facing;
            if (netFacingRight)
            {
                Vector3 SpriteScale = transform.localScale;
                SpriteScale.x = 1;
                transform.localScale = SpriteScale;
            }
            else
            {
                Vector3 SpriteScale = transform.localScale;
                SpriteScale.x = -1;
                transform.localScale = SpriteScale;
            }
        }

        [Command]
        public void CmdTake_HP(float hp)
        {
            _HP -= hp;
            _HpBar.GetComponent<Image>().fillAmount = (_HP / _AmountOfHealth);    // update hit points / health bar
            if (_HP <= 0)
            {
                _MANA = 0;
                _ManaBar.GetComponent<Image>().fillAmount = (_MANA / _AmountOfMana); // update  magic points bar
                gameObject.SetActive(false);
            }
        }

        IEnumerator RegenMagicPoint() // Regeneration of magic points ( +10 MANA per one second).
        {
            _MANA += 10;
            yield return new WaitForSeconds(1);
            _OnRegenMagicPoints = false;
        }

        public void PullOutSword()
        {
            _PullOutSword = !_PullOutSword;
        }

        [Command]
        public void CmdSpellPower(float spellPower)
        {
            _SpellPower = spellPower;
        }

        [Command]
        public void CmdSpell1()
        {
            waterBall.GetComponent<Transform>().localScale = this.GetComponent<Transform>().localScale;
            _PlayerAnimator.GetComponent<Animator>().SetTrigger("Shoot");
            _NextSpell = Time.time + _Spell_1Rate;
            GameObject instance = Instantiate(waterBall, diraction.position, diraction.rotation, this.GetComponent<Transform>()); //Creating an spell - object clone. Clone inherits from GameMaster class (transform.parent) ;
            GameManager dictionarySpell = new GameManager();

            _MANA -= waterBall.GetComponent<MultiplayerSpell_1>().costOfUseSpell * _SpellPower;  // Reduce mana points;
        }

        [Command]
        public void CmdSpell2()
        {
            _PlayerAnimator.GetComponent<Animator>().SetTrigger("Cast");
            _NextSpell = Time.time + _Spell_1Rate;
            Instantiate(waterImplosion, diraction.position, diraction.rotation, this.GetComponent<Transform>()); //Creating an spell - object clone. Clone inherits from GameMaster class;

            _MANA -= waterImplosion.GetComponent<MultiplayerSpell_2>().CostOfUseSpell * _SpellPower;  // Reduce mana points;
        }
        [Command]
        public void CmdUpdatePlayerBars()
        {
            RpcUpdatePlayerBars();
        }

        [ClientRpc]
        public void RpcUpdatePlayerBars()
        {
            //   _ManaBar.GetComponent<Image>().fillAmount = (_MANA / _AmountOfMana); // update  magic points bar
        }

    }
}
