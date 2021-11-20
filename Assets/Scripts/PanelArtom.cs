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

    private SpriteRenderer renderer;
    [SerializeField]
    private SpriteRenderer[] panelAreas;
    private float[] color;
    private bool[] colorDirection;

    private float delayLength;
    private bool clockwise;
    private bool rotatePanel;

    private float avalue;

    public bool completed;
    private bool cycleColors;
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        color = new float[3];
        colorDirection = new bool[3];
        color[0] = 50;// 50 + Random.value*199;
        color[1] = 250;// 50 + Random.value * 199;
        color[2] = 25;// 50 + Random.value * 199;
        for (int i = 0; i < color.Length; i++)
        {
            if (color[i] >= 250)
            {
                colorDirection[i] = false;
            }
            else
            {
                colorDirection[i] = false;
            }
        }
        if (moveAnchors.Length > 0)
        {
            transform.position = moveAnchors[getCurrentAnchor()].position;
        }
        delayLength = 0.06f;
        avalue = 0.25f;
        cycleColors = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (cycleColors) { 
            for (int i = 0; i < color.Length; i++)
            {
                if (colorDirection[i])
                {
                    color[i]++;
                    if (color[i] >= 250)
                    {
                        colorDirection[i] = false;
                    }
                }
                else
                {
                    color[i]--;
                    if (color[i] <= 50)
                    {
                        colorDirection[i] = true;
                    }
                }
            }
        }
        renderer.color = new Color(color[0]/255, color[1] / 255, color[2] / 255);
        if (avalue > 0.25f)
        {
            avalue -= 0.01f;
        }
        for (int i = 0; i<panelAreas.Length;i++)
        {
            panelAreas[i].color = new Color(color[0] / 255, color[1] / 255, color[2] / 255, avalue);
        }
        if (rotatePanel && timer + delayLength < Time.time)
        {
            avalue = 0.8f;
            if (clockwise)
            {
                mainPanel.rotation = Quaternion.Euler(0, 0, mainPanel.eulerAngles.z - rotateAmount);
            }
            else
            {
                mainPanel.rotation = Quaternion.Euler(0, 0, mainPanel.eulerAngles.z + rotateAmount);
            }
            rotatePanel = false;
        }
    }

    public void rotate(Player player, bool isUp, bool clockwise)
    {
        Debug.Log("TRYING TO ROTATE: " + ID + " " + timer + " " + Time.time);
        if (timer + rotateCD < Time.time)
        {
            launchPlayer(player, isUp, 35);
            this.clockwise = clockwise;
            rotatePanel = true;
            timer = Time.time;
        }        
    }

    public void launchPlayer(Player player, bool isUp, float force)
    {
        if (isUp)
        {
            player.ApplyForce(transform.up* force);
        }
        else
        {
            player.ApplyForce(-transform.up * force);
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

    public void moveAnchor(Player player, bool forward)
    {
        launchPlayer(player, !forward, 15);
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

    public bool Completed()
    {
        Debug.Log("Completed() methond not implemented.");
        return completed;
    }
}
