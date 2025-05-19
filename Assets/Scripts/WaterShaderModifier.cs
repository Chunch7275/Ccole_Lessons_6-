using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA;

public class WaterShaderModifier : MonoBehaviour
{
    // Start is called before the first frame update

    private Renderer rend;

    void Start()
    {
        rend= GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        rend.material.SetColor("_HighlightEdgeColor", Color.yellow);
    }
}
