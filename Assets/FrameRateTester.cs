using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameRateTester : MonoBehaviour
{


    // Update is called once per frame
    void Update()
    {
        #if (UNITY_EDITOR)
        if (Input.GetKeyDown(KeyCode.H))
        {
            int curRate = Application.targetFrameRate;
            switch (curRate)
            {
                case 30:
                    Application.targetFrameRate = 60;
                    break;
                default:
                case 60:
                    Application.targetFrameRate = 60;
                    break;
                case 90:
                    Application.targetFrameRate = 60;
                    break;
                case 120:

                    break;
            }

        }
        #endif
    }
}
