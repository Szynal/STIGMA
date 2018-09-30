using UnityEngine;


namespace Assets.Scripts.SinglePlayer.Character
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class CharacterPhysics : MonoBehaviour
    {
        public bool Grounded { get; private set; }
        public Rigidbody2D AttachedRigidbody { get; set; }

        [Header("Physics")]
        public float GravitationalAcceleration = 9.80665f;
        public float Mass;
        public BoxCollider2D CharacterCollider;

        [Header("Move")]
        public float MoveAcceleration = 0.2f;
        public float MoveMaxSpeed = 8;

        [Header("Jump")]
        public float JumpAcceleration = 2;

        private int collisions;
        private bool facingRight = true;

        public JumpState jumpState;
        public enum JumpState
        {
            Grounded,
            Jump,
            AirJump
        }

        private void Awake()
        {
            if (CharacterCollider == null)
            {
                Debug.LogError(" BoxCollider2D is required");
            }

            AttachedRigidbody = GetComponent<Rigidbody2D>();
            SetRigidbody2D();
        }

        #region InitializationRigidbody
        private void SetRigidbody2D()
        {
            if (AttachedRigidbody != null)
            {
                AttachedRigidbody.simulated = true;
                AttachedRigidbody.mass = Mass;
                //AttachedRigidbody.drag = 0.1f;
                AttachedRigidbody.angularDrag = 0.1f;
                AttachedRigidbody.gravityScale = GravitationalAcceleration;
                AttachedRigidbody.freezeRotation = true;
            }
            else
            {
                Debug.LogWarning("CharacterRigidbody is null");
            }
        }
        #endregion

        private void Start()
        {
            CheckGrounded(); ;
        }

        public void Move(float moveDelta, bool jumpInput)
        {
            Vector2 velocity = AttachedRigidbody.velocity;
            velocity.x = UpdateHorizontalVelocity(new Vector2(moveDelta,0), velocity);
            velocity = ClampToMaxSpeed(velocity);

            CheckGrounded();
            if (jumpInput && jumpState != JumpState.AirJump)
            {
                Jump();
            }
            AttachedRigidbody.velocity = velocity;

            CharacterTurn(velocity.x);
        }

        private float UpdateHorizontalVelocity(Vector2 moveDelta, Vector2 velocity)
        {
            var goalAcceleration = transform.rotation * moveDelta.normalized * MoveAcceleration;
            return velocity.x += goalAcceleration.x;
        }

        private Vector2 ClampToMaxSpeed(Vector2 velocity)
        {
            var horizontalVelocity = velocity.x;
            var verticalVelocity = velocity.y;
            if (Mathf.Abs(horizontalVelocity) > MoveMaxSpeed)
            {
                velocity.x *= Mathf.Abs(MoveMaxSpeed / horizontalVelocity);
            }
            if (verticalVelocity < -(MoveMaxSpeed*2))
            {
                velocity.y *= -MoveMaxSpeed / verticalVelocity;
            }

            return velocity;
        }

        private void Jump()
        {
            Grounded = false;
            jumpState++;
            AttachedRigidbody.AddForce(new Vector2(0f, JumpAcceleration - (AttachedRigidbody.mass * (AttachedRigidbody.velocity.y / Time.deltaTime))));
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            collisions++;
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            collisions--;
        }

        private void CheckGrounded()
        {
            Grounded = (collisions > 0 && Physics2D.Raycast(transform.position, Vector3.down));
            if (!Grounded)
            {
                return;
            }
            jumpState = JumpState.Grounded;
            AttachedRigidbody.velocity= new Vector2(AttachedRigidbody.velocity.x,0);
        }

        private void CharacterTurn(float move)
        {
            if (move > 0 && !facingRight)
            {
                Flip();
            }
            else if (move < 0 && facingRight)
            {
                Flip();
            }
        }

        public void Flip()
        {
            facingRight = !facingRight;
            var theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }
}
