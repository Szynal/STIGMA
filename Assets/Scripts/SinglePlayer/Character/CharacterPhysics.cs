using UnityEngine;

namespace Assets.uScripstsi.SinglePlayer.Character
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class CharacterPhysics : MonoBehaviour
    {
        public bool Grounded
        {
            get { return grounded; }
            set { grounded = value; }
        }

        public float Mass;
        public const float GravityScale = 9;

        public float MovementSpeed = 10f;
        public float JumpForce = 400f;

        private Transform transform;
        private Rigidbody2D rigidbody2D;
        private RaycastHit2D raycastHit;

        private readonly bool airControl = true;
        private bool grounded = true;
        private readonly CharacterState characterState;
        private readonly JumpState jumpState;

        public enum CharacterState
        {
            Idle,
            Run,
            Jump
        }

        public enum JumpState
        {
            Idle,
            Jump,
            AirJump
        }

        private void Start()
        {
            transform = GetComponent<Transform>();
            rigidbody2D = GetComponent<Rigidbody2D>();
            SetRigidbody2D();
        }

        private void SetRigidbody2D()
        {
            if (rigidbody2D != null)
            {
                rigidbody2D.mass = Mass;
                rigidbody2D.gravityScale = GravityScale;
                rigidbody2D.freezeRotation = true;
            }
            else
            {
                Debug.LogWarning("Rigidbody2D is null");
            }
        }

        private void CheckGrounding()
        {
            raycastHit = Physics2D.Raycast(transform.position, Vector3.down, transform.position.y + 1f);
            Debug.DrawRay(transform.position, Vector3.down, Color.red, 5f); // Only for test
            if (raycastHit.collider != null)
            {
                grounded = true;
            }
            else
            {
                grounded = true;
            }
        }
    }
}
