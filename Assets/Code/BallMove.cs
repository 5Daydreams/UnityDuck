using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMove : MonoBehaviour
{
    void MoveLeft()
    {
        if (Input.GetKey("a"))
        {
            gameObject.transform.position += Vector3.left * Time.deltaTime;
        }
    }

    float AddTwoNumbers(float a, float b)
    {
        float c = a + b;

        return c;
    }

    void Update()
    {
        MoveLeft();

        AddTwoNumbers(4,3);

        if (Input.GetKey("d"))
        {
            gameObject.transform.position += Vector3.right * Time.deltaTime;
        }
    }
}
