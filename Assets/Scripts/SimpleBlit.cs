using UnityEngine;
using System.Collections;

[ExecuteInEditMode, ImageEffectAllowedInSceneView]
public class SimpleBlit : MonoBehaviour
{
    //public Material TransitionMaterial;

    void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        //if (TransitionMaterial != null)
        RenderTexture r = RenderTexture.GetTemporary(
            src.width, src.height, 0
        );


        Graphics.Blit(src, dst);
    }
}
