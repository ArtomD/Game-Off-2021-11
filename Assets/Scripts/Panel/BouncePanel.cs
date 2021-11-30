using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncePanel : MonoBehaviour
{
    [SerializeField]
    private PanelArtom parentPanel;

    [SerializeField]
    private bool isUp;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
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
    }
}
