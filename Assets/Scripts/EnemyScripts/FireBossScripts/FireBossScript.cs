using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireBossScript : BossScript
{
    //Personal Variables
    //float _healthBarBaseSize;
    public bool _canRotate;

    //Combat variables
    int _maxSlams = 4;
    int _slams = 0; //_slams decrements every time a slam is done, which increases the chance of a spin.

    //Public Components
    public GameObject _fireBossHead;
    public Sprite _brokenHeadSprite;
    public GameObject _fireballPrefab;
    public GameObject _volcanoHand;
    public GameObject _firepitSpinner;

    //Private Components
    Animator _animator;
    SpriteRenderer _fireBossHeadSR;
    Transform _volcanoHandTransform;

    void Start()
    {
        InitBoss(60, 20);

        if (PPrefsList.GetBool(PPrefsList.DefeatedFire, false))
        {
            RemoveBossInfo();
            Destroy(gameObject);
            return;
        }

        _animator = _transform.GetChild(0).gameObject.GetComponent<Animator>();
        _fireBossHeadSR = _fireBossHead.GetComponent<SpriteRenderer>();
        _volcanoHandTransform = _volcanoHand.transform;
        
        _canRotate = true;
    }

    protected override void Update()
    {
        if (!canFight)
            return;

        base.Update();

        if (!PPrefsList.GetBool(PPrefsList.DefeatedFire, false))
            _UI.GetComponent<Slider>().value = GetHealthPercent();

        //Rotate boss to face the player.
        if (_canRotate)
        {
            float angle = Mathf.Atan2(playerTransform.position.y - _transform.position.y, playerTransform.position.x - _transform.position.x) * Mathf.Rad2Deg;
            angle += 90;
            _transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }

        // CONTROL BOSS ATTACKS
        if (Time.time - lastAttackTime >= cooldown)
        {
            if (Vector2.Distance(playerTransform.position, _transform.position) > 10)
            {
                //Decide which ranged attack to perform.
                if (Random.Range(0, 3) == 0)
                {
                    _animator.SetBool("DoSpew", true);
                    cooldown = 4;
                }
                else
                {
                    _animator.SetBool("DoFlamethrower", true);
                    cooldown = 4;
                }
            }
            else
            {
                //Decide which melee attack to perform.
                if (_slams < _maxSlams)
                {
                    //Sword Slam
                    _animator.SetBool("DoSlam", true);
                    cooldown = 2.1f;

                    //Track how many slams there have been since the last spin.
                    _slams++;
                }
                else
                {
                    //Spin
                    _animator.SetBool("DoSpin", true);
                    cooldown = 3;

                    //Reset the slam count.
                    _slams = 0;
                    _maxSlams = Random.Range(3, 6);
                }
            }

            lastAttackTime = Time.time;
        }
        if (Time.time - lastAttackTime >= 1)
        {
            _animator.SetBool("DoSlam", false);
            _animator.SetBool("DoFlamethrower", false);
            _animator.SetBool("DoSpin", false);
            _animator.SetBool("DoSpew", false);
        }

    }

    override protected void EnterRageMode()
    {
        _fireBossHeadSR.sprite = _brokenHeadSprite;
        _firepitSpinner.SetActive(true);
    }

    override protected void BossDeath()
    {
        PPrefsList.SetBool(PPrefsList.DefeatedFire, true);
        /*PlayerPrefs.SetInt(PPrefsList.MaxHealth, PPrefsList.GetValue(PPrefsList.MaxHealth, 10) + 5);
        PlayerPrefs.SetInt(PPrefsList.MaxMagic, PPrefsList.GetValue(PPrefsList.MaxMagic, 10) + 5);*/
        PlayerPrefs.SetInt(PPrefsList.SwordType, 1);
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

        GameObject fireball = Instantiate(_fireballPrefab, _volcanoHandTransform.position, Quaternion.identity);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TakeDamage(collision);
    }
}
