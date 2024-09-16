using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordScript : MonoBehaviour
{
    PlayerScript _mainPlayerScript;
    Transform _transform;
    Queue<GameObject> _hitParticles;
    public GameObject _hitParticlesPrefab;

    void Start()
    {
        _mainPlayerScript = transform.root.gameObject.GetComponent<PlayerScript>();
        _transform = transform;
        _hitParticles = new Queue<GameObject>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            _mainPlayerScript.RestoreMagic(0.5f);

            _hitParticles.Enqueue(Instantiate(_hitParticlesPrefab, collision.ClosestPoint(_transform.position), Quaternion.identity));
            Invoke("DeleteParticles", 1);
        }
    }

    void DeleteParticles()
    {
        Destroy(_hitParticles.Dequeue());
    }
}
