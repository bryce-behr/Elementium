using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFireballScript : MonoBehaviour
{
    public GameObject _firepitPrefab;

    float _timeStart;

    Rigidbody2D _rbody;
    Transform _transform;

    const float FIREBALL_SPEED = 20;

    private void Start()
    {
        _transform = transform;
        _rbody = GetComponent<Rigidbody2D>();

        //TODO: This trig
        _rbody.velocity = _transform.up * FIREBALL_SPEED;

        _timeStart = Time.time;
    }

    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        Instantiate(_firepitPrefab, _transform.position, Quaternion.identity);
        Destroy(gameObject);
    }*/

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Instantiate(_firepitPrefab, _transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - _timeStart >= 5)
        {
            Destroy(gameObject);
        }
    }
}
