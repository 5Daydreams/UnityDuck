using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLock : MonoBehaviour
{
    public Vector3 endPosition;

    private bool isUnlocked = false;

    public void Unlock()
    {
        isUnlocked = true;
    }

    private void Update()
    {
        if(isUnlocked)
        {
            this.transform.position = Vector3.Lerp(this.transform.position, endPosition, Time.deltaTime * 0.5f);
        }

        if(Vector3.Distance(this.transform.position,endPosition) < 0.1f)
        {
            isUnlocked = false;
        }
    }
}
