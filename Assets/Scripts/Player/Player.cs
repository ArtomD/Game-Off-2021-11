using UnityEngine;
using System.Collections;
using Prime31;


public class Player : MonoBehaviour
{
    // movement config
    public float gravity = -25f;
    public float runSpeed = 8f;
    public float groundDamping = 20f; // how fast do we change direction? higher means faster
    public float inAirDamping = 5f;
    public float jumpHeight = 3f;
    public float maxDashTime = 0.25f;

    internal void ApplyForce(Vector2 force)
    {
        _velocity.x += force.x;
        _velocity.y += force.y;

        
    }

    public float maxDashSpeed = 20f;
    public float maxJumps = 2f;

    private CharacterController2D _controller;
    private Animator _animator;
    private RaycastHit2D _lastControllerColliderHit;
    private Vector3 _velocity;


    private bool isDashing = false;
    private float dashTimeRemaining = 0f;
    private Vector2 dashDirection = new Vector2();
    private Vector3 dashStart = new Vector3();
    private Vector3 dashComplete = new Vector3();
    private int jumpsSinceGrounded = 0;

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

        // logs any collider hits if uncommented. it gets noisy so it is commented out for the demo
        //Debug.Log( "flags: " + _controller.collisionState + ", hit.normal: " + hit.normal );
    }


    void onTriggerEnterEvent(Collider2D col)
    {
        Debug.Log("onTriggerEnterEvent: " + col.gameObject.name);

        jumpsSinceGrounded = 0;
        //JumpPanel jumpPanel = col.gameObject.GetComponent<JumpPanel>();

        //if (jumpPanel != null)
        //{
        //    _velocity = _velocity  * -1;
        //}
    }


    void onTriggerExitEvent(Collider2D col)
    {
        Debug.Log("onTriggerExitEvent: " + col.gameObject.name);
    }

    #endregion



    // the Update loop contains a very simple example of moving the character around and controlling the animation
    void Update()
    {

        if (_controller.isGrounded)
        {
            _velocity.y = 0;
            isDashing = false;
            jumpsSinceGrounded = 0;
        }


        float horizontalDir = Input.GetAxisRaw("Horizontal");
        float verticalDir = Input.GetAxisRaw("Vertical");

        if (horizontalDir > 0.5f)
        {
            if (transform.localScale.x < 0f)
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

            if (_controller.isGrounded)
                _animator.Play(Animator.StringToHash("Run"));
        }
        else if (horizontalDir < -0.5f)
        {
            if (transform.localScale.x > 0f)
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

            if (_controller.isGrounded)
                _animator.Play(Animator.StringToHash("Run"));
        }
        else
        {
            horizontalDir = 0;

            if (_controller.isGrounded)
                _animator.Play(Animator.StringToHash("Idle"));
        }

        bool allowedToDash = !isDashing && (horizontalDir != 0 || verticalDir != 0) && !_controller.isGrounded && jumpsSinceGrounded < maxJumps;
        // we can only dash when in the air
        if (Input.GetKey(KeyCode.Space) && allowedToDash)
        {
            Debug.Log("Began dashing!");
            isDashing = true;
            dashTimeRemaining = maxDashTime;
            jumpsSinceGrounded = jumpsSinceGrounded + 1;
            dashStart = this.gameObject.transform.position;
            dashDirection.Set(horizontalDir, verticalDir);
            _velocity = dashDirection.normalized * (maxDashSpeed + inAirDamping) ;
            _animator.Play(Animator.StringToHash("Jump"));
            
        }



        // we can only jump whilst grounded
        if (_controller.isGrounded && Input.GetKeyDown(KeyCode.UpArrow))
        {
            Debug.Log("Began jump!");
            _velocity.y = Mathf.Sqrt(2f * jumpHeight * -gravity);
            _animator.Play(Animator.StringToHash("Jump"));
        }


        // Timer to stop immediate duplicate dashes, todo set the decay properly, ie now if you put a time of like 3s you will be at max run speed long before then
        if (isDashing && dashTimeRemaining > 0)
        {
            dashTimeRemaining -= Time.deltaTime;

            if (dashTimeRemaining <= 0)
            {                
                dashComplete= this.gameObject.transform.position;
                isDashing = false;
                dashTimeRemaining = 0;

                _velocity =  Vector2.ClampMagnitude(_velocity, runSpeed);
     

                Debug.DrawLine(dashStart, dashComplete, Color.red, 10);
                Debug.Log("Dash Complete, travelled: " + Vector3.Distance(dashStart, dashComplete));
            }
        }



        var smoothedMovementFactor = _controller.isGrounded ? groundDamping : inAirDamping; // how fast do we change direction?

        if (isDashing)
        {
            // Lerp to max run speed in the dash direction to smoothly exit

            Vector2 targetVelocity = Vector2.ClampMagnitude(_velocity, runSpeed);
            _velocity.x = Mathf.Lerp(_velocity.x, targetVelocity.x, Time.deltaTime * smoothedMovementFactor);
            _velocity.y = Mathf.Lerp(_velocity.y, targetVelocity.y, Time.deltaTime * smoothedMovementFactor);
        }
        else
        {
            float targetHorizontalVelocity = horizontalDir * runSpeed;
            _velocity.x = Mathf.Lerp(_velocity.x, targetHorizontalVelocity, Time.deltaTime * smoothedMovementFactor);
            _velocity.y += gravity * Time.deltaTime;
         
        } 

      

        // if holding down bump up our movement amount and turn off one way platform detection for a frame.
        // this lets us jump down through one way platforms
        if (_controller.isGrounded && verticalDir <= -0.5f )
        {
            _velocity.y *= 3f;
            _controller.ignoreOneWayPlatformsThisFrame = true;
        }

        _controller.move(_velocity * Time.deltaTime);

        // grab our current _velocity to use as a base for all calculations
        _velocity = _controller.velocity;
    }
    //}

}
