using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MusicScript : MonoBehaviour
{
    private AudioSource _combat;
    private AudioSource _peaceful;
    public AudioClip _combatClip;
    public AudioClip _peacefulClip;

    private bool _fadeOut;

    // Start is called before the first frame update
    void Start()
    {
        _fadeOut = false;

        //set up combat music 
        _combat = gameObject.AddComponent<AudioSource>();
        _combat.clip = _combatClip;
        _combat.volume = 0.2f;
        _combat.loop = true;

        //set up peaceful music
        _peaceful = gameObject.AddComponent<AudioSource>();
        _peaceful.clip = _peacefulClip;
        _peaceful.loop = true;

        string _sceneName = SceneManager.GetActiveScene().name;
        if (_sceneName == "FireBossRoom") FireBossMusic();
        else if (_sceneName == "LightningBossRoom") LightningBossMusic();
        else if (_sceneName == "WindBossRoom") WindBossMusic();
        else if (_sceneName == "FinalBoss") FinalBossMusic();
        else if (_sceneName == "HomeScene") HubMusic();
        else if (_sceneName == "TutorialZone") TutorialMusic();
    }

    private void FixedUpdate()
    {
        if(_fadeOut)
        {
            _combat.volume -= 0.1f;
        }
    }

    void FireBossMusic()
    {
        if (PPrefsList.GetBool(PPrefsList.DefeatedFire, true))
        {
            _peaceful.Play();
        }
        else
        {
            _combat.Play();
        }
    }

    void LightningBossMusic()
    {
        if (PPrefsList.GetBool(PPrefsList.DefeatedLightning, true))
        {
            _peaceful.Play();
        }
        else
        {
            _combat.Play();
        }
    }

    void FinalBossMusic()
    {
        if (PPrefsList.GetBool(PPrefsList.DefeatedFinal, true))
        {
            _peaceful.Play();
        }
        else
        {
            _combat.Play();
        }
    }

    void WindBossMusic()
    {
        if (PPrefsList.GetBool(PPrefsList.DefeatedWind, true))
        {
            _peaceful.Play();
        }
        else
        {
            _combat.Play();
        }
    }

    void HubMusic()
    {
        _peaceful.Play();
    }

    void TutorialMusic()
    {
        _peaceful.Play();
    }

    public void FadeOutBossMusic()
    {
        _fadeOut = true;
    }
}
