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
                default:
                case 30:
                    Application.targetFrameRate = 60;
                    break;
                case 60:
                    Application.targetFrameRate = 90;
                    break;
                case 90:
                    Application.targetFrameRate = 120;
                    break;
                case 120:
                    Application.targetFrameRate = 30;
                    break;
            }

        }
        #endif
    }
}
