using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIScript : MonoBehaviour
{
    public GameObject _player;
    private Transform _transform;
    private PlayerScript _playerScript;
    private Slider _healthSlider;
    private Slider _magicSlider;
    private Slider _staminaSlider;


    // Start is called before the first frame update
    void Start()
    {
        _transform = transform;
        _playerScript = _player.GetComponent<PlayerScript>();
        _healthSlider = transform.GetChild(0).GetComponent<Slider>();
        _magicSlider = transform.GetChild(1).GetComponent<Slider>();
        _staminaSlider = transform.GetChild(2).GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        _healthSlider.value = _playerScript.GetHealthPercent(); ;
        _magicSlider.value = _playerScript.GetMagicPercent();
        _staminaSlider.value = _playerScript.GetStaminaPercent();
    }
}
