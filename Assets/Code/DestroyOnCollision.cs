using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DestroyOnCollision : MonoBehaviour
{
    public UnityEvent onTriggerCollision;
    public UnityEvent<bool> onTriggerCollisionWithBool;
    public UnityEvent<Color> onTriggerCollisionWithColor;
    public UnityEvent<AnimationClip> onTriggerCollisionWithAnimClip;
    public Color closedColor;
    public Color openColor;
    
    private bool isDoorOpen = false;

    // Happens once on the frame when there is a new object inside this object's collider
    private void OnTriggerEnter2D(Collider2D other)
    {
        // This event was setup in the inspector!!
        //onTriggerCollision.Invoke();

        // Idea for the homework, if you want to: 
        // Make the door open when you touch the door with the bouncy ball :v
        this.transform.parent = other.transform;
        return;

        if(isDoorOpen == true)
        {
            onTriggerCollisionWithColor.Invoke(openColor);
        }
        else
        {
            onTriggerCollisionWithColor.Invoke(closedColor);
        }

        onTriggerCollisionWithBool.Invoke(!isDoorOpen); // <-- this line
        //gameObject.SetActive(!isDoorOpen); // <-- same as this
        /* assuming gameObject is the door's game object */
        isDoorOpen = !isDoorOpen;

        // Destroys the OTHER object
        //GameObject targetToDestroy = other.gameObject;
        //Destroy(targetToDestroy);


        onTriggerCollisionWithAnimClip.Invoke(new AnimationClip()); 
    }

    // Happens every frame when there is an object inside this object's collider
    private void OnTriggerStay2D(Collider2D collision)
    {
        
    }

    // Happens once on the frame where another object leaves this object's collider
    private void OnTriggerExit2D(Collider2D collision)
    {
        
    }
}
