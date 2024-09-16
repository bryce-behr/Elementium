using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BathScript : MonoBehaviour
{

    public SpriteRenderer _headSR;
    public SpriteRenderer _leftSR;
    public SpriteRenderer _rightSR;
    public SpriteRenderer _swordSR;
    public GameObject _wings;

    public Sprite _originalHead;
    public Sprite _originalLeft;
    public Sprite _originalRight;
    public Sprite _originalSword;

    public PlayerScript _ps;

    ParticleSystem _particles;

    // Start is called before the first frame update
    void Start()
    {
        _particles = GetComponent<ParticleSystem>();
        _particles.Stop();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        _ps._enableMovement = false;
        _particles.Play();
        Invoke("Reset", 5);
    }

    void Reset()
    {
        _headSR.sprite = _originalHead;
        _leftSR.sprite = _originalLeft;
        _rightSR.sprite = _originalRight;
        _swordSR.sprite = _originalSword;
        _wings.SetActive(false);
        PPrefsList.ResetData();
        _ps._enableMovement = true;
    }


}
