using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour
{

    public GameObject _pauseMenuPanel;

    bool isEnabled = false;
    public bool canPause = true;

    // Start is called before the first frame update
    void Start()
    {
        _pauseMenuPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!canPause && isEnabled)
        {
            ResumeButton();
            return;
        }
        else if (!canPause)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isEnabled)
            {
                _pauseMenuPanel.SetActive(true);
                Time.timeScale = 0;
                isEnabled = true;
            }
            else
            {
                ResumeButton();
            }
        }
    }

    public void ResumeButton()
    {
        _pauseMenuPanel.SetActive(false);
        Time.timeScale = 1;
        isEnabled = false;
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("HomeScene");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
