using UnityEngine;
using System.Collections;
using Prime31;
using System;

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
    [SerializeField] private int maxJumps = 2;
    [SerializeField] private bool dropThroughOneWayPlatform = false;

    private CharacterController2D _controller;
    private Animator _animator;
    private Vector3 _velocity;


    private int _curJumps = 0; 
    private bool _isDashing = false;
    private float _dashTimeRemaining = 0f;
    private Vector2 impulseForce = new Vector2(0, 0);

    private Disolver _disolver;
    private Transform _trail;

    public GameObject chargeIndicator;
    private bool firstGrounding = false;
    private bool chargeIndicatorOn = true;

    void Awake()
    {
        
        _animator = GetComponent<Animator>();
        _disolver = GetComponent<Disolver>();
        _controller = GetComponent<CharacterController2D>();
        _trail = transform.Find("10 Trail");

        // listen to some events for illustration purposes
        _controller.onControllerCollidedEvent += onControllerCollider;
        _controller.onTriggerEnterEvent += onTriggerEnterEvent;
        _controller.onTriggerExitEvent += onTriggerExitEvent;

        // Listen to disolver events
        _disolver.onDissolved += _disolver_onDissolved;
        _disolver.onMaterialized += _disolver_onMaterialized;

    }


    public void Start()
    {
        _disolver.In();
        _trail.gameObject.SetActive(false);
    }

    private void _disolver_onMaterialized()
    {
        AudioManager.instance.PlaySound(Sound.Name.PlayerSpawned);
        alive = true;
        _trail.gameObject.SetActive(true);
      
    }

    private void _disolver_onDissolved()
    {
        AudioManager.instance.PlaySound(Sound.Name.PlayerDied);
        AudioManager.instance.Pause(Sound.Name.PlayerWalk);
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
        if (_curJumps  > 0) { 
            AudioManager.instance.PlaySound(Sound.Name.PlayerDashAvailable);
        }
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
    private bool alive;

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
            firstGrounding = true;
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
        bool tryingToDash = Input.GetKey(KeyCode.LeftShift);
        bool allowedToDash = !_isDashing  &&  _curJumps < maxJumps;
        
        if (tryingToDash && allowedToDash )
        {
            AudioManager.instance.PlaySound(Sound.Name.PlayerDash);
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (  _controller.isGrounded) {
                AudioManager.instance.PlaySound(Sound.Name.PlayerJump);
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
        if (dropThroughOneWayPlatform && _controller.isGrounded && vertical <= -0.5f )
        {
            _velocity.y = -runSpeed;

            _controller.ignoreOneWayPlatformsThisFrame = true;
        }

        if (alive) { 
            _controller.move(_velocity * Time.deltaTime);
        }
        // TODO: Make sure the sound manager completes a loop before playing again
        if (_controller.isGrounded && _controller.velocity.sqrMagnitude >= 0.2f)
        {
            
            AudioManager.instance.UnPause(Sound.Name.PlayerWalk);
        } else
        {
            AudioManager.instance.Pause(Sound.Name.PlayerWalk);
        }

        // grab our current _velocity to use as a base for all calculations
        _velocity = _controller.velocity;         
        
        if(_curJumps == maxJumps)
        {
            chargeIndicator.SetActive(false);
        }
        else if(firstGrounding && chargeIndicatorOn)
        {
            chargeIndicator.SetActive(true);
        }
    }

    public bool playerIsDashing()
    {
        return _isDashing;
    }


    internal void Damage()
    {
        alive = false;
        hideIndicator();
        _trail.gameObject.SetActive(false);
        AudioManager.instance.PlaySound(Sound.Name.PlayerDamaged);
        _disolver.Out();
        FindObjectOfType<LevelController>().Lose();
    }

    public void hideIndicator()
    {
        chargeIndicatorOn = false;
        chargeIndicator.SetActive(false);
    }

}
