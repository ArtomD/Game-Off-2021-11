using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidePanelAnchor : MonoBehaviour
{

    [SerializeField]
    private GameObject prevArrow;
    [SerializeField]
    private GameObject nextArrow;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void activateAnchor()
    {
        prevArrow.GetComponent<SpriteRenderer>().enabled = true;
        nextArrow.GetComponent<SpriteRenderer>().enabled = true;
    }

    public void deactivateAnchor()
    {
        prevArrow.GetComponent<SpriteRenderer>().enabled = false;
        nextArrow.GetComponent<SpriteRenderer>().enabled = false;
    }
}
