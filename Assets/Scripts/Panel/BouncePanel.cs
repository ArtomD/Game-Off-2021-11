using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncePanel : MonoBehaviour
{
    [SerializeField]
    private PanelArtom parentPanel;

    [SerializeField]
    private bool isUp;

    private float lastLaunch;
    private bool canTrigger;

    // Start is called before the first frame update
    void Start()
    {
        canTrigger = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!canTrigger && lastLaunch + 0.3f < Time.time)
        {
            canTrigger = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (canTrigger)
        {
            Debug.Log("DASHING: " + collision.gameObject.GetComponent<Player>().PlayerIsDashing());
            Player player = collision.gameObject.GetComponent<Player>();
            if (player == null)
            {
                player = collision.gameObject.GetComponentInParent<Player>();
            }
            Debug.Log(player.PlayerIsDashing());
            if (collision.gameObject.tag == "Player" && player.PlayerIsDashing())
            {
                AudioManager.instance.PlaySound(Sound.Name.PanelBounce);
                parentPanel.setNextJumpHigh();

            }
            canTrigger = false;
            lastLaunch = Time.time;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (canTrigger)
        {
            Debug.Log("DASHING: " + collision.gameObject.GetComponent<Player>().PlayerIsDashing());
            Player player = collision.gameObject.GetComponent<Player>();
            if (player == null)
            {
                player = collision.gameObject.GetComponentInParent<Player>();
            }
            Debug.Log(player.PlayerIsDashing());
            if (collision.gameObject.tag == "Player" && player.PlayerIsDashing())
            {
                AudioManager.instance.PlaySound(Sound.Name.PanelBounce);
                parentPanel.setNextJumpHigh();

            }
            canTrigger = false;
            lastLaunch = Time.time;
        }
    }
}
