using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballScript : MonoBehaviour
{
    public GameObject _firepitPrefab;


    Vector2 _targetPos;
    bool _canGo = false;

    Rigidbody2D _rbody;
    Transform _transform;
    Vector2 _startingPlace;

    const float FIREBALL_SPEED = 20;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetTarget(float x, float y)
    {
        _transform = transform;
        _rbody = GetComponent<Rigidbody2D>();

        _startingPlace = _transform.position;
        _targetPos = new Vector2(x, y);
        _rbody.velocity = new Vector2(x - _transform.position.x, y - _transform.position.y).normalized * FIREBALL_SPEED;

        _canGo = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_canGo)
        {
            return;
        }
        
        if (Vector2.Distance(_transform.position, _startingPlace) >= Vector2.Distance(_targetPos, _startingPlace))
        {
            Instantiate(_firepitPrefab, _transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
