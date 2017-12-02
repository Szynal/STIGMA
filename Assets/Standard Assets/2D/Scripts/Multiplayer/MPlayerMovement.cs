using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MPlayerMovement : MonoBehaviour
{
    private float _Velocity = 0F;
    private Vector2 _JumpVector = Vector2.zero;
    [SerializeField] private float _Speed = 10f;
    private Rigidbody2D _Rigidbody;
    private Animator _Animator;

    public int _JumpCounter { get; set; }
    public bool _Grounded { get; set; }            // Whether or not the player is grounded.
    private bool _FacingRight = true;  // For determining which way the player is currently facing.

    void Start()
    {
        _Rigidbody = GetComponent<Rigidbody2D>();
        _Animator = GetComponent<Animator>();
    }

    // Gets a movement vector
    public void Move(float velocity)
    {
        _Velocity = velocity;

    }

    // Get a force vector for our thrusters
    public void ApplyJump(Vector2 jumpForce)
    {
        _JumpVector = jumpForce;
    }

    private void Update()
    {
        PerformMovement();
        _Animator.SetBool("Ground", _Grounded);

    }


    // Run every physics iteration
    void FixedUpdate()
    {

    }

    //Perform movement based on velocity variable
    void PerformMovement()
    {
        if (_Grounded)
        {
            _JumpCounter = 0;
        }

        if ((_Velocity > 0 && !_FacingRight) || (_Velocity < 0 && _FacingRight))
        {
            _FacingRight = !_FacingRight;

            if (_FacingRight)
            {
                Vector3 SpriteScale = transform.localScale;
                SpriteScale.x = 1;
                transform.localScale = SpriteScale;
                GetComponent<MPlayer>().CmdSyncScale(1);

            }
            else
            {
                Vector3 SpriteScale = transform.localScale;
                SpriteScale.x = -1;
                transform.localScale = SpriteScale;
                GetComponent<MPlayer>().CmdSyncScale(-1);
            }


        }

        if (_JumpVector != Vector2.zero && _JumpCounter <= 0)
        {
            _Rigidbody.velocity = new Vector2(0f, 0f);
            _Rigidbody.AddForce(_JumpVector, ForceMode2D.Impulse);
            _JumpCounter++;
            _Animator.SetTrigger("Jump");
        }

        if (_Velocity != 0F)
        {
            _Rigidbody.velocity = new Vector2(_Velocity * _Speed, _Rigidbody.velocity.y);
            _Animator.SetFloat("Speed", Mathf.Abs(_Velocity));   // Animate movement
        }
    }
}
