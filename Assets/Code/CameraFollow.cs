using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player;
    public float snapSpeed = 10.0f;

    // Update is called once per frame
    void LateUpdate()
    {
        if(player == null)
        {
            this.enabled = false;
            return; // Teleports from here --> ]]
        }

        this.transform.position = Vector3.Lerp(this.transform.position, player.transform.position + Vector3.back * 2, Time.deltaTime * snapSpeed);
        
    } // To here [[ -->
}
