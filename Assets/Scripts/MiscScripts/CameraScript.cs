using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    public GameObject _player;
    Transform _playerTransform;
    Transform _cameraTransform;
    public int _cameraYOffset; // NOTE: set before running the game
    Vector3 _cameraOffsetVec;

    void Start()
    {
        // cacheing transforms into variables
        _playerTransform = _player.transform;
        _cameraTransform = transform;
        _cameraOffsetVec = new Vector3(0, _cameraYOffset, -10); // -10 ensures everything seen
    }

    void Update()
    {
        // keeps camera
        _cameraTransform.position = _playerTransform.position + _cameraOffsetVec;
    }
}
