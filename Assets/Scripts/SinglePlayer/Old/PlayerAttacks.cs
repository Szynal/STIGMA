using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Singleplayer
{
    [RequireComponent(typeof(PlatformerCharacter2D))]
    public class PlayerAttacks : MonoBehaviour
    {
        [SerializeField] public Collider2D IdleAttackCollider;
        [SerializeField] public Collider2D JumpAttackCollider;
        [SerializeField] public Animator Animator;
        [SerializeField] private Transform groundCheck;
        [SerializeField] private LayerMask whatIsGround;                  // A mask determining what is ground to the character

        [SerializeField] private readonly string attack;
        [SerializeField] private readonly string pullOutWeapon;

        private Animation _Animation;

        private const float AttackCast = 0.5F;
        private const float GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
        private const float CeilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up

        private bool pullOutSword = false; // Checks if the sword is pulling out
        private bool canAttack = true;
        private bool grounded;            // Whether or not the player is grounded.

        public PlayerAttacks(LayerMask whatIsGround, string attack, string pullOutWeapon)
        {
            this.whatIsGround = whatIsGround;
            this.attack = attack;
            this.pullOutWeapon = pullOutWeapon;
        }

        private void Awake()
        {
            groundCheck = transform.Find("GroundCheck");
            _Animation = gameObject.GetComponent<Animation>();
            IdleAttackCollider.enabled = false;
            JumpAttackCollider.enabled = false;
        }

        private void FixedUpdate()
        {
            grounded = false;
            Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, GroundedRadius, whatIsGround);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
                {
                    grounded = true;
                }
            }
        }

        // Update is called once per frame
        private void Update()
        {
            if (Input.GetButtonDown(pullOutWeapon))
            {
                pullOutSword = !pullOutSword;
            }

            if (Input.GetButtonDown(attack) && pullOutSword == true && canAttack == true)
            {
                if (grounded == true)
                {
                    IdleAttackCollider.enabled = true;  // activate idle Attacking Collider
                }

                if (grounded == false)
                {
                    JumpAttackCollider.enabled = true; // activate jump attack colider
                }

                Animator.GetComponent<Animator>().SetTrigger("Attacking");

                StartCoroutine(CanAttack());
            }
            if (canAttack)
            {
                IdleAttackCollider.enabled = false;
                JumpAttackCollider.enabled = false;
            }
        }

        private IEnumerator CanAttack()
        {
            canAttack = false;
            yield return new WaitForSeconds(AttackCast);
            canAttack = true;
        }

    }
}
