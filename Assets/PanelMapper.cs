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

    // Start is called before the first frame update
    void Start()
    {
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
                if (panelConnections[panelIndex].targetPanel.Completed())
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
            if(end.targetPanel.Completed())
            {
                Activate(end);
                Win();
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

    private void Win()
    {
        Debug.Log("YOU WIN!");
    }

    private void Activate(PanelConnection panel)
    {        
        if(panel.connection == null)
        {
            panel.connection = Instantiate(visualTemplate, panel.startPosition.transform.position, Quaternion.identity);
            
            panel.connection.transform.parent = gameObject.transform;

            panel.connection.gameObject.transform.localScale = new Vector3(panel.connection.gameObject.transform.localScale.x, Vector3.Distance(panel.endPosition.gameObject.transform.position, panel.startPosition.gameObject.transform.position), panel.connection.gameObject.transform.localScale.z);
            panel.connection.transform.Rotate(panel.connection.transform.rotation.x, panel.connection.transform.rotation.y, GetAngle(panel.startPosition.gameObject.transform.position, panel.endPosition.gameObject.transform.position), Space.World);
        }
        else
        {
            panel.connection.SetActive(true);
        }
    }

    private void Deactivate(PanelConnection panel)
    {
        if(panel.connection != null)
            panel.connection.SetActive(false);
    }

}
