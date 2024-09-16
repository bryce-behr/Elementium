using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerScript : MonoBehaviour
{
    //Components
    Rigidbody2D _rbody;
    Transform _transform;
    Animator _animator;
    PlayerCosmeticScript _pcs;

    Camera _mainCam;

    //Animation variables
    public bool _isCommitted = false;
    public bool _isDashing = false;
    public bool _isCharging = false;
    public bool _isCharged = false;

    //sound effects objects
    public GameObject _sceneManager;
    private SoundEffectsScript _soundEffectsScript;

    float _PLAYER_SPEED = 10;

    //Bar variables
    public GameObject _UIPrefab;
    private GameObject _UI;
    private GameObject _staminaBar;
    float _maxHealth;
    float _currentHealth;
    float _maxMagic;
    float _currentMagic;
    float _maxStamina;
    float _currentStamina;
    bool _outOfStamina = false;

    //Hit variables
    float _lastHitTime = 0;
    float _invincibilityTime = 0;

    public bool _invincible = false;

    //Spell variables
    bool _hasFire;
    bool _hasLightning;
    bool _hasWind;
    public GameObject _playerLightningPrefab;
    GameObject _currentLightning;
    public GameObject _playerFirePrefab;

    // Enable and disable movement
    public bool _enableMovement = true;


    /* ~~ NOTE ON PLAYER ATTACKS ~~
     * The player's sword has a trigger hitbox. This hitbox is only active during attack animations.
     * The sword's tag changes based on what attack is currently being used so that the boss
     * knows which attack it has been hit with.
     */

    /* ~~ Note on _isCommitted ~~
     * The idea is that when starting an attack or dodge, this variable is set to true.
     * This prevents the player from doing anything else, like moving or starting new attacks.
     * After a set amount of time, this variable will be set to false, and the player can move again.
     */

    //Set up player
    void Awake()
    {
        //sound effects
        _soundEffectsScript = _sceneManager.GetComponent<SoundEffectsScript>();

        //DEBUG
        Application.targetFrameRate = -1;
        //Application.targetFrameRate = 30;
        QualitySettings.vSyncCount = 0;
        //END DEBUG

        _rbody = GetComponent<Rigidbody2D>();
        _transform = transform;
        _animator = _transform.GetChild(0).gameObject.GetComponent<Animator>();
        _pcs = GetComponent<PlayerCosmeticScript>();

        _mainCam = Camera.main;

        //Set player stats
        RefreshPlayerStats(null);
        _currentHealth = _maxHealth;
        _maxMagic = 10; //PPrefsList.GetValue(PPrefsList.MaxMagic, 10);
        _currentMagic = _maxMagic;
        _maxStamina = 10;
        _currentStamina = _maxStamina;

        //Instantiate Player UI
        _UI = Instantiate(_UIPrefab);
        _UI.GetComponent<PlayerUIScript>()._player = gameObject;
        _UI.transform.SetParent(GameObject.Find("Canvas").transform, false);
        _staminaBar = _UI.transform.GetChild(2).gameObject;
    }

    // Player Movement
    void FixedUpdate()
    {
        if (!_enableMovement)
        {
            _rbody.velocity = Vector2.zero;
            return;
        }

        //Allow the player to turn while charging.
        if (_animator.GetBool("Charge") && !_animator.GetBool("ChargeAttack"))
        {
            //Point the player towards the mouse.
            Vector2 mousePos = _mainCam.ScreenToWorldPoint(Input.mousePosition);
            float playerAngle = Mathf.Atan2(mousePos.y - _transform.position.y, mousePos.x - _transform.position.x) * Mathf.Rad2Deg;
            playerAngle -= 90;
            _transform.rotation = Quaternion.Euler(new Vector3(0, 0, playerAngle));
        }

        if (!_isCommitted)
        {
            //Get player movement.
            float xVel = Input.GetAxisRaw("Horizontal");
            float yVel = Input.GetAxisRaw("Vertical");

            //Set the player's velocity.
            _rbody.velocity = new Vector2(xVel, yVel).normalized * _PLAYER_SPEED;

            //Point the player towards the mouse.
            Vector2 mousePos = _mainCam.ScreenToWorldPoint(Input.mousePosition);
            float playerAngle = Mathf.Atan2(mousePos.y - _transform.position.y, mousePos.x - _transform.position.x) * Mathf.Rad2Deg;
            playerAngle -= 90;
            _transform.rotation = Quaternion.Euler(new Vector3(0, 0, playerAngle));
        }
        else
        {
            if (!_isDashing)
            {
                _rbody.velocity = Vector2.zero;
            }
        }
    }

    // Player Attacks
    void Update()
    {
        //START DEBUG
        /*if (Input.GetKeyDown(KeyCode.P))
        {
            //PPrefsList.ResetData();
        }
        else if (Input.GetKeyDown(KeyCode.I))
        {
            PPrefsList.SetBool(PPrefsList.DefeatedFinal, false);
        }*/
        //END DEBUG

        if (_isCharging && !Input.GetKey(KeyCode.Mouse0))
        {
            _isCharging = false;

            if (_isCharged)
            {
                _animator.SetBool("ChargeAttack", true);
                _isCharged = false;
            }
            else
            {
                _animator.SetBool("Charge", false);
            }
        }

        if (!_isCommitted)
        {
            //Player basic attack.
            if (Input.GetKeyDown(KeyCode.Mouse0) && !_isCharging && !_outOfStamina)
            {
                _currentStamina -= 3.4f;
                _isCommitted = true;
                _animator.SetBool("DoBasicAttack", true);
                _isCharging = true;
            }
            //Charge Attack
            else if (_isCharging)
            {
                _isCommitted = true;
                _animator.SetBool("Charge", true);
            }
            //Dash
            else if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                if (_hasWind || !_outOfStamina)
                {
                    if (_rbody.velocity.magnitude > 0)
                    {
                        if (!_hasWind)
                        {
                            _currentStamina -= 4;
                        }

                        _soundEffectsScript.Dash();
                        //Set the player's dash velocity.
                        _isDashing = true;
                        _rbody.velocity = _rbody.velocity.normalized * _PLAYER_SPEED * 3;

                        //Rotate the player in the direction of the dash.
                        float playerAngle = Mathf.Atan2(_rbody.velocity.y, _rbody.velocity.x) * Mathf.Rad2Deg;
                        playerAngle -= 90;
                        _transform.rotation = Quaternion.Euler(0, 0, playerAngle);

                        //Commit the player to the dash.
                        _isCommitted = true;
                        _animator.SetBool("DoDash", true);
                    }
                }
            }
            //Heal Spell
            else if (Input.GetKeyDown(KeyCode.F))
            {
                if (_currentMagic >= 5)
                {
                    _soundEffectsScript.HealSpell();
                    _currentMagic -= 5;
                    _isCommitted = true;
                    _animator.SetBool("DoHeal", true);
                }
            }
            //Fire Spell
            else if (Input.GetKeyDown(KeyCode.E) && _hasFire)
            {
                if (_currentMagic >= 5)
                {
                    _soundEffectsScript.FireAttack();
                    _currentMagic -= 5;
                    _isCommitted = true;
                    //Instantiate(_playerFirePrefab, _transform.position, _transform.rotation);
                    _animator.SetBool("DoFire", true);
                }
            }
            //Lightning Spell
            else if (Input.GetKeyDown(KeyCode.Mouse1) && _hasLightning)
            {
                if (_currentMagic >= 2.5f)
                {
                    _soundEffectsScript.PlayerElectricAttack();
                    _currentMagic -= 2.5f;
                    _isCommitted = true;
                    _currentLightning = Instantiate(_playerLightningPrefab, _transform);
                    _animator.SetBool("DoLightning", true);
                    Invoke("DeleteLightning", 0.9f);
                }
            }
        }
        else
        {
            //Dash Attack
            if (_isDashing && Input.GetKeyDown(KeyCode.Mouse0) && !_outOfStamina)
            {
                _currentStamina -= 2;

                _isDashing = false;
                _animator.SetBool("DoDashAttack", true);
            }
        }

        //Regenerate stamina
        if (_currentStamina <= 0)
        {
            _currentStamina = 0;
            _outOfStamina = true;
            _staminaBar.GetComponent<Image>().color = Color.red;
            _staminaBar.transform.GetChild(0).GetComponent<Image>().color = new Color(.5f, 0, 0);
        }
        if (!_isCharging)
        {
            if (_outOfStamina)
            {
                _currentStamina += (10f / 3f) * Time.deltaTime;
            }
            else
            {
                _currentStamina += (10f / 7f) * Time.deltaTime;
            }
            if (_currentStamina >= _maxStamina)
            {
                _currentStamina = _maxStamina;
                _outOfStamina = false;
                _staminaBar.GetComponent<Image>().color = Color.black;
                _staminaBar.transform.GetChild(0).GetComponent<Image>().color = new Color(0, 0.6981132f, 0.05864924f);
            }
        }
    }

    public void RefreshPlayerStats(Sprite levelUpDetails)
    {
        _hasFire = PPrefsList.GetBool(PPrefsList.DefeatedFire, false);
        _hasLightning = PPrefsList.GetBool(PPrefsList.DefeatedLightning, false);
        _hasWind = PPrefsList.GetBool(PPrefsList.DefeatedWind, false);

        _pcs.ChangeSprites(PPrefsList.GetBool(PPrefsList.DefeatedLightning, false),
            PPrefsList.GetBool(PPrefsList.DefeatedFire, false),
            PPrefsList.GetBool(PPrefsList.DefeatedWind, false));

        _maxHealth = _hasLightning ? 15 : 10;
        _PLAYER_SPEED = _hasWind ? 15 : 10;

        if (levelUpDetails != null)
        {
            GameObject detailBox = _UI.transform.GetChild(3).gameObject;
            detailBox.GetComponent<Image>().sprite = levelUpDetails;
            detailBox.SetActive(true);
        }
    }

    void DeleteLightning()
    {
        Destroy(_currentLightning);
    }

    public void HealPlayer(float amount)
    {
        _currentHealth += amount;
        if (_currentHealth > _maxHealth)
        {
            _currentHealth = _maxHealth;
        }
    }

    public void RestoreMagic(float amount)
    {
        _currentMagic += amount;
        if (_currentMagic >= _maxMagic)
        {
            _currentMagic = _maxMagic;
        }
    }

    // Getting Hit
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Time.time - _lastHitTime <= _invincibilityTime)
        {
            return;
        }

        if (_invincible)
        {
            return;
        }

        bool gotHit = false;

        if (collision.CompareTag(TagList.ExampleEnemyAttack))
        {
            _currentHealth -= 2;
        }

        if (collision.CompareTag(TagList.BasicEnemySword))
        {
            _soundEffectsScript.TakeDamage();
            _currentHealth -= 3;
            gotHit = true;
            _invincibilityTime = 1;
            _lastHitTime = Time.time;
        }

        if (collision.CompareTag(TagList.LightningAOEAttack))
        {
            _currentHealth -= 2;
            gotHit = true;
            _invincibilityTime = 1;
            _lastHitTime = Time.time;
        }

        if (collision.CompareTag(TagList.LightningBossShell))
        {
            _currentHealth -= 2;
            gotHit = true;
            _invincibilityTime = 0.5f;
            _lastHitTime = Time.time;
        }

        if (collision.CompareTag(TagList.LightningBeamAttack))
        {
            _currentHealth -= 2;
            gotHit = true;
            _invincibilityTime = 0.5f;
            _lastHitTime = Time.time;
        }

        if (collision.CompareTag(TagList.FireBossSlam))
        {
            _currentHealth -= 4;
            gotHit = true;
            _invincibilityTime = 1;
            _lastHitTime = Time.time;
        }

        if (collision.CompareTag(TagList.FireBossSpin))
        {
            _currentHealth -= 8;
            gotHit = true;
            _invincibilityTime = 1;
            _lastHitTime = Time.time;
        }

        if (collision.CompareTag(TagList.FireBossFlamethrower))
        {
            _currentHealth -= 2;
            gotHit = true;
            _invincibilityTime = 1;
            _lastHitTime = Time.time;
        }

        if (collision.CompareTag(TagList.Firepit))
        {
            _currentHealth -= 0.1f;
            gotHit = true;
            _invincibilityTime = 0;
        }

        if (collision.CompareTag(TagList.WindBigTornado))
        {
            _currentHealth -= 0.5f;
            gotHit = true;
            _invincibilityTime = 0;
        }

        if (collision.CompareTag(TagList.WindSlam))
        {
            _currentHealth -= 5;
            gotHit = true;
            _invincibilityTime = 1;
            _lastHitTime = Time.time;
        }

        if (collision.CompareTag(TagList.WindBoomerang))
        {
            _currentHealth -= 3;
            gotHit = true;
            _invincibilityTime = 1;
            _lastHitTime = Time.time;
        }

        if (collision.CompareTag(TagList.WindSpear))
        {
            _currentHealth -= 2;
            gotHit = true;
            _invincibilityTime = 1;
            _lastHitTime = Time.time;
        }

        if (collision.CompareTag(TagList.FinalBeam))
        {
            _currentHealth -= 5;
            gotHit = true;
            _invincibilityTime = 1;
            _lastHitTime = Time.time;
        }

        if (collision.CompareTag(TagList.FinalTounge))
        {
            _currentHealth -= 10;
            gotHit = true;
            _invincibilityTime = 1;
            _lastHitTime = Time.time;
        }

        if (gotHit)
        {
            _soundEffectsScript.TakeDamage();
            _pcs.GetHurt(_invincibilityTime);
        }

        if (_currentHealth <= 0)
        {
            Destroy(_UI);
            _currentHealth = 0;
            SceneManager.LoadScene("DeathScene");
        }


    }

    public float GetHealthPercent()
    {
        return _currentHealth/_maxHealth;
    }

    public float GetMagicPercent()
    {
        return _currentMagic/_maxMagic;
    }

    public float GetStaminaPercent()
    {
        return _currentStamina/_maxStamina;
    }
}
