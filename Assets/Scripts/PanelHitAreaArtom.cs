using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelHitAreaArtom : MonoBehaviour
{
    [SerializeField]
    private PanelArtom parentPanel;

    [SerializeField]
    private bool rotateClockwise;
    [SerializeField]
    private bool isUp;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("DASHING: " + collision.gameObject.GetComponent<PlayerControls>().playerIsDashing());
        PlayerControls1Artom player = collision.gameObject.GetComponent<PlayerControls1Artom>();
        if (player == null)
        {
            player = collision.gameObject.GetComponentInParent<PlayerControls1Artom>();
        }
        Debug.Log(player.playerIsDashing());
        if (collision.gameObject.tag == "Player" && player.playerIsDashing())
        {
            if (rotateClockwise)
            {
                parentPanel.rotateClockwise(player, isUp);
            }
            else
            {
                parentPanel.rotateCounterClockwise(player, isUp);
            }

        }

    }
}
