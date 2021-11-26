using System;
using System.Collections;
using UnityEngine;

public class Disolver : MonoBehaviour
{

    public event Action onMaterialized;
    public event Action onDissolved;


    public float solveSpeed = 1f;
    [ColorUsageAttribute(true, true, 0f, 8f, 0.125f, 3f)]
    public Color materialColor;

    public float desolveSpeed = 1f;
    [ColorUsageAttribute(true, true, 0f, 8f, 0.125f, 3f)]
    public Color desolveColor;

    public bool startVisible = false;
    public bool solveOnLoad = false;

    private Material material;
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
        material = gameObject.GetComponent<Renderer>().material;
        if (startVisible)
            disolveAmount = maxDesolve;        
        else        
            disolveAmount = minDesolve;
        material.SetFloat("_DisolveAmount", disolveAmount);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void In(){ 
        material.SetColor("_DisolveColor", materialColor);
        StartCoroutine(Process(solveSpeed, maxDesolve)); 
    }

    public void Out() {
        material.SetColor("_DisolveColor", desolveColor);
        StartCoroutine(Process(desolveSpeed, minDesolve));
    }

    private void UpdateColor(Color color) {
        
    }

    protected IEnumerator Process(float speed, float target)
    {
        while (target != disolveAmount)
        {
            Mathf.MoveTowards(disolveAmount, target, speed);
            material.SetFloat("_DisolveAmount", disolveAmount);
            yield return new WaitForSeconds(effectStepDelay);
        }

        if (target == maxDesolve && onMaterialized != null)
        {
            onMaterialized();
        }

        if (target == minDesolve && onDissolved != null)
        {
            onDissolved();
        }


        yield return null;
    }

    
}
