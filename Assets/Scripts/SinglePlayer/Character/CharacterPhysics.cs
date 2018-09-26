using UnityEngine;

namespace Assets.Scripts.SinglePlayer.Character
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class CharacterPhysics : MonoBehaviour
    {
        public bool Grounded { get; set; }
        public Rigidbody2D PhysicSimulation { get; set; }
        public RaycastHit2D Hit { get; set; }
        public const float GravityScale = 10;

        public float Mass;
        public float MovementSpeed = 10f;
        public float JumpForce = 400f;

        private bool facingRight = true;
        private JumpState jumpState;

        private enum JumpState
        {
            Idle,
            Jump,
            AirJump
        }

        private void Start()
        {
            PhysicSimulation = GetComponent<Rigidbody2D>();
            SetRigidbody2D();
        }

        private void SetRigidbody2D()
        {
            if (PhysicSimulation != null)
            {
                PhysicSimulation.mass = Mass;
                PhysicSimulation.gravityScale = GravityScale;
                PhysicSimulation.freezeRotation = true;
            }
            else
            {
                Debug.LogWarning("CharacterRigidbody is null");
            }
        }

        private void CheckGrounding()
        {
            Hit = Physics2D.Raycast(transform.position, Vector3.down);
            Debug.DrawRay(transform.position, Vector3.down, Color.red, 5f); // Only for test
            Grounded = Hit.collider != null ? true : false;

            if (Grounded)
            {
                jumpState = JumpState.Idle;
            }
        }

        public void Jump()
        {
            CheckGrounding();
            if (jumpState == JumpState.AirJump)
            {
                return;
            }

            PhysicSimulation.AddForce(new Vector2(0f, JumpForce - (Mass * (PhysicSimulation.velocity.y / Time.deltaTime))));
            jumpState++;
        }

        public void Move(float move)
        {
            PhysicSimulation.velocity = new Vector2(move * MovementSpeed, PhysicSimulation.velocity.y);
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
