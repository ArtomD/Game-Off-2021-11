using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{

    private bool moveRight;
    private bool moveLeft;
    private bool moveUp;
    private bool moveDown;
    private enum Direction { up, upRight, right, downRight, down, downLeft, left, upLeft }
    private Direction facing;
    private bool isRight;
    [SerializeField]
    private Transform arrow;
    private Rigidbody2D rb;
    private bool isAirborne;
    private bool isJumpAvaliable;
    private float jumpPauseTime;
    private float jumpVerticalStrength;
    private float jumpDiagonalStrengthH;
    private float jumpDiagonalStrengthV;
    private bool isDashAvaliable;
    private bool freeMove;
    private float freeMoveTimer;
    private float freeMoveInterval;
    private float dashResetTimer;
    private float sqrDashLength;
    private Vector3 oldPos;
    private bool isDashing;
    private float dashDuration;
    private float dashCooldown;

    private float horizontalMovement;
    private float horizontalMovementAir;
    
    private float dashVelocityCardinal;
    private float dashVelocityDiagonal;

    
    // Start is called before the first frame update
    void Start()
    {
        facing = Direction.right;
        isRight = true;
        rb = GetComponent<Rigidbody2D>();
        isAirborne = true;
        isJumpAvaliable = false;
        isDashAvaliable = false;
        dashResetTimer = Time.time;
        jumpPauseTime = 0.5f;
        jumpVerticalStrength = 850f;
        jumpDiagonalStrengthH = 60f;
        jumpDiagonalStrengthV = 750f;
        dashDuration = 0.3f;
        dashCooldown = 0.6f;
        horizontalMovement = 7f;
        horizontalMovementAir = 3f;
        dashVelocityCardinal = 38f;
        dashVelocityDiagonal = 27f;
        sqrDashLength = 49f;
}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            moveLeft = true;
            moveRight = false;
            if (moveUp)
            {
                facing = Direction.upLeft;
            }
            else if (moveDown)
            {
                facing = Direction.downLeft;
            }
            else
            {
                facing = Direction.left;
            }
            isRight = false;
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            moveLeft = false;
            if (Input.GetKey(KeyCode.D))
            {
                moveRight = true;
                facing = Direction.right;
            }
            if (moveUp && moveRight)
            {
                facing = Direction.upRight;
            }
            else if (moveDown && moveRight)
            {
                facing = Direction.downRight;
            }else if (moveUp)
            {
                facing = Direction.up;
            }
            else if (moveDown)
            {
                facing = Direction.down;
            }

        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            moveRight = true;
            moveLeft = false;
            if (moveUp)
            {
                facing = Direction.upRight;
            }
            else if (moveDown)
            {
                facing = Direction.downRight;
            }
            else
            {
                facing = Direction.right;
            }
            isRight = true;
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            moveRight = false;
            if (Input.GetKey(KeyCode.A))
            {
                moveLeft = true;
                facing = Direction.left;
            }
            if (moveUp && moveLeft)
            {
                facing = Direction.upLeft;
            }
            else if (moveDown && moveLeft)
            {
                facing = Direction.downLeft;
            }
            else if (moveUp)
            {
                facing = Direction.up;
            }
            else if (moveDown)
            {
                facing = Direction.down;
            }
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            moveUp = true;
            moveDown = false;
            if (moveLeft)
            {
                facing = Direction.upLeft;
            }
            else if (moveRight)
            {
                facing = Direction.upRight;
            }
            else
            {
                facing = Direction.up;
            }
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            moveUp = false;
            if (Input.GetKey(KeyCode.S))
            {
                moveDown = true;
            }
            if (isRight)
            {
                facing = Direction.right;
            }
            else
            {
                facing = Direction.left;
            }
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            moveDown = true;
            moveUp = false;
            if (moveLeft)
            {
                facing = Direction.downLeft;
            }
            else if (moveRight)
            {
                facing = Direction.downRight;
            }
            else
            {
                facing = Direction.down;
            }
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            moveDown = false;
            if (Input.GetKey(KeyCode.W))
            {
                moveUp = true;
            }
            if (isRight)
            {
                facing = Direction.right;
            }
            else
            {
                facing = Direction.left;
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jump();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            dash();
        }
    }

    private void FixedUpdate()
    {
        switch (facing)
        {
            case Direction.up:
                arrow.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case Direction.upRight:
                arrow.rotation = Quaternion.Euler(0, 0, -45);
                break; ;
            case Direction.right:
                arrow.rotation = Quaternion.Euler(0, 0, -90f);
                break; ;
            case Direction.downRight:
                arrow.rotation = Quaternion.Euler(0, 0, -135f);
                break; ;
            case Direction.down:
                arrow.rotation = Quaternion.Euler(0, 0, -180f);
                break; ;
            case Direction.downLeft:
                arrow.rotation = Quaternion.Euler(0, 0, -225f);
                break; ;
            case Direction.left:
                arrow.rotation = Quaternion.Euler(0, 0, -270f);
                break; ;
            case Direction.upLeft:
                arrow.rotation = Quaternion.Euler(0, 0, -315f);
                break;
            default: break;
        }
        if (freeMove)
        {
            if (moveRight)
            {
                if (isAirborne)
                {
                    if(rb.velocity.x < horizontalMovementAir)
                    {
                        rb.velocity = new Vector2(horizontalMovementAir, rb.velocity.y);
                    }
                    
                }
                else
                {
                    rb.velocity = new Vector2(horizontalMovement, 0);
                }

            } else if (moveLeft)
            {
                if (isAirborne)
                {
                    if (rb.velocity.x > -horizontalMovementAir)
                    {
                        rb.velocity = new Vector2(-horizontalMovementAir, rb.velocity.y);
                    }
                }
                else
                {
                    rb.velocity = new Vector2(-horizontalMovement, 0);
                }

            }
            else
            {
                if (!isAirborne) {
                    rb.velocity = new Vector2(0, 0);
                   
                }
            }
        }
        else
        {
            if(!isDashing && freeMoveTimer + freeMoveInterval < Time.time)
            {
                freeMove = true;                
            }
        }

        if (!isDashAvaliable && !isAirborne && dashResetTimer+ dashCooldown < Time.time)
        {
            isDashAvaliable = true;
        }
        if (isDashing && dashResetTimer + dashDuration < Time.time)
        {
            isDashing = false;
            freeMove = true;
            rb.velocity = new Vector2(0, 0);
        }
        if(isDashing && (oldPos - transform.position).sqrMagnitude >= sqrDashLength)
        {
            isDashing = false;
            freeMove = true;
            rb.velocity = new Vector2(0, 0);
        }

  

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            Debug.Log("COLL ENTER");
            isAirborne = false;
            isJumpAvaliable = true;            
        }
        
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            Debug.Log("COLL EXIT");
            isAirborne = true;
            isJumpAvaliable = false;
        }
    }

    private void jump()
    {
        if (isJumpAvaliable && freeMove)
        {
            freeMove = false;
            if (moveLeft)
            {
                rb.AddForce(new Vector2(-jumpDiagonalStrengthH, jumpDiagonalStrengthV));
            }
            else if (moveRight)
            {
                rb.AddForce(new Vector2(jumpDiagonalStrengthH, jumpDiagonalStrengthV));
            }
            else
            {
                rb.AddForce(new Vector2(0f, jumpVerticalStrength));
            }
            freeMoveInterval = jumpPauseTime;
            freeMoveTimer = Time.time;
            isJumpAvaliable = false;
        }
    }

    private void dash()
    {
        if (isDashAvaliable)
        {
            oldPos = transform.position;
            switch (facing)
            {
                case Direction.up:
                    rb.velocity = new Vector2(0,dashVelocityCardinal);
                    break;
                case Direction.upRight:
                    rb.velocity = new Vector2(dashVelocityDiagonal, dashVelocityDiagonal);
                    break; ;
                case Direction.right:
                    rb.velocity = new Vector2(dashVelocityCardinal, 0);
                    break; ;
                case Direction.downRight:
                    rb.velocity = new Vector2(dashVelocityDiagonal, -dashVelocityDiagonal);
                    break; ;
                case Direction.down:
                    rb.velocity = new Vector2(0, -dashVelocityCardinal);
                    break; ;
                case Direction.downLeft:
                    rb.velocity = new Vector2(-dashVelocityDiagonal, -dashVelocityDiagonal);
                    break; ;
                case Direction.left:
                    rb.velocity = new Vector2(-dashVelocityCardinal, 0);
                    break; ;
                case Direction.upLeft:
                    rb.velocity = new Vector2(-dashVelocityDiagonal, dashVelocityDiagonal);
                    break;
                default: break;
            }
            freeMove = false;
            isDashing = true;
            isDashAvaliable = false;
            dashResetTimer = Time.time;
        }
    }
}
