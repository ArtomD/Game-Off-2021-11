using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PanelMapper : MonoBehaviour
{
    public PanelConnection[] panelConnections;    
    public PanelConnection start;
    public PanelConnection end;
    public GameObject visualTemplate;

    public Material glow;
    public Material dim;

    // Start is called before the first frame update
    void Start()
    {
        foreach(PanelConnection connection in panelConnections)
        {
            connection.Init(glow, dim);
        }
        start.Init(glow, dim);
        end.Init(glow, dim);
        Activate(start);
        Process();
    }

    // Update is called once per frame
    void Update()
    {
        Process();
    }

    public void Process()
    {
        bool disconnected = false;

        for (int panelIndex = 0; panelIndex < panelConnections.Length; panelIndex++)
        {
            if (disconnected)
            {
                Deactivate(panelConnections[panelIndex]);
            }
            else
            {
                if(panelConnections[panelIndex].targetPanel == null)
                {
                    Activate(panelConnections[panelIndex]);
                }else if (panelConnections[panelIndex].targetPanel.isCompleted())
                {
                    Activate(panelConnections[panelIndex]);
                }                    
                else
                {
                    disconnected = true;
                    Deactivate(panelConnections[panelIndex]);
                }
                    
            }
            
        }

        if (!disconnected)
        {
            if(end.targetPanel == null || end.targetPanel.isCompleted())
            {
                // TODO: activate iscalled in way too many places
                Activate(end);

                // TODO: calls to find should be done on awake
                // TODO: also in general this should be avoided, and you should fire an event. This gets called on repeat in every loop.
                FindObjectOfType<LevelController>().Win(); 
            }            
        }
        else
        {
            Deactivate(end);
        }
    }

    private float GetAngle(Vector3 start, Vector3 end)
    {        
        return Vector3.SignedAngle(new Vector3(0, 1, 0), end - start, Vector3.forward);
    }


    private void Activate(PanelConnection panel)
    {        
        if(panel.effect == null)
        {
            panel.effect = Instantiate(visualTemplate, panel.startPosition.transform.position, Quaternion.identity);
            
            panel.effect.transform.parent = gameObject.transform;            
            panel.effect.gameObject.transform.localScale = new Vector3(Vector3.Distance(panel.endPosition.gameObject.transform.position, panel.startPosition.gameObject.transform.position) / 10, panel.effect.gameObject.transform.localScale.x, panel.effect.gameObject.transform.localScale.z);

            Debug.Log(GetAngle(panel.startPosition.gameObject.transform.position, panel.endPosition.gameObject.transform.position));

            panel.effect.transform.Rotate(panel.effect.transform.rotation.x, panel.effect.transform.rotation.y, GetAngle(panel.startPosition.gameObject.transform.position, panel.endPosition.gameObject.transform.position) - 90, Space.World);
        }
        else
        {
            panel.effect.SetActive(true);
        }
        panel.Glow();
        panel?.targetPanel?.panelOnState(true);
        foreach(GameObject connection in panel.lineMain)
        {
            connection.GetComponent<BoxCollider2D>().enabled = true;
        }
        if (panel.lineJoint != null)
            panel.lineJoint.GetComponent<CircleCollider2D>().enabled = true;
    }

    private void Deactivate(PanelConnection panel)
    {
        if(panel.effect != null)
        {
            panel.effect.SetActive(false);            
        }

        panel.Dim();
        panel?.targetPanel?.panelOnState(false);
        foreach (GameObject connection in panel.lineMain)
        {
            connection.GetComponent<BoxCollider2D>().enabled = false;
        }
        if(panel.lineJoint != null)
            panel.lineJoint.GetComponent<CircleCollider2D>().enabled = false;
    }

}
