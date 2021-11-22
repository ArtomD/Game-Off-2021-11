using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class PanelConnection
{

    public GameObject connection;
    public PanelArtom targetPanel;

    [HideInInspector]
    public GameObject startPosition;
    [HideInInspector]
    public GameObject endPosition;
    [HideInInspector]
    public GameObject lineJoint;
    [HideInInspector]
    public List<GameObject> lineMain;

    private GameObject visual;
    [HideInInspector]
    public GameObject effect;

    private Material on;
    private Material off;

    // Start is called before the first frame update
    void Start() {
        // startPosition = connection.FindComponentInChildWithTag<GameObject>("StartPoint");
    }

    public void Init(Material on, Material off)
    {
        this.on = on;
        this.off = off;
        //GameObject[] children = connection.G<GameObject>();        
        for (int i = 0; i < connection.transform.childCount; i++)
        {
            GameObject child = connection.transform.GetChild(i).gameObject;
            if (child.tag == "StartPoint")
                startPosition = child;
            if (child.tag == "EndPoint")
                endPosition = child;
            if (child.tag == "LineJoint")
                lineJoint = child;
            if (child.tag == "LineMain")
                lineMain.Add(child);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Dim()
    {
        if(lineJoint != null)
            lineJoint.GetComponent<Renderer>().material = off;
        foreach(GameObject line in lineMain)
        {
            line.GetComponent<Renderer>().material = off;
        }
        
    }

    public void Glow()
    {
        if (lineJoint != null)
            lineJoint.GetComponent<Renderer>().material = on;
        foreach (GameObject line in lineMain)
        {
            line.GetComponent<Renderer>().material = on;
        }
    }
}

