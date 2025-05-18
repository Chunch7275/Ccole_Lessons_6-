using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class SkyboxRotation : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time);
    }
}
