using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossDefeatManagerScript : MonoBehaviour
{
    public bool _isFinalBoss;
    public DoorScript _doorScript;


    public void BossDefeated()
    {
        if (_isFinalBoss)
        {
            Invoke("PlayEnd", 1);
        }
        else
        {
            if (_doorScript != null)
            {
                _doorScript.OpenDoor();
            }
        }
    }

    void PlayEnd()
    {
        SceneManager.LoadScene("EndScene");
    }
}
