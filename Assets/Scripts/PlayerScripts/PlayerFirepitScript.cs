using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFirepitScript : MonoBehaviour
{
    Transform _transform;

    float _scale = 5;

    // Start is called before the first frame update
    void Awake()
    {
        _transform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        _scale -= 1f * Time.deltaTime;
        _transform.localScale = new Vector3(_scale, _scale, 1);

        if (_scale <= 0)
        {
            Destroy(gameObject);
        }
    }
}
