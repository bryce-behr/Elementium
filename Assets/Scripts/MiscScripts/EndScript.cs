using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScript : MonoBehaviour
{
    //SpriteRenderer _sr;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void ReturnToHub()
    {
        PPrefsList.ResetData();
        SceneManager.LoadScene("HomeScene");
    }
}
