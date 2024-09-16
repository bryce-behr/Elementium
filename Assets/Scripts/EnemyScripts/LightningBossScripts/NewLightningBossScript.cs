using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewLightningBossScript : BossScript
{
    //Personal Variables
    public bool _canRotate;
    float _angle = 0;

    //Dash variables
    float _dashX;
    float _dashY;
    Vector2 _dashTarget;
    Vector2 _startingPlace;
    bool _isDashing = false;
    int _MAX_ATTACKS_UNTIL_DASH = 3;
    int _attacksUntilDash = 2;

    //Public Components
    //public GameObject _player;
    public GameObject _head;
    public GameObject _arrow;
    //public GameObject _rageParticles;
    //public GameObject _defeatParticles;
    public GameObject _lightningAOEPrefab;
    public GameObject _lightningBlastPrefab;
    public Sprite _tiredSprite;
    public Sprite _brokenShellSprite;
    //public GameObject _healthBar;
    //public Text _name;

    //Private Components
    Animator _shellAnimator;
    Animator _headAnimator;
    //Transform _transform;
    Transform _headTransform;
    //Transform _playerTransform;
    Rigidbody2D _rbody;
    private GameObject[] _AOEStrikes;
    private GameObject _lightningBlast;
    //private GameObject _UI;

    Transform _innerBarTransform;

    //Attack Variables
    //public bool _canFight = true;
    private float _colorTracker;
    private bool _strikeZoneActive;
    private bool _activeLightningBlast;
    private int _numAOEStrikes;
    private int _maxAOESize;
    private bool _activeAOEAttack;

    float _aoeScale;

    //BossDefeatManagerScript _bdms;
    //public GameObject _bossDefeatManager;

    void Start()
    {
        InitBoss(70, 30);

        if (PPrefsList.GetBool(PPrefsList.DefeatedLightning, false))
        {
            RemoveBossInfo();
            Destroy(gameObject);
            return;
        }

        _rbody = GetComponent<Rigidbody2D>();
        _shellAnimator = _transform.GetChild(0).gameObject.GetComponent<Animator>();
        _headAnimator = _transform.GetChild(1).gameObject.GetComponent<Animator>();
        _headTransform = _head.transform;
        _canRotate = true;

        //Lightning Attacks
        _colorTracker = 0;
        _strikeZoneActive = false;
        _activeAOEAttack = false;
        _activeLightningBlast = false;

        //initialize CUSTOMIZABLE vars
        _numAOEStrikes = 3;
        _maxAOESize = 4;

    }

    void Update()
    {
        if (!canFight)
            return;

        base.Update();

        if (!PPrefsList.GetBool(PPrefsList.DefeatedLightning, false))
            _UI.GetComponent<Slider>().value = GetHealthPercent();

        
        //Rotate head to face the player.
        if (_canRotate)
        {
            _angle = Mathf.Atan2(playerTransform.position.y - _transform.position.y, playerTransform.position.x - _transform.position.x) * Mathf.Rad2Deg;
        }
        if (_angle > 90 || _angle < -90)
        {
            _headTransform.rotation = Quaternion.Euler(new Vector3(0, 180, 180 - _angle));
        }
        else
        {
            _headTransform.rotation = Quaternion.Euler(new Vector3(0, 0, _angle));
        }

        //Stop dashing
        if (_isDashing && Vector2.Distance(_transform.position, _startingPlace) >= Vector2.Distance(_dashTarget, _startingPlace))
        {
            _isDashing = false;
            ShowFace();
            _rbody.velocity = Vector2.zero;
            cooldown = Time.time - lastAttackTime + (rageMode ? 0.5f : 1);
            _shellAnimator.SetBool("DoShake", false);
        }

        if (_strikeZoneActive)
        {
            LightningAOEMaintain();
        }

        // CONTROL BOSS ATTACKS
        if (Time.time - lastAttackTime >= cooldown)
        {
            if (Vector2.Distance(playerTransform.position, _transform.position) > 10)
            {
                if (_attacksUntilDash <= 0)
                {
                    //Dash
                    HideFace();
                    _shellAnimator.SetBool("DoShake", true);
                    _attacksUntilDash = Random.Range(1, _MAX_ATTACKS_UNTIL_DASH + 2);

                    _dashX = Random.Range(-15f, 15f);
                    _dashY = Random.Range(-15f, 15f);
                    _angle = Mathf.Atan2(_dashY - _transform.position.y, _dashX - _transform.position.x) * Mathf.Rad2Deg;
                    _arrow.SetActive(true);
                    _dashTarget = new Vector2(_dashX, _dashY);

                    cooldown = 99;
                    Invoke("Dash", rageMode ? 0.8f : 1.3f);
                }
                else
                {
                    //AOE
                    _numAOEStrikes = rageMode ? 7 : 3;
                    LightningAOEAttack();
                    _shellAnimator.SetBool("DoAOE", true);
                    HideFace();
                    cooldown = rageMode ? 4 : 4;
                    Invoke("ShowFace", cooldown - 1);
                }
            }
            else
            {
                if (_attacksUntilDash <= 0)
                {
                    //Dash
                    HideFace();
                    _shellAnimator.SetBool("DoShake", true);
                    _attacksUntilDash = Random.Range(1, _MAX_ATTACKS_UNTIL_DASH + 2);

                    _dashX = Random.Range(-15f, 15f);
                    _dashY = Random.Range(-15f, 15f);
                    _angle = Mathf.Atan2(_dashY - _transform.position.y, _dashX - _transform.position.x) * Mathf.Rad2Deg;
                    _arrow.SetActive(true);
                    _dashTarget = new Vector2(_dashX, _dashY);

                    cooldown = 99;
                    Invoke("Dash", rageMode ? 0.8f : 1.3f);
                }
                else
                {
                    //Spit
                    _headAnimator.SetBool("DoSpit", true);
                    cooldown = rageMode ? 3 : 3;
                }
            }

            _attacksUntilDash--;
            lastAttackTime = Time.time;
        }
        if (Time.time - lastAttackTime >= 1)
        {
            _shellAnimator.SetBool("DoAOE", false);
            _headAnimator.SetBool("DoSpit", false);
        }
    }

    override protected void EnterRageMode()
    {
        _transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = _brokenShellSprite;
        DestroyStrikeZones();
        _maxAOESize = 5;
        _MAX_ATTACKS_UNTIL_DASH = 2;
    }

    override protected void BossDeath()
    {
        PPrefsList.SetBool(PPrefsList.DefeatedLightning, true);
        /*PlayerPrefs.SetInt(PPrefsList.MaxHealth, PPrefsList.GetValue(PPrefsList.MaxHealth, 10) + 10);
        PlayerPrefs.SetInt(PPrefsList.MaxMagic, PPrefsList.GetValue(PPrefsList.MaxMagic, 10) + 10);*/
        DestroyStrikeZones();
        RemoveBossInfo();
    }

    public void TakeDamage(Collider2D collision)
    {
        //currentHealth -= damage;
        base.TakeDamage(collision);
    }

    public void HideFace()
    {
        _headAnimator.SetBool("HideFace", true);
        _canRotate = false;
    }

    public void ShowFace()
    {
        _headAnimator.SetBool("HideFace", false);
        _canRotate = true;
    }

    public void StopRotate()
    {
        _canRotate = false;
    }

    public void AllowRotate()
    {
        _canRotate = true;
    }

    void Dash()
    {
        _isDashing = true;
        _arrow.SetActive(false);
        soundEffects.Dash();

        _startingPlace = _transform.position;
        _rbody.velocity = new Vector2(_dashX - _transform.position.x, _dashY - _transform.position.y).normalized * 40;
    }

    /* -- Attack Functions -- */

    public void LightningBlastInitiate()
    {
        _activeLightningBlast = true;
        soundEffects.BossElectricAttack();
        _lightningBlast = Instantiate(_lightningBlastPrefab, _headTransform.GetChild(4));
        Invoke("EndLightningBlast", 0.5f);
    }

    public void EndLightningBlast()
    {
        Destroy(_lightningBlast);
        _activeLightningBlast = false;
    }


    public void LightningAOEMaintain()
    {
        // determines how quickly the color shifts
        _colorTracker += 0.02f;

        _aoeScale += _maxAOESize * Time.deltaTime;

        // iterate through each of the strike zone objects; increase their size and shift their colors
        for (int i = 0; i < _AOEStrikes.Length; i++)
        {
            _AOEStrikes[i].transform.localScale = new Vector3(_aoeScale, _aoeScale, _aoeScale);
            _AOEStrikes[i].GetComponent<SpriteRenderer>().color = Color.Lerp(Color.clear, new Color(0, 0, 0, 0.6f), _colorTracker);
        }

        // when strike zones reach their max size, engage lightning animation and damage player if inside a zone
        if (_aoeScale >= _maxAOESize)
        {
            _strikeZoneActive = false;

            // briefly enable strike zone colliders as triggers
            EnableStrikeZones();
            Invoke("DestroyStrikeZones", 2);

            //TODO: Trigger lightning strike animation
        }
    }

    public void LightningAOEAttack()
    {
        // begin aoe attack
        _activeAOEAttack = true;

        //reinitialize color shift var
        _colorTracker = 0;

        soundEffects.BossElectricAttack();

        //create array of strike zone objects
        _AOEStrikes = new GameObject[_numAOEStrikes];

        _aoeScale = 0;

        GameObject temp;
        for (int i = 0; i < _AOEStrikes.Length; i++)
        {
            temp = Instantiate(_lightningAOEPrefab, new Vector3(playerTransform.position.x + Random.Range(-9, 10), playerTransform.position.y + Random.Range(-5, 6), 0), Quaternion.identity);
            _AOEStrikes[i] = temp;
            _AOEStrikes[i].GetComponent<ParticleSystem>().Stop();

        }

        //start strike zone animation which is handled in fixed update
        _strikeZoneActive = true;
    }

    void EnableStrikeZones()
    {
        for (int i = 0; i < _numAOEStrikes; i++)
        {
            _AOEStrikes[i].GetComponent<ParticleSystem>().Play();
            _AOEStrikes[i].GetComponent<CircleCollider2D>().enabled = true;
        }
    }

    void DestroyStrikeZones()
    {
        for (int i = 0; i < _numAOEStrikes; i++)
        {
            Destroy(_AOEStrikes[i]);
        }

        // end aoe attack
        _activeAOEAttack = false;
    }
}
