using UnityEngine;
using System.Collections;
using Prime31;


public class Player : MonoBehaviour
{
    [Header("Physics Settings")]
    [SerializeField] private float gravity = -25f;
    [SerializeField] private float runSpeed = 8f;
    [SerializeField] private float groundDamping = 20f; // how fast do we change direction? higher means faster
    [SerializeField] private float inAirDamping = 5f;

    [Header("Jump & Dash Settings")]
    [SerializeField] private float jumpHeight = 3f;
    [SerializeField] private float maxDashTime = 0.25f;
    [SerializeField] private float maxDashSpeed = 20f;
    [SerializeField] private float maxJumps = 2f;

    private CharacterController2D _controller;
    private Animator _animator;
    private Vector3 _velocity;


    private int _curJumps = 0; 
    private bool _isDashing = false;
    private float _dashTimeRemaining = 0f;
    private Vector2 impulseForce = new Vector2(0, 0);

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _controller = GetComponent<CharacterController2D>();

        // listen to some events for illustration purposes
        _controller.onControllerCollidedEvent += onControllerCollider;
        _controller.onTriggerEnterEvent += onTriggerEnterEvent;
        _controller.onTriggerExitEvent += onTriggerExitEvent;
    }


    #region Event Listeners

    void onControllerCollider(RaycastHit2D hit)
    {
        // bail out on plain old ground hits cause they arent very interesting
        if (hit.normal.y == 1f)
            return;

        //Debug.Log( "flags: " + _controller.collisionState + ", hit.normal: " + hit.normal );
    }


    void onTriggerEnterEvent(Collider2D col)
    {
        _curJumps = 0;
    }


    void onTriggerExitEvent(Collider2D col)
    {

    }
    #endregion

    public void ApplyForce(Vector2 force)
    {
        impulseForce.x  = force.x;
        impulseForce.y = force.y;
    }

    private float debugMarkTimer = 0;

    // the Update loop contains a very simple example of moving the character around and controlling the animation
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector2 targetVelocity = new Vector2(0, 0);

        if (_controller.isGrounded)
        {
            _curJumps = 0;
            _isDashing = false;
            _velocity.y = 0;
        }


        debugMarkTimer += Time.deltaTime;
        if (debugMarkTimer >= 0.05f)
        {

            Debug.DrawLine(this.transform.position, this.transform.position + new Vector3(0.1f, 0.1f, 0), Color.red, 2);
            debugMarkTimer -= .05f;
        }

        
        // TODO: Move to animation state machine
        if (horizontal > 0.5f)
        {
            if (transform.localScale.x < 0f)
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

            if (_controller.isGrounded)
                _animator.Play(Animator.StringToHash("Run"));
        }
        else if (horizontal < -0.5f)
        {
            if (transform.localScale.x > 0f)
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

            if (_controller.isGrounded)
                _animator.Play(Animator.StringToHash("Run"));
        }
        else
        {
            if (_controller.isGrounded)
                _animator.Play(Animator.StringToHash("Idle"));
        }

        // Apply velocity based on the current dash input
        bool tryingToDash = Input.GetKey(KeyCode.Space);
        bool allowedToDash = !_isDashing  &&  _curJumps < maxJumps;
        
        if (tryingToDash && allowedToDash )
        {
            // By default dash forward w/out keys
            if (vertical == 0 && horizontal == 0)
            {
                horizontal = transform.localScale.x;
            }

            _isDashing = true;
            _dashTimeRemaining = maxDashTime;
            _curJumps = _curJumps + 1;

            _velocity.Set(horizontal, vertical, 0);
            _velocity = _velocity.normalized * (maxDashSpeed + inAirDamping);
            _animator.Play(Animator.StringToHash("Jump"));
            
        }

        // Change state out of dashing after a delay
        if (_isDashing && _dashTimeRemaining > 0)
        {
            _dashTimeRemaining -= Time.deltaTime;

            if (_dashTimeRemaining <= 0)
            {
                _isDashing = false;
                _dashTimeRemaining = 0;
            }
        }


        // Add an upwards velocity if trying to jump
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (  _controller.isGrounded) {
                _velocity.y = Mathf.Sqrt(2f * jumpHeight * -gravity  * inAirDamping);
                _animator.Play(Animator.StringToHash("Jump"));
            } else
            {
                Debug.Log("Jump pressed but not grounded - maybe change jump threshold");
            }

        }

        var smoothedMovementFactor = _controller.isGrounded ? groundDamping : inAirDamping; // how fast do we change direction?

    

        if (impulseForce.x != 0)
        {
            _velocity.x = impulseForce.x;
            impulseForce.x = 0;
        }

        if (impulseForce.y != 0)
        {
            _velocity.y = impulseForce.y;
            impulseForce.y = 0;
        }

        // If dashing, don't appply gravity so that we have straight dashes
        if (!_isDashing)
        {
            targetVelocity.y = _velocity.y + gravity;

        
            targetVelocity.x = Mathf.MoveTowards(_velocity.x, horizontal * runSpeed, -1*gravity);


        }

        //targetVelocity = Vector2.ClampMagnitude(targetVelocity, runSpeed);
        _velocity.x = Mathf.Lerp(_velocity.x, targetVelocity.x, Time.deltaTime * smoothedMovementFactor);
        _velocity.y = Mathf.Lerp(_velocity.y, targetVelocity.y, Time.deltaTime * smoothedMovementFactor);


        // if holding down bump up our movement amount and turn off one way platform detection for a frame.
        // this lets us jump down through one way platforms
        if (_controller.isGrounded && vertical <= -0.5f )
        {
            _velocity.y = -runSpeed;
            _controller.ignoreOneWayPlatformsThisFrame = true;
        }

        _controller.move(_velocity * Time.deltaTime);

        // grab our current _velocity to use as a base for all calculations
        _velocity = _controller.velocity;
    }

    public bool playerIsDashing()
    {
        return _isDashing;
    }

}
