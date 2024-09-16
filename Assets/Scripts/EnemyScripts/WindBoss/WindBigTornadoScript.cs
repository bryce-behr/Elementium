using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindBigTornadoScript : MonoBehaviour
{
    public Transform _playerTransform;
    Transform _transform;

    bool _canGo = false;

    // Start is called before the first frame update
    void Start()
    {
        _transform = transform;

        Invoke("Go", 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (_canGo)
        {
            _transform.position = Vector3.MoveTowards(_transform.position, _playerTransform.position, 3 * Time.deltaTime);
        }
    }

    void Go()
    {
        _canGo = true;
    }
}
