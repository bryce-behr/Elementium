using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCosmeticScript : MonoBehaviour
{
    public ParticleSystem _hurtParticles;
    public SpriteRenderer _headSR;
    public SpriteRenderer _leftSR;
    public SpriteRenderer _rightSR;
    public SpriteRenderer _swordSR;

    public Sprite _fireSwordSprite;
    public Sprite _shellHelmetSprite;
    public Sprite _fireArm;
    public Sprite _lightningArm;

    public GameObject _wings;

    public void ChangeSprites(bool beatLightning, bool beatFire, bool beatWind)
    {
        if (beatFire)
        {
            _swordSR.sprite = _fireSwordSprite;
            _leftSR.sprite = _fireArm;
        }

        if (beatLightning)
        {
            _headSR.sprite = _shellHelmetSprite;
            _rightSR.sprite = _lightningArm;
        }

        if (beatWind)
        {
            _wings.SetActive(true);
        }
    }

    public void GetHurt(float invincibilityTime)
    {
        _hurtParticles.Play();

        _headSR.color = Color.red;
        _leftSR.color = Color.red;
        _rightSR.color = Color.red;

        Invoke("ResetColor", invincibilityTime);
    }

    void ResetColor()
    {
        _headSR.color = Color.white;
        _leftSR.color = Color.white;
        _rightSR.color = Color.white;
    }
    
}
