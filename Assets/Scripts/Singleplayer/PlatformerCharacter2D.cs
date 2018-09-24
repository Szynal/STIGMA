using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Singleplayer
{
    public class PlatformerCharacter2D : MonoBehaviour
    {
        [SerializeField] private LayerMask whatIsGround;
        [SerializeField] private readonly string pullOutWeapon;
        [SerializeField] private readonly string skill_1;
        [SerializeField] private readonly string skill_2;
        [SerializeField] private readonly string spellPower_1;
        [SerializeField] private readonly string spellPower_2;
        [SerializeField] private readonly string spellPower_3;

        public int NumberOfPlayer;
        public float MagicPoints;
        private float amountOfMana;
        private bool onRegenMagicPoints;

        [SerializeField] public float Hp; // hit points / health     AKCESOR !!!! WYMAGANY 
        private float amountOfHealth;

        public float SpellPower;      /// AKCESOR !!!! WYMAGANY 

        [SerializeField] public GameObject HpBar;    // Reference to HP bar 
        [SerializeField] public GameObject ManaBar;  // Reference to Mana bar 
        [SerializeField] private GameMaster gm;                // Reference to the Game Master class. Access to the List<T>()...

        private Transform ceilingCheck;   // A position marking where to check for ceilings
        private Transform groundCheck;    // A position marking where to check if the player is grounded.

        [SerializeField] private readonly float maxSpeed = 10f;                    // The fastest the player can travel in the x axis.
        [SerializeField] private readonly float jumpForce = 400f;                  // Amount of force added when the player jumps.
        [Range(0, 1)] [SerializeField] private readonly float crouchSpeed = .36f;  // Amount of maxSpeed applied to crouching movement. 1 = 100%

        private const float GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
        private const float CeilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up
        private float nextSpell = 0;
        private readonly float spell1Rate = 1F;

        private Animator playerAnimator;  // Reference to the player's animator component.
        private Rigidbody2D _Rigidbody2D;  // Reference to the player's _Rigidbody2D component.

        [SerializeField] private readonly bool airControl = true;                 // Whether or not a player can steer while jumping;
        private bool grounded;            // Whether or not the player is grounded.
        private bool facingRight = true;  // For determining which way the player is currently facing.
        private bool jump;                 // Checks if player is jumping
        private int jumpCount = 0;
        private bool pullOutSword = false; // Checks if the sword is pulling out

        public GameObject WaterBall;       // Reference to Spell_1 (water ball)
        public GameObject WaterImplosion;  // Reference to Spell_2 (water implosion)
        public Transform Diraction;         // Check In which direction the player moves.

        [SerializeField] public GameObject HeroBar;

        private void Awake()
        {
            amountOfHealth = Hp;
            amountOfMana = MagicPoints;
            // Setting up references.
            groundCheck = transform.Find("GroundCheck");
            ceilingCheck = transform.Find("CeilingCheck");
            playerAnimator = GetComponent<Animator>();
            _Rigidbody2D = GetComponent<Rigidbody2D>();
            HeroBar = GameObject.Find("Canvas").transform.GetChild(0).gameObject;
            HeroBar.transform.GetChild(3).gameObject.SetActive(false);
            HeroBar.transform.GetChild(4).gameObject.SetActive(false);


        }

        private void Update()
        {
            HpBar.GetComponent<Image>().fillAmount = (Hp / amountOfHealth);    // update hit points / health bar
            ManaBar.GetComponent<Image>().fillAmount = (MagicPoints / amountOfMana); // update  magic points bar

            if (Input.GetButtonDown(pullOutWeapon))
            {
                pullOutSword = !pullOutSword;
            }

            playerAnimator.SetBool("PullingOutSword", pullOutSword);

            if (grounded)
            {
                jumpCount = 0;
            }
            //      spellpower
            if (Input.GetButtonDown(spellPower_1))
            {
                SpellPower = 1F;
                HeroBar.transform.GetChild(2).gameObject.SetActive(true);
                HeroBar.transform.GetChild(3).gameObject.SetActive(false);
                HeroBar.transform.GetChild(4).gameObject.SetActive(false);
            }
            if (Input.GetButtonDown(spellPower_2))
            {
                SpellPower = 2F;
                HeroBar.transform.GetChild(2).gameObject.SetActive(true);
                HeroBar.transform.GetChild(3).gameObject.SetActive(true);
                HeroBar.transform.GetChild(4).gameObject.SetActive(false);
            }
            if (Input.GetButtonDown(spellPower_3))
            {
                SpellPower = 4F;
                HeroBar.transform.GetChild(2).gameObject.SetActive(true);
                HeroBar.transform.GetChild(3).gameObject.SetActive(true);
                HeroBar.transform.GetChild(4).gameObject.SetActive(true);
            }

            if (Input.GetButtonDown(skill_1) && Time.time > nextSpell && pullOutSword == false && MagicPoints >= WaterBall.GetComponent<Spell1>().CostOfUseSpell * SpellPower)
            {
                WaterBall.GetComponent<Transform>().localScale = this.GetComponent<Transform>().localScale;
                playerAnimator.GetComponent<Animator>().SetTrigger("Shoot");
                nextSpell = Time.time + spell1Rate;
                Instantiate(WaterBall, Diraction.position, Diraction.rotation, transform.parent); //Creating an spell - object clone. Clone inherits from GameMaster class (transform.parent) ;
                gm.ListSpell1.Add(NumberOfPlayer);
                MagicPoints -= WaterBall.GetComponent<Spell1>().CostOfUseSpell * SpellPower;  // Reduce mana points;
            }
            else if (Input.GetButtonDown(skill_2) && Time.time > nextSpell && pullOutSword == false && MagicPoints >= WaterImplosion.GetComponent<Spell_2>().CostOfUseSpell * SpellPower)
            {
                playerAnimator.GetComponent<Animator>().SetTrigger("Cast");
                nextSpell = Time.time + spell1Rate;
                Instantiate(WaterImplosion, Diraction.position, Diraction.rotation, transform.parent); //Creating an spell - object clone. Clone inherits from GameMaster class;
                gm.ListSpell2.Add(NumberOfPlayer);
                MagicPoints -= WaterImplosion.GetComponent<Spell_2>().CostOfUseSpell * SpellPower;  // Reduce mana points;
            }

            // Regeneration of magic points ( +5 MANA per one second).
            if (MagicPoints < 100 && onRegenMagicPoints == false)
            {
                onRegenMagicPoints = true;
                StartCoroutine("RegenMagicPoint");
            }
        }

        private void FixedUpdate()
        {
            grounded = false;

            // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
            // This can be done using layers instead but Sample Assets will not overwrite your project settings.
            Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, GroundedRadius, whatIsGround);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
                {
                    grounded = true;
                }
            }

            playerAnimator.SetBool("Ground", grounded);
            playerAnimator.SetBool("Jump", jump);
            playerAnimator.SetFloat("vSpeed", _Rigidbody2D.velocity.y);
        }

        public void Move(float move, bool crouch, bool jump)
        {
            this.jump = jump;
            // If crouching, check to see if the character can stand up
            if (!crouch && playerAnimator.GetBool("Crouch"))
            {
                // If the character has a ceiling preventing them from standing up, keep them crouching
                if (Physics2D.OverlapCircle(ceilingCheck.position, CeilingRadius, whatIsGround))
                {
                    crouch = true;
                }
            }

            if (!pullOutSword && playerAnimator.GetBool("PullingOutSword"))
            {
                if (Physics2D.OverlapCircle(ceilingCheck.position, CeilingRadius, whatIsGround))
                {
                    pullOutSword = true;
                }
            }

            // Set whether or not the character is crouching in the animator
            playerAnimator.SetBool("Crouch", crouch);
            playerAnimator.SetBool("PullingOutSword", pullOutSword);

            //only control the player if grounded or airControl is turned on
            if (grounded || airControl)
            {
                // Reduce the speed if crouching by the crouchSpeed multiplier
                move = (crouch ? move * crouchSpeed : move);

                // The Speed animator parameter is set to the absolute value of the horizontal input.
                playerAnimator.SetFloat("Speed", Mathf.Abs(move));

                // Move the character
                _Rigidbody2D.velocity = new Vector2(move * maxSpeed, _Rigidbody2D.velocity.y);

                // If the input is moving the player right and the player is facing left...
                if (move > 0 && !facingRight)
                {
                    // ... flip the player.
                    Flip();
                }
                // Otherwise if the input is moving the player left and the player is facing right...
                else if (move < 0 && facingRight)
                {
                    // ... flip the player.
                    Flip();
                }
            }
            // If the player should jump...
            if (jump && jumpCount < 1)
            {
                jumpCount++;
                // Add a vertical force to the player.
                grounded = false;
                playerAnimator.SetBool("Ground", false);
                _Rigidbody2D.AddForce(new Vector2(0f, jumpForce - (_Rigidbody2D.mass * (_Rigidbody2D.velocity.y / Time.deltaTime))));
            }
        }

        private void Flip()
        {
            // Switch the way the player is labelled as facing.
            facingRight = !facingRight;

            // Multiply the player's x local scale by -1.
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }

        public void TakeHp(float hp)
        {
            Hp -= hp;
        }

        private IEnumerator RegenMagicPoint() // Regeneration of magic points ( +5 MANA per one second).
        {
            MagicPoints += 5;
            yield return new WaitForSeconds(1);
            onRegenMagicPoints = false;
        }
    }
}

