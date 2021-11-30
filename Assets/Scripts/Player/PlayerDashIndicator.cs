using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(Player))]

public class PlayerDashIndicator : MonoBehaviour
{


    [SerializeField] private GameObject indicator;
    [SerializeField] private float offset = 1f;
    [SerializeField] private float tweakRotation = -90;


    private Player _player;

    private void Awake()
    {
        _player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        if (_player.IsAlive() && Mathf.Abs(horizontal) + Mathf.Abs(vertical) > 0.1f) {
            indicator.SetActive(true);
            // Alternately, we can get the 
            Vector3 offsetDirection = new Vector3(horizontal, vertical, 0).normalized;
            Vector3 newPosition = this.transform.position + offsetDirection * offset;
            indicator.transform.position = newPosition ;

            // rotate?
            var dir =  newPosition - transform.position;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + tweakRotation;
            indicator.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        } else
        {
            indicator.SetActive(false);

        }
    }

}
