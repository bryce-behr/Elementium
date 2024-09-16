using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectsScript : MonoBehaviour
{
    private bool fadeOut;

    private AudioSource _playerSounds;
    public AudioClip _playerElectricAttack;
    public AudioClip _healSpell;
    public AudioClip _fireAttack;
    public AudioClip _dash;
    public AudioClip _playerFailure;
    public AudioClip _playerVictory;
    public AudioClip _swordClash;

    private AudioSource _tornadoSource;
    public AudioClip _tornado;

    private AudioSource _bossAttacks;
    public AudioClip _fireSlam;
    public AudioClip _windWoosh;
    public AudioClip _harp;
    public AudioClip _windSlam;
    public AudioClip _windSlamDrop;
    public AudioClip _windLiftOff;

    private AudioSource _takeDamageSource;
    public AudioClip _takeDamage;

    public AudioClip _enemySeesPlayer;

    // Start is called before the first frame update
    void Start()
    {
        _playerSounds = gameObject.AddComponent<AudioSource>();
        _bossAttacks = gameObject.AddComponent<AudioSource>();
        _tornadoSource = gameObject.AddComponent<AudioSource>();
        _takeDamageSource = gameObject.AddComponent <AudioSource>();
    }

    private void FixedUpdate()
    {
        if(fadeOut)
        {
            _tornadoSource.volume *= .99f;
        }
        if(_tornadoSource.volume <= 0.05f)
        {
            fadeOut = false;
            _tornadoSource.Stop();
        }
    }

    public void WindLiftOff()
    {
        _bossAttacks.volume = 1f;
        _bossAttacks.PlayOneShot(_windLiftOff);
    }

    public void WindSlamDrop()
    {
        _bossAttacks.volume = 1f;
        _bossAttacks.PlayOneShot(_windSlamDrop);
    }

    public void WindSlam()
    {
        _bossAttacks.volume = 1f;
        _bossAttacks.PlayOneShot(_windSlam);
    }

    public void HarpAttack()
    {
        _bossAttacks.volume = 1f;
        _bossAttacks.PlayOneShot(_harp);
    }

    public void TornadoStart()
    {
        _tornadoSource.volume = .25f;
        _tornadoSource.PlayOneShot(_tornado);
    }

    public void TornadoStop()
    {
        fadeOut = true;
    }

    public void WindBoomerang()
    {
        _bossAttacks.volume = 1f;
        _bossAttacks.PlayOneShot(_windWoosh);
    }

    public void SwordClash()
    {
        _playerSounds.volume = .15f;
        _playerSounds.PlayOneShot(_swordClash);
    }

    public void FireAttack()
    {
        _tornadoSource.volume = .4f;
        _tornadoSource.PlayOneShot(_fireAttack);
    }

    public void BossFireAttack()
    {
        _bossAttacks.volume = .4f;
        _bossAttacks.PlayOneShot(_fireAttack);
    }

    public void PlayerElectricAttack()
    {
        _playerSounds.volume = .175f;
        _playerSounds.PlayOneShot(_playerElectricAttack);
    }

    public void BossElectricAttack()
    {
        _bossAttacks.volume = .175f;
        _bossAttacks.PlayOneShot(_playerElectricAttack);
    }

    public void HealSpell()
    {
        _playerSounds.volume = 1f;
        _playerSounds.PlayOneShot(_healSpell);
    }

    public void Dash()
    {
        _playerSounds.volume = .5f;
        _playerSounds.PlayOneShot(_dash);
    }

    public void TakeDamage()
    {
        _takeDamageSource.volume = 1f;
        _takeDamageSource.PlayOneShot(_takeDamage);
    }

    public void FireSlam()
    {
        _bossAttacks.volume = .1f;
        _bossAttacks.PlayOneShot(_fireSlam);  
    }

    public void PlayerFailure()
    {
        _playerSounds.volume = .5f;
        _playerSounds.PlayOneShot(_playerFailure);
    }

    public void PlayerVictory()
    {
        if(!PPrefsList.GetBool(PPrefsList.DefeatedFinal, true))
        {
            _playerSounds.volume = .35f;
            _playerSounds.PlayOneShot(_playerVictory);
        }
        GetComponent<MusicScript>().FadeOutBossMusic();
    }

    public void EnemySeesPlayer()
    {
        _bossAttacks.volume = 1;
        _bossAttacks.PlayOneShot(_enemySeesPlayer);
    }
}
