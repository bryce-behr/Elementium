using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewTestDummyScript : MonoBehaviour
{
    public GameObject _leftArm;
    SpriteRenderer _leftArmSR;
    public GameObject _rightArm;
    SpriteRenderer _rightArmSR;
    public GameObject _head;
    SpriteRenderer _headSR;
    public GameObject _sceneManager;
    private SoundEffectsScript _soundEffectsScript;

    // Start is called before the first frame update
    void Start()
    {
        _soundEffectsScript = _sceneManager.GetComponent<SoundEffectsScript>();
        _leftArmSR = _leftArm.GetComponent<SpriteRenderer>();
        _rightArmSR = _rightArm.GetComponent<SpriteRenderer>();
        _headSR = _head.GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag(TagList.PlayerBasicAttack))
        {
            SetColor(Color.red);
            print("Ouch!");
            Invoke("ResetColor", 0.1f);
            _soundEffectsScript.SwordClash();
        }

        if (collider.CompareTag(TagList.PlayerDashAttack))
        {
            SetColor(Color.red);
            print("Ouch!");
            Invoke("ResetColor", 0.1f);
            _soundEffectsScript.SwordClash();
        }

        if (collider.CompareTag(TagList.PlayerChargeAttack))
        {
            SetColor(Color.red);
            print("Ouch!");
            Invoke("ResetColor", 0.1f);
            _soundEffectsScript.SwordClash();
        }

        if (collider.CompareTag(TagList.PlayerLightning))
        {
            SetColor(Color.red);
            print("Ouch!");
            Invoke("ResetColor", 0.1f);
        }

        if (collider.CompareTag(TagList.PlayerFire))
        {
            SetColor(Color.red);
            print("Ouch!");
            Invoke("ResetColor", 0.1f);
        }
    }

    void SetColor(Color color)
    {
        _leftArmSR.color = color;
        _rightArmSR.color = color;
        _headSR.color = color;
    }

    void ResetColor()
    {
        SetColor(Color.white);
    }

}
