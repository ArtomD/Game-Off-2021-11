using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPanel : MonoBehaviour
{
    //public Vector2 force;
    public float power = 20f;
    private Vector2 force;

    private void Awake()
    {
        force = this.transform.up.normalized * power;
        Debug.Log($"Force for {gameObject.name} is: {force}");
    }


    public void OnTriggerEnter2D(Collider2D col)
    {
        Debug.DrawLine(gameObject.transform.position, force, Color.cyan, 5);
        Player player = col.GetComponent<Player>();
        if (player != null)
        {
            //ContactPoint2D[] contacts = new ContactPoint2D[10];
            //col.GetContacts(contacts);


            player.ApplyForce(force);
            
        }
    }
}
