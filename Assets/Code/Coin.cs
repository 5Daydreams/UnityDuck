using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class Coin : MonoBehaviour
{
    public UnityEvent onTriggerCollision;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        onTriggerCollision.Invoke();

        Destroy(this.gameObject);
    }
}
