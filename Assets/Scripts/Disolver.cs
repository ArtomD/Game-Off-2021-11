using System.Collections;
using UnityEngine;

public class Disolver : MonoBehaviour
{
    
    public float solveSpeed = 1f;
    [ColorUsageAttribute(true, true, 0f, 8f, 0.125f, 3f)]
    public Color solveColor;

    public float desolveSpeed = 1f;
    [ColorUsageAttribute(true, true, 0f, 8f, 0.125f, 3f)]
    public Color desolveColor;

    public bool startVisible = false;
    public bool solveOnLoad = false;

    private Material materail;
    public float disolveAmount = 1f;
    private IEnumerator coroutine;
    private float effectStepDelay = 0.05f;
    private float maxDesolve = 1f;
    private float minDesolve = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        if (solveOnLoad)
        {
            Initialize();            
            In();

        }        
            
    }

    public void Initialize()
    {
        materail = gameObject.GetComponent<Renderer>().material;
        if (startVisible)
            disolveAmount = maxDesolve;        
        else        
            disolveAmount = minDesolve;        
        UpdateMaterial();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void In(){ UpdateColor(solveColor);  StartCoroutine(Process(solveSpeed, maxDesolve)); }

    public void Out() { UpdateColor(desolveColor);  StartCoroutine(Process(desolveSpeed, minDesolve));}

    private void UpdateMaterial() { materail.SetFloat("_DisolveAmount", disolveAmount); }
    private void UpdateColor(Color color) { materail.SetColor("_DisolveColor", color); }

    protected IEnumerator Process(float speed, float target)
    {        
        while(target != disolveAmount)
        {     
            if (target > disolveAmount)
            {
                disolveAmount = Mathf.Min(target, disolveAmount + speed);
            }
            else
            {
                disolveAmount = Mathf.Max(target, disolveAmount - speed);
            }
            UpdateMaterial();
            yield return new WaitForSeconds(effectStepDelay);
        }       
        yield return null;
    }

    
}
