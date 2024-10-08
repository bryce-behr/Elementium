using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirebossSpinnerScript : MonoBehaviour
{
    Transform _transform;

    float _scale = 0;
    float _angle = 0;

    bool _scaledUp = false;
    
    // Start is called before the first frame update
    void Awake()
    {
        _transform = transform;
        _transform.localScale = new Vector3(0, 0, 1);
    }

    // Update is called once per frame
    void Update()
    {
        _angle = (_angle + (30 * Time.deltaTime)) % 360;
        _scale += 0.25f * Time.deltaTime;

        if (_scale >= 1)
        {
            _transform.localScale = new Vector3(1, 1, 1);
            _scaledUp = true;
        }

        if (!_scaledUp)
        {
            _transform.localScale = new Vector3(_scale, _scale, 1);
        }
        _transform.rotation = Quaternion.Euler(0, 0, _angle);
    }
}
