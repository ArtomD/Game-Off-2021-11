using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelColliderDetector : MonoBehaviour
{
    public PanelController panel;
    private float triggerTimer;    

    void OnTriggerEnter2D(Collider2D col)
    {        
        if (triggerTimer + 0.5f < Time.time)
        {
            try
            {
                if (col.gameObject.GetComponent<PlayerControls1Artom>())
                {

                    panel.RegisterImpact(col.gameObject, col.gameObject.GetComponent<PlayerControls1Artom>().velocity);
                }
                else if (col.gameObject.transform.parent.GetComponent<PlayerControls1Artom>())
                {

                    panel.RegisterImpact(col.gameObject, col.gameObject.transform.parent.GetComponent<PlayerControls1Artom>().velocity);
                }
                triggerTimer = Time.time;
            }
            catch (NullReferenceException e)
            {
                
            }

        }
    }

}
