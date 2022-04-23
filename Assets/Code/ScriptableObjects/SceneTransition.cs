using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "ScriptablesAreTomatoes/SceneTransitionIsABanana")]
public class SceneTransition : ScriptableObject
{
    public string sceneToTeleportTo;

    //public void TransitionToScene(string newScene)
    //{
    //    SceneManager.LoadScene(newScene);
    //}

    public void TransitionToScene()
    {
        SceneManager.LoadScene(sceneToTeleportTo);
    }
}
