using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIAnimator : MonoBehaviour
{
    

    private GameObject[] glows;
    private GameObject[] TMPFields;
    private GameObject[] TMPNonGlowFields;
    private GameObject[] SliderGlowFields;

    private float glowMin = 0.4f;
    private float glowMax = 0.8f;

    PointerEventData pe = new PointerEventData(EventSystem.current);
    

    public Color32[] colors;

    void Awake()
    {

        glows = GameObject.FindGameObjectsWithTag("ButtonGlow");
        TMPFields = GameObject.FindGameObjectsWithTag("TextGlow");
        TMPNonGlowFields = GameObject.FindGameObjectsWithTag("TextNoneGlow");
        SliderGlowFields = GameObject.FindGameObjectsWithTag("SliderGlow");
        //Debug.Log(TMPFields.Length);
    }

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
        Color lerpedColor = Color.Lerp(colors[0], colors[1], Mathf.PingPong(Time.time, 1.5f));           

        for (int buttonIndex = 0; buttonIndex < glows.Length; buttonIndex++)
        {
            lerpedColor.a = glows[buttonIndex].GetComponent<Image>().color.a;
            glows[buttonIndex].GetComponent<Image>().color = lerpedColor;
            
        }
        for (int TMPFieldIndex = 0; TMPFieldIndex < TMPFields.Length; TMPFieldIndex++)
        {
            if(TMPFields[TMPFieldIndex].GetComponent<TMPro.TextMeshProUGUI>() != null)
            {
                lerpedColor.a = TMPFields[TMPFieldIndex].GetComponent<TMPro.TextMeshProUGUI>().fontSharedMaterial.GetColor(ShaderUtilities.ID_GlowColor).a;
                TMPFields[TMPFieldIndex].GetComponent<TMPro.TextMeshProUGUI>().fontSharedMaterial.SetColor(ShaderUtilities.ID_GlowColor, lerpedColor);
                lerpedColor.a = TMPFields[TMPFieldIndex].GetComponent<TMPro.TextMeshProUGUI>().color.a;
                TMPFields[TMPFieldIndex].GetComponent<TMPro.TextMeshProUGUI>().color = lerpedColor;
            }
            

        }
        for (int TMPNoneFieldIndex = 0; TMPNoneFieldIndex < TMPNonGlowFields.Length; TMPNoneFieldIndex++)
        {
            lerpedColor.a = TMPNonGlowFields[TMPNoneFieldIndex].GetComponent<TMPro.TextMeshProUGUI>().color.a;
            TMPNonGlowFields[TMPNoneFieldIndex].GetComponent<TMPro.TextMeshProUGUI>().color = lerpedColor;            
        }
        /*
        for (int SliderGlowFieldsIndex = 0; SliderGlowFieldsIndex < SliderGlowFields.Length; SliderGlowFieldsIndex++)
        {
            lerpedColor.a = SliderGlowFields[SliderGlowFieldsIndex].GetComponent<Slider>().colors.normalColor.a;
            SliderGlowFields[SliderGlowFieldsIndex].GetComponent<Slider>().colors.normalColor = lerpedColor;
        }*/
    }

    public void MouseOver(GameObject glow)
    {
        Color color = glow.GetComponent<Image>().color;
        color.a = glowMax;
        glow.GetComponent<Image>().color = color;
    }

    public void MouseExit(GameObject glow)
    {
        Color color = glow.GetComponent<Image>().color;
        color.a = glowMin;
        glow.GetComponent<Image>().color = color;
    }
}
