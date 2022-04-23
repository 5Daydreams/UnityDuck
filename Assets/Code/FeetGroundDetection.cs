using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeetGroundDetection : MonoBehaviour
{
    public PlayerMovement playerBody;
    public GameObject landingParticles;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject particleThingy = Instantiate(landingParticles,this.transform.position, Quaternion.identity);
        
        Destroy(particleThingy, 1.5f);

        playerBody.isGrounded = true;
    }
}
