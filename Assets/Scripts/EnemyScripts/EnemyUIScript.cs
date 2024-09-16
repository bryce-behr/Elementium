using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUIScript : MonoBehaviour
{
    public GameObject _enemy;
    
    private Transform _transform;
    private PlayerScript _enemyScript;
    private Slider _healthSlider;


    // Start is called before the first frame update
    void Start()
    {
        _transform = transform;
        _enemyScript = _enemy.GetComponent<PlayerScript>();
        _healthSlider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        _healthSlider.value = _enemyScript.GetHealthPercent();
    }
}
