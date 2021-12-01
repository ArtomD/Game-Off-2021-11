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


    private int _jumpsUsed = 0; 
    private bool _isDashing = false;
    private float _dashTimeRemaining = 0f;
    private Vector2 impulseForce = new Vector2(0, 0);

    private Disolver _disolver;
    private TrailEffect _trail;

    public GameObject chargeIndicator;
    public ParticleSystem dashTrail;

    private bool _isAlive = true;
    private bool _allowMovement = false;

    public event Action<bool> onPlayerDissolveBegin;
    public event Action<bool> onPlayerDissolveComplete;

    private bool PlayerDashAvailableIsUncalled;

    void Awake()
    {
        
        _animator = GetComponent<Animator>();
        _disolver = GetComponent<Disolver>();
        _controller = GetComponent<CharacterController2D>();
        _trail = transform.Find("10 Trail").GetComponent<TrailEffect>();

        // listen to some events for illustration purposes
        _controller.onControllerCollidedEvent += onControllerCollider;
        _controller.onTriggerEnterEvent += onTriggerEnterEvent;
        _controller.onTriggerExitEvent += onTriggerExitEvent;

        // Listen to disolver events
        _disolver.onDissolved += _disolver_onDissolved;
        _disolver.onMaterialized += _disolver_onMaterialized;

        dashTrail.Stop();
        PlayerDashAvailableIsUncalled = true;
    }


    public void Start()
    {
        _disolver.In();
        _trail.gameObject.SetActive(false);
        AudioManager.instance.PlaySound(Sound.Name.PlayerSpawned);
    }

    private void _disolver_onMaterialized()
    {
        
        _allowMovement = true;
        _trail.gameObject.SetActive(true);
      
    }

    private void _disolver_onDissolved()
    {
        if (onPlayerDissolveComplete != null)
            onPlayerDissolveComplete(_isAlive);

        Destroy(this.gameObject);

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
        if (_isDashing && _jumpsUsed  > 0) { 
            AudioManager.instance.PlaySound(Sound.Name.PlayerDashAvailable);
            PlayerDashAvailableIsUncalled = false;
            _jumpsUsed = 0;

            chargeIndicator.SetActive(true);
        }

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



    private float Damp(float source, float target, float smoothing, float dt)
    {
        return Mathf.Lerp(source, target, 1 - Mathf.Pow(smoothing, dt));
    }

    // the Update loop contains a very simple example of moving the character around and controlling the animation
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector2 targetVelocity = new Vector2(0, 0);


        if (_controller.isGrounded)
        {
            _jumpsUsed = 0;
            _isDashing = false;
            _velocity.y = 0;
            chargeIndicator.SetActive(true);
            dashTrail.Stop();
            if (PlayerDashAvailableIsUncalled)
            {
                AudioManager.instance.PlaySound(Sound.Name.PlayerDashAvailable);
                PlayerDashAvailableIsUncalled = false;
            }
        }
        
        // TODO: Move to animation state machine
        if (horizontal > 0.5f)
        {
            if (transform.localScale.x < 0f)
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

            dashTrail.startRotation3D = new Vector3(0, 0, 0);

            if (_controller.isGrounded)
                _animator.Play(Animator.StringToHash("Run"));
            
        }
        else if (horizontal < -0.5f)
        {
            if (transform.localScale.x > 0f)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                dashTrail.startRotation3D = new Vector3(0, Mathf.PI, 0);
            }
                



            if (_controller.isGrounded)
                _animator.Play(Animator.StringToHash("Run"));
        }
        else
        {
            if (_controller.isGrounded)
                _animator.Play(Animator.StringToHash("Idle"));
        }

        // Apply velocity based on the current dash input
        bool tryingToDash = Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift);
        bool allowedToDash = !_isDashing  &&  _jumpsUsed < maxJumps;
        
        if (tryingToDash && allowedToDash )
        {
            AudioManager.instance.PlaySound(Sound.Name.PlayerDash);
            // By default dash forward w/out keys
            if (vertical == 0 && horizontal == 0)
            {
                horizontal = transform.localScale.x;
            }

            _isDashing = true;
            dashTrail.Play();
            _dashTimeRemaining = maxDashTime;
            _jumpsUsed = _jumpsUsed + 1;

            _velocity.Set(horizontal, vertical, 0);
            _velocity = _velocity.normalized * (maxDashSpeed + inAirDamping);
            _animator.Play(Animator.StringToHash("Jump"));
            PlayerDashAvailableIsUncalled = true;
        }

        // Change state out of dashing after a delay
        if (_isDashing && _dashTimeRemaining > 0)
        {
            _dashTimeRemaining -= Time.deltaTime;

            if (_dashTimeRemaining <= 0)
            {
                _isDashing = false;
                dashTrail.Stop();
                _dashTimeRemaining = 0;
            }
        }


        // Add an upwards velocity if trying to jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (  _controller.isGrounded) {
                AudioManager.instance.PlaySound(Sound.Name.PlayerJump);
                _velocity.y =2f * jumpHeight * -gravity  * inAirDamping;
                _animator.Play(Animator.StringToHash("Jump"));
            } else
            {
                //Debug.Log("Jump pressed but not grounded - maybe change jump threshold");
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


        //transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        _velocity.x = Damp(_velocity.x, targetVelocity.x, smoothedMovementFactor, Time.deltaTime);
        _velocity.y = Damp(_velocity.y, targetVelocity.y, smoothedMovementFactor, Time.deltaTime);


        // if holding down bump up our movement amount and turn off one way platform detection for a frame.
        // this lets us jump down through one way platforms
        if (dropThroughOneWayPlatform && _controller.isGrounded && vertical <= -0.5f )
        {
            _velocity.y = -runSpeed;
            _controller.ignoreOneWayPlatformsThisFrame = true;
        }

        if (_allowMovement) { 
            _controller.move(_velocity * Time.deltaTime);
            // grab our current _velocity to use as a base for all calculations
            _velocity = _controller.velocity;

           
            float roundedVelocity = Mathf.Round(_velocity.sqrMagnitude * 100) / 100;
            _trail.SetIntensity(roundedVelocity);
        }

        // TODO: Make sure the sound manager completes a loop before playing again
        if (_allowMovement && _controller.isGrounded && _controller.velocity.sqrMagnitude >= 0.2f)
        {
            
            AudioManager.instance.UnPause(Sound.Name.PlayerWalk);
            
        } else
        {
            AudioManager.instance.Pause(Sound.Name.PlayerWalk);
        }

        


        if (_jumpsUsed == maxJumps)
        {
            chargeIndicator.SetActive(false);
        }

    }

    public bool IsAlive()
    {
        return _isAlive;
    }

    public bool PlayerIsDashing()
    {
        return _isDashing;
    }


    public void Dissolve()
    {
        chargeIndicator.SetActive(false);
        _trail.gameObject.SetActive(false);
        _allowMovement = false;
        _disolver.Out();
    }

    public void Die()
    {
        AudioManager.instance.PlaySound(Sound.Name.PlayerDamaged);
        _isAlive = false;
        Dissolve();
    }

}
