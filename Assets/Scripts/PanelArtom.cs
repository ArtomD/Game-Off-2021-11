using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelArtom : MonoBehaviour
{

    [SerializeField]
    private Transform mainPanel;

    [SerializeField]
    private float rotateAmount;
    [SerializeField]
    private float rotateCD;
    private bool canTrigger;
    private float timer;
    [SerializeField]
    private float ID;

    [SerializeField]
    public Transform[] moveAnchors;
    [SerializeField]
    private int anchorIndex;
    [SerializeField]
    private int nextAnchor;
    [SerializeField]
    private int prevAnchor;
    // Start is called before the first frame update
    void Start()
    {
        if (moveAnchors.Length > 0)
        {
            transform.position = moveAnchors[getCurrentAnchor()].position;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Debug.Log(ID + " " + timer);
    }

    public void rotateClockwise(Player player, bool isUp)
    {
        Debug.Log("TRYING TO ROTATE: " + ID + " " + timer + " " + Time.time);
        if (timer + rotateCD < Time.time)
        {
            //launchPlayer(player, isUp);
            mainPanel.rotation = Quaternion.Euler(0, 0, mainPanel.eulerAngles.z - rotateAmount);
            timer = Time.time;
        }
        
    }
    public void rotateCounterClockwise(Player player, bool isUp)
    {
        Debug.Log("TRYING TO ROTATE: " + ID + " " + timer + " " + Time.time);
        if (timer + rotateCD < Time.time)
        {
            //launchPlayer(player, isUp);
            mainPanel.rotation = Quaternion.Euler(0, 0, mainPanel.eulerAngles.z + rotateAmount);
            timer = Time.time;
        }

    }

    public void launchPlayer(PlayerControls1Artom player, bool isUp)
    {
        if (isUp)
        {
            player.panelDashNoReset(transform.up);
        }
        else
        {
            player.panelDashNoReset(-transform.up);
        }
        
    }

    public int getCurrentAnchor()
    {
        return this.anchorIndex;
    }
    public int getNextAnchor()
    {
        return this.nextAnchor;
    }
    public int getPrevAnchor()
    {
        return this.prevAnchor;
    }

    private Vector2 nextAnchorVector()
    {
        if (nextAnchor == -1)
        {
            return new Vector2();
        }
        else
        {
            return (moveAnchors[getNextAnchor()].position - moveAnchors[getCurrentAnchor()].position);
        }
    }

    private Vector2 prevAnchorVector()
    {
        if (prevAnchor == -1)
        {
            return new Vector2();
        }
        else
        {
            return (moveAnchors[getPrevAnchor()].position - moveAnchors[getCurrentAnchor()].position);
        }
    }

    public int chooseAnchor(bool isUp)
    {
        Debug.Log(nextAnchorVector());
        Debug.Log(prevAnchorVector());
        Vector2 targetVector;
        if (isUp)
        {
            targetVector = transform.up;
        }
        else
        {
            targetVector = -transform.up;
        }
        if(!(nextAnchorVector().x==0 && nextAnchorVector().y==0) && !(prevAnchorVector().x == 0 && prevAnchorVector().y == 0))
        {
            return Vector2.Angle(nextAnchorVector() - targetVector, targetVector) < Vector2.Angle(prevAnchorVector() - targetVector, targetVector) ? -1 : 1;
        }
        else if(nextAnchorVector().x == 0 && nextAnchorVector().y == 0)
        {
            Debug.Log(Vector2.Angle(nextAnchorVector() - targetVector, targetVector));
            float angle = Vector2.Angle(prevAnchorVector() - targetVector, targetVector);
            return (angle <= 180 && angle > 0) ? -1 : 0;
        }else if (prevAnchorVector().x == 0 && prevAnchorVector().y == 0)
        {
            Debug.Log(Vector2.Angle(nextAnchorVector() - targetVector, targetVector));
            float angle = Vector2.Angle(nextAnchorVector() - targetVector, targetVector);
            return (angle <= 180 && angle > 0) ? 1 : 0;
        }
        return 0;
    }

    public void moveAnchor(bool forward)
    {
        if (forward)
        {
            if (anchorIndex < moveAnchors.Length-1)
            {
                anchorIndex++;
            }
        }
        else
        {
            if (anchorIndex > 0)
            {
                anchorIndex--;
            }
        }

        if (anchorIndex > 0)
        {
            prevAnchor = anchorIndex - 1;
        }
        else
        {
            prevAnchor = -1;
        }

        if (anchorIndex < moveAnchors.Length-1)
        {
            nextAnchor = anchorIndex + 1;
        }
        else
        {
            nextAnchor = -1;
        }
    }
}
