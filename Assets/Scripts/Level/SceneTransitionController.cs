
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneTransitionController : MonoBehaviour
{
    public string sceneToLoad;

    private Image theImage;

    public float transitionSpeed = 2f;

    private bool shouldReveal;
    private float targetCutoff = 1.1f;
    private float minCutoff = -0.1f;
    private float maxCutoff = 1.1f;

    // Start is called before the first frame update
    void Start()
    {
        shouldReveal = false;
        theImage = transform.Find("background").GetComponent<Image>();

        if (!theImage)
        {
            Debug.LogWarning("Scene Transition Controller could not load background");
        }

        float edgeSmoothing = theImage.material.GetFloat("_EdgeSmoothing");
        minCutoff -= edgeSmoothing;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            shouldReveal = !shouldReveal;
            targetCutoff = shouldReveal ? minCutoff : maxCutoff;
        }


        float maxStep = transitionSpeed * Time.deltaTime;
        float currentCutoff = theImage.material.GetFloat("_Cutoff");
        theImage.material.SetFloat("_Cutoff", Mathf.MoveTowards(currentCutoff, targetCutoff, maxStep));

        if (currentCutoff == minCutoff)
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        
    }
}
