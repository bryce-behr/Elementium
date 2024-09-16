using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathWallScript : MonoBehaviour
{
    Transform _transform;
    float yScale;
    float xScale = 0;

    // Start is called before the first frame update
    void Start()
    {
        _transform = transform;
        yScale = _transform.localScale.y;
    }

    // Update is called once per frame
    void Update()
    {
        xScale += 0.6f * Time.deltaTime;
        _transform.localScale = new Vector3(xScale, yScale, 1);
    }
}
