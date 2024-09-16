using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindBossScript : BossScript
{
    Animator _animator;
    public Transform _pivotTransform;
    public Transform _boomerangPivot;

    public Sprite _rageFace;
    public Sprite _rageArm;
    public SpriteRenderer _faceSR;
    public SpriteRenderer _armSR;

    public GameObject _bigTornadoPrefab;
    GameObject _bigTornado;

    float _rotationValue;

    int _spearCount = 0;
    int _spearsNeeded = 3;

    // Start is called before the first frame update
    void Start()
    {
        InitBoss(70, 30);

        if (PPrefsList.GetBool(PPrefsList.DefeatedWind, false))
        {
            RemoveBossInfo();
            Destroy(gameObject);
            return;
        }

        _animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (!canFight)
            return;

        base.Update();

        if (!PPrefsList.GetBool(PPrefsList.DefeatedWind, false))
            _UI.GetComponent<Slider>().value = GetHealthPercent();

        //Boss Movement
        _rotationValue += Time.deltaTime * 10;
        _rotationValue = _rotationValue % 360;
        _pivotTransform.rotation = Quaternion.Euler(new Vector3(0, 0, _rotationValue));
        _transform.localRotation = Quaternion.Euler(new Vector3(0, 0, _rotationValue * -1));

        //Do Attacking
        if (Time.time - lastAttackTime >= cooldown)
        {
            if (Vector2.Distance(playerTransform.position, _transform.position) > 8)
            {
                Invoke("AimBoomerang", 2f/3f);
                _animator.SetBool("DoSlash", true);
                cooldown = 3f;
            }
            else
            {
                if (_spearCount >= _spearsNeeded)
                {
                    _animator.SetBool("DoSlam", true);
                    cooldown = 4f;
                    _spearCount = 0;
                    _spearsNeeded = Random.Range(3, 5);
                }
                else
                {
                    HarpAttackSound();
                    AimBoomerang();
                    _animator.SetBool("DoSpear", true);
                    cooldown = 2;
                    _spearCount++;
                }
            }

            lastAttackTime = Time.time;
        }
        if (Time.time - lastAttackTime >= 1)
        {
            _animator.SetBool("DoSlam", false);
            _animator.SetBool("DoSlash", false);
            _animator.SetBool("DoSpear", false);
        }
    }

    void AimBoomerang()
    {
        float angle = Mathf.Atan2(playerTransform.position.y - _transform.position.y, playerTransform.position.x - _transform.position.x) * Mathf.Rad2Deg;
        _boomerangPivot.localRotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    override protected void EnterRageMode()
    {
        _faceSR.sprite = _rageFace;
        _armSR.sprite = _rageArm;

        _bigTornado = Instantiate(_bigTornadoPrefab, _transform.position, Quaternion.identity);
        _bigTornado.GetComponent<WindBigTornadoScript>()._playerTransform = playerTransform;
        TornadoSound();
    }

    override protected void BossDeath()
    {
        soundEffects.TornadoStop();
        //Destroy(_innerBarTransform.parent.gameObject);
        PPrefsList.SetBool(PPrefsList.DefeatedWind, true);
        Destroy(_bigTornado);
        RemoveBossInfo();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TakeDamage(collision);
    }

    public void BoomerangSound()
    {
        soundEffects.WindBoomerang();
    }

    public void TornadoSound()
    {
        soundEffects.TornadoStart();
    }

    public void HarpAttackSound()
    {
        soundEffects.HarpAttack();
    }

    public void WindSlamSound()
    {
        soundEffects.WindSlam();
    }

    public void WindSlamDropSound()
    {
        soundEffects.WindSlamDrop();
    }

    public void WindLiftOffSound()
    {
        soundEffects.WindLiftOff();
    }
}
