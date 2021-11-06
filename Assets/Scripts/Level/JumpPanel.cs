using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPanel : MonoBehaviour
{
    //public Vector2 force;
    public float power = 40f;
    private Vector2 force;

    private void Awake()
    {

        //force = this.transform.up.Round(0) * power;
        force = this.transform.up * power ;
        Debug.Log($"Force for {gameObject.name} is: {force}");
    }

    private void Start()
    {
        Debug.DrawLine(gameObject.transform.position, gameObject.transform.position + this.transform.up, Color.cyan, 5);
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
       
        Player player = col.GetComponent<Player>();
        if (player != null)
        {
            Debug.Log("Detected Player");
            player.ApplyForce(force);
        } else
        {
            Debug.Log($"Didn't detect player, actually detected: {col.gameObject.name}");
        }
    }
}
