using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarGoal : MonoBehaviour
{
    public SceneTransition victorySceneTransition;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        victorySceneTransition.TransitionToScene();
    }
}
