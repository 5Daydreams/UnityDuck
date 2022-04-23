using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorKey : MonoBehaviour
{
    public DoorLock door;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        door.Unlock();

        Destroy(this.gameObject);
    }
}
