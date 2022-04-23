using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScore : MonoBehaviour
{
    public Text textBox;
    public string scorePretext;
    public IntValue currentScore;

    private void Awake()
    {
        currentScore.SetValue(0);
    }

    void OnGUI()
    {
        textBox.text = scorePretext + currentScore.Value;
    }
}
