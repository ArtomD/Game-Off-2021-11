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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Debug.Log(ID + " " + timer);
    }

    public void rotateClockwise(PlayerControls1Artom player, bool isUp)
    {
        Debug.Log("TRYING TO ROTATE: " + ID + " " + timer + " " + Time.time);
        if (timer + rotateCD < Time.time)
        {
            launchPlayer(player, isUp);
            mainPanel.rotation = Quaternion.Euler(0, 0, mainPanel.eulerAngles.z - rotateAmount);
            timer = Time.time;
        }
        
    }
    public void rotateCounterClockwise(PlayerControls1Artom player, bool isUp)
    {
        Debug.Log("TRYING TO ROTATE: " + ID + " " + timer + " " + Time.time);
        if (timer + rotateCD < Time.time)
        {
            launchPlayer(player, isUp);
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
}
