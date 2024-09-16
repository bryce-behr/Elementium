using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalBossScript : BossScript
{
    //Personal Variables
    //float _maxHealth = 100;
    //float _currentHealth;
    //bool _rageMode = false;
    public bool _canMove;
    //float _lastAttackTime;
    //float _cooldown;
    float _xPos = 0;
    //float _healthBarBaseSize;

    //Dash variables
    float _dashX;
    float _dashY;
    Vector2 _dashTarget;
    Vector2 _startingPlace;
    bool _isDashing = false;
    int _maxBeams = 4;
    int _beams = 0;

    //Public Components
    //public GameObject _player;
    //public GameObject _healthBar;

    //public GameObject _rageParticles;
    public GameObject _fireballPrefab;
    //public GameObject _defeatParticles;
    public GameObject _lightningAOEPrefab;
    public GameObject _lightningBlastPrefab;
    public PauseMenuScript pauseMenuScript;
    //public Text _name;
    //public GameObject _bossDefeatManager;
    public GameObject _deathWall;

    //Private Components
    Animator _animator;
    //Transform _transform;
    //Transform _playerTransform;
    private GameObject[] _AOEStrikes;
    private GameObject _lightningBlast;
    //BossDefeatManagerScript _bdms;
    //private GameObject _UI;

    //Sound effect objects
    //public GameObject _sceneManager;
    //private SoundEffectsScript _soundEffects;

    //Attack Variables
    //public bool _canFight = true;
    private float _colorTracker;
    private bool _strikeZoneActive;
    private bool _activeLightningBlast;
    private int _numAOEStrikes;
    private int _maxAOESize;
    private bool _activeAOEAttack;

    float _aoeScale;


    void Start()
    {
        InitBoss(120, 35);

        if (PPrefsList.GetBool(PPrefsList.DefeatedFinal, false))
        {
            RemoveBossInfo();
            Destroy(gameObject);
            return;
        }

        _animator = _transform.GetChild(0).gameObject.GetComponent<Animator>();
        _canMove = true;

        //Lightning Attacks
        _colorTracker = 0;
        _strikeZoneActive = false;
        _activeAOEAttack = false;
        _activeLightningBlast = false;

        //initialize CUSTOMIZABLE vars
        _numAOEStrikes = 5;
        _maxAOESize = 3;
    }

    void Update()
    {

        if (!canFight)
            return;

        base.Update();

        if (!PPrefsList.GetBool(PPrefsList.DefeatedFinal, false))
            _UI.GetComponent<Slider>().value = GetHealthPercent();

        //Move with the player.
        if (_canMove)
        {
            _xPos = playerTransform.position.x;
            if (_xPos < -12.5f)
            {
                _xPos = -12.5f;
            }
            if (_xPos > 12.41f)
            {
                _xPos = 12.41f;
            }
            _transform.position = new Vector2(_xPos, _transform.position.y);
        }

        if (_strikeZoneActive)
        {
            LightningAOEMaintain();
        }

        // CONTROL BOSS ATTACKS
        if (Time.time - lastAttackTime >= cooldown)
        {
            if (Vector2.Distance(playerTransform.position, _transform.position) > 5)
            {
                //Barrage
                LightningAOEAttack();
                _animator.SetBool("DoBarrage", true);
                cooldown = 4;
            }
            else
            {
                //_animator.SetBool("DoBeam", true);
                //_cooldown = 2.5f;

                //Decide which melee attack to perform.
                if (_beams < _maxBeams)
                {
                    //Beam
                    _animator.SetBool("DoBeam", true);
                    cooldown = 2.5f;

                    //Track how many beams there have been since the last tounge attack.
                    _beams++;
                }
                else
                {
                    //Tounge
                    _animator.SetBool("DoTounge", true);
                    cooldown = 3.1f;

                    //Reset the beam count.
                    _beams = 0;
                    _maxBeams = Random.Range(3, 6);
                }

            }

            lastAttackTime = Time.time;
        }
        if (Time.time - lastAttackTime >= 1)
        {
            _animator.SetBool("DoTounge", false);
            _animator.SetBool("DoBeam", false);
            _animator.SetBool("DoBarrage", false);
        }
    }

    override protected void EnterRageMode()
    {
        _deathWall.SetActive(true);
    }

    override protected void BossDeath()
    {
        pauseMenuScript.canPause = false; // prevents pauses after boss death
        PPrefsList.SetBool(PPrefsList.DefeatedFinal, true);
        Destroy(_deathWall);
        DestroyStrikeZones();
        RemoveBossInfo();
    }
    public void LaunchFireball(bool randomizeTarget)
    {
        float x;
        float y;

        if (randomizeTarget)
        {
            x = Random.Range(-20f, 20f);
            y = Random.Range(-20f, 20f);
        }
        else
        {
            x = playerTransform.position.x;
            y = playerTransform.position.y;
        }

        GameObject fireball = Instantiate(_fireballPrefab, new Vector2(_xPos, _transform.position.y + 1), Quaternion.identity);
        fireball.GetComponent<FireballScript>().SetTarget(x, y);
    }

    public void FireballSound()
    {
        soundEffects.BossFireAttack();
    }

    public void SwordSound()
    {
        soundEffects.FireSlam();
    }

    public void ElecSound()
    {
        soundEffects.BossElectricAttack();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TakeDamage(collision);
    }

    /* -- Attack Functions -- */

    public void LightningBlastInitiate()
    {
        _activeLightningBlast = true;
        _lightningBlast = Instantiate(_lightningBlastPrefab, _transform);
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
