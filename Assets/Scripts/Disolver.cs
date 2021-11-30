using System;
using System.Collections;
using UnityEngine;

public class Disolver : MonoBehaviour
{

    public event Action onMaterialized;
    public event Action onDissolved;


    public float materializeSpeed = 0.01f;
    [ColorUsageAttribute(true, true, 0f, 8f, 0.125f, 3f)]
    public Color materialColor;

    public float dissolveSpeed = 0.01f;
    [ColorUsageAttribute(true, true, 0f, 8f, 0.125f, 3f)]
    public Color dissolveColor;

    private Material material;
    public float curDissolveAmount = 0f;
    private IEnumerator coroutine;
    private float effectStepDelay = 0.05f;
    private float maxDissolve = 1f;
    private float minDissolve = 0.0f;

    // Start is called before the first frame update
    void Awake()
    {
        material = gameObject.GetComponent<Renderer>().material;
        material.SetFloat("_DisolveAmount", curDissolveAmount);
    }


    public void In(){ 
        //Debug.Log("Materializing to: " + materialColor.ToString());
        curDissolveAmount = material.GetFloat("_DisolveAmount");
        material.SetColor("_DisolveColor", materialColor);
        StartCoroutine(Process(maxDissolve, materializeSpeed)); 
    }

    public void Out() {
        curDissolveAmount = material.GetFloat("_DisolveAmount");
        material.SetColor("_DisolveColor", dissolveColor);
        StartCoroutine(Process(minDissolve, dissolveSpeed));
    }

    private void UpdateColor(Color color) {
        
    }

    protected IEnumerator Process(float target, float speed)
    {
        while (target != curDissolveAmount)
        {
            curDissolveAmount = Mathf.MoveTowards(curDissolveAmount, target, speed);
            material.SetFloat("_DisolveAmount", curDissolveAmount);
            yield return new WaitForSeconds(effectStepDelay);
        }

        if (target == maxDissolve && onMaterialized != null)
        {
            onMaterialized();
        }

        if (target == minDissolve && onDissolved != null)
        {
            onDissolved();
        }


        yield return null;
    }

    
}
