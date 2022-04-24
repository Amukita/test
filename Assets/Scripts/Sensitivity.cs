using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Sensitivity : MonoBehaviour
{
    public Slider sensitivity;

    public void ApplySensitivity()
    {
        GetComponent<CameraHandler>().HandleCameraRotation(sensitivity.value, sensitivity.value, sensitivity.value);
    }
}
