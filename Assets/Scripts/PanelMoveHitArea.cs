using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelMoveHitArea : MonoBehaviour
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
    void FixedUpdate()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("DASHING: " + collision.gameObject.GetComponent<Player>().playerIsDashing());
        Player player = collision.gameObject.GetComponent<Player>();
        if (player == null)
        {
            player = collision.gameObject.GetComponentInParent<Player>();
        }
        Debug.Log(player.playerIsDashing());
        if (collision.gameObject.tag == "Player" && player.playerIsDashing())
        {
            int nextAnchor = parentPanel.chooseAnchor(isUp);
            Debug.Log(nextAnchor);
            if (nextAnchor != 0)
            {
                parentPanel.moveAnchor(nextAnchor == 1);
            }
            parentPanel.transform.position = parentPanel.moveAnchors[parentPanel.getCurrentAnchor()].position;
        }
    }
}
