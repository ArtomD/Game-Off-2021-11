using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelController : MonoBehaviour
{

    public GameObject defaultPosition;
    public GameObject targetPosition;
    public GameObject goalPosition;
    public GameObject activeSprite;
    public GameObject inactiveSprite;
    public GameObject completeSprite;


    public float requiredForce = -100f;
    public float pushRatio = 1f;
    public float forceThreashold = 1f;
    public float moveSpeed = 0.1f;

    public bool selfReset;
    public float resetTime;
    public float resetSpeed;

    public Direction resetDirection;

    private float LastMoveTimestamp;

    public enum Direction
    {
        Default,
        Target        
    }

    public bool lockOnGoal;
    public bool locked = false;
    
    private Rigidbody2D rb;
    private bool moving;
    private Vector3 moveTarget;


    private void Update()
    {
        if (complete())
        {
            if (lockOnGoal)
                locked = true;
            ToggleInactiveSprite();
        }
        else
        {
            ToggleActiveSprite();
        }
          

        if (selfReset && resetTime < Time.time - LastMoveTimestamp)
        {
            Reset();
        }
    }

    // Start is called before the first frame update
    void Start()
    {        
        rb = GetComponent<Rigidbody2D>();
    }

    public void RegisterImpact(GameObject obj, Vector2 velocity)
    {        
        Vector2 relativePointOfContact = gameObject.transform.InverseTransformPoint(obj.transform.position);
        Vector2 relativeVelocityOfContact = gameObject.transform.InverseTransformPoint(velocity);

        bool abovePanel = relativePointOfContact.y > 0;
        float pushForce = Mathf.Abs(relativeVelocityOfContact.y);

        if (abovePanel)
            Compress(pushForce);
        else
            Expand(pushForce);

    }

    public void Compress(float pushForce)
    {  
        if (GetProgress() < 1 && !locked)
        {
            if(Mathf.Abs(pushForce / requiredForce) > (1 - GetProgress()))            
                moveTarget = targetPosition.transform.position;
            else                
                moveTarget = gameObject.transform.position + (targetPosition.transform.position - gameObject.transform.position) * Mathf.Abs(pushForce / requiredForce);
            Move(moveSpeed);
        }

    }

    public void Expand(float pushForce)
    {

        if (GetProgress() > 0 && !locked)
        {
            if (Mathf.Abs(pushForce / requiredForce) > (GetProgress()))
                moveTarget = defaultPosition.transform.position;                            
            else
                moveTarget = (defaultPosition.transform.position - gameObject.transform.position) * Mathf.Abs(pushForce / requiredForce);
            Move(moveSpeed);
        }

    }

    private void Move(float speed)
    {        
        IEnumerator coroutine;
        coroutine = SmoothMovement(speed);
        if (moving)
            StopCoroutine(coroutine);
        StartCoroutine(coroutine);        
    }

    private float GetProgress()
    {
        return 1 - (targetPosition.transform.localPosition - gameObject.transform.localPosition).magnitude / (targetPosition.transform.localPosition - defaultPosition.transform.localPosition).magnitude;
    }


    protected IEnumerator SmoothMovement(float speed)
    {
        float sqrRemainingDistance = (transform.position - moveTarget).sqrMagnitude;
        moving = true;

        while (sqrRemainingDistance > float.Epsilon && !locked)
        {         
            Vector3 newPostion = Vector3.MoveTowards(rb.position, moveTarget, speed * Time.deltaTime);         
            rb.MovePosition(newPostion);
            sqrRemainingDistance = (transform.position - moveTarget).sqrMagnitude;
            LastMoveTimestamp = Time.time;
            yield return null;
        }
        moving = false;

    }

    private void ToggleActiveSprite()
    {
        activeSprite.SetActive(true);
        inactiveSprite.SetActive(false);
        completeSprite.SetActive(false);
    }

    private void ToggleInactiveSprite()
    {
        activeSprite.SetActive(false);
        inactiveSprite.SetActive(true);
        completeSprite.SetActive(false);
    }

    private void ToggleLockedSprite()
    {
        activeSprite.SetActive(false);
        inactiveSprite.SetActive(false);
        completeSprite.SetActive(true);
    }

    private void Lock()
    {
        locked = true;
        ToggleLockedSprite();
    }

    private void Unlock()
    {
        locked = false;
        ToggleActiveSprite();
    }    

    public bool complete()
    {
        return Vector3.Distance(goalPosition.transform.position, gameObject.transform.position) < 0.1;
    }

    public void Reset()
    {
        Debug.Log("resetting");
        switch (resetDirection)
        {
            case Direction.Default:
                moveTarget = defaultPosition.transform.position;
                Move(resetSpeed);
                break;
            case Direction.Target:
                moveTarget = targetPosition.transform.position;
                Move(resetSpeed);
                break;
            default:
                break;

        }   
    }

}


