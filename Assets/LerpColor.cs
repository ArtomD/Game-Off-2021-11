using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LerpColor : MonoBehaviour
{

    public Color start;
    public Color finish;

    public float cycleDuration = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Color lerpedColor = Color.Lerp(start, finish, Mathf.PingPong(Time.time, cycleDuration));

        
        lerpedColor.a = gameObject.GetComponent<Image>().color.a;
        gameObject.GetComponent<Image>().color = lerpedColor;

    }
}
