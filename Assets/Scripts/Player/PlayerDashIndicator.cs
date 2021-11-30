using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(Player))]

public class PlayerDashIndicator : MonoBehaviour
{


    [SerializeField] private GameObject Indicator;
    [SerializeField] private float offset;

    private Player player;

    void Start()
    {
        //player = GetComponent<Player>();
    }


    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");



        
    }

}
