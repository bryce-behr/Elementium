using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScript : MonoBehaviour
{
    public Sprite _jokeText;
    SpriteRenderer _sr;
    public SoundEffectsScript _soundEffectsScript;

    // Start is called before the first frame update
    void Start()
    {
        _sr = GetComponent<SpriteRenderer>();

        if (Random.Range(0, 50) == 0)
        {
            _sr.sprite = _jokeText;
        }

        //_soundEffectsScript = GetComponent<SoundEffectsScript>();
        _soundEffectsScript.PlayerFailure();
    }

    public void ReturnToHub()
    {
        SceneManager.LoadScene("HomeScene");
    }
}
