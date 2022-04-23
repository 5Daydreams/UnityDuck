using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/GlobalValues/T")]
public abstract class GlobalValue<T> : ScriptableObject
{
    public T Value;
}
