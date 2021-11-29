using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tooltip : MonoBehaviour
{
    public GameObject tip;
    public bool hideOnExit = true;
    public float minShowTime = 2f;
    public float maxShowTime = 2f;

    private float elapsedTime;
    private float showTime;
    private DateTime time;

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
        tip.SetActive(true);        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (hideOnExit)
        {
            tip.SetActive(false);
        }            
    }
}
