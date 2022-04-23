using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/GlobalValues/int")]
public class IntValue : GlobalValue<int>
{
    public void SetValue(int newValue)
    {
        Value = newValue;
    }

    public void IncrementValue(int increment)
    {
        Value += increment;
    }
}
