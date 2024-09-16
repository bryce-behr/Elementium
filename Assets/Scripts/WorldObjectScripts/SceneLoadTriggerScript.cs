using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadTriggerScript : MonoBehaviour
{
    public string sceneNameToLoad;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //NOTE make sure that the Scene is in Build Settings
        SceneManager.LoadScene(sceneNameToLoad);
    }
}
