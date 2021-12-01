using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailEffect : MonoBehaviour
{


    [SerializeField] private float minIntensity = 5;
    [SerializeField] private float maxIntensity = 20;

    private ParticleSystem _zeroes;
    private ParticleSystem _ones;
    
    
    void Start()
    {
        _zeroes = transform.Find("Zeros trail").GetComponent<ParticleSystem>();
        _ones = transform.Find("Ones trail").GetComponent<ParticleSystem>();

        
    }


    public void SetIntensity(float intensity)
    {
        intensity = Mathf.Clamp(intensity, 0, 1);
        intensity = minIntensity + ((maxIntensity - minIntensity) * intensity);
        

        var zeroesEmmision = _zeroes.emission;
        zeroesEmmision.rateOverTime = intensity;

        var onesEmmission = _ones.emission;
        onesEmmission.rateOverTime = intensity;
    }
}
