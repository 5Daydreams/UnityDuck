using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate2D : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(Vector3.forward, Time.deltaTime * 20.0f);
        transform.localScale = Vector3.one * (0.7f + 0.4f * Mathf.Sin(1.5f*Time.time));
    }
}
