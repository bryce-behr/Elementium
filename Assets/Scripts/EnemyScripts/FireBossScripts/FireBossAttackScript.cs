using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBossAttackScript : MonoBehaviour
{
    FireBossScript _mainBossScript;
    public GameObject _sword;
    BoxCollider2D _swordCollider;
    Animator _animator;

    string[] _swordTags = { TagList.FireBossSlam, TagList.FireBossSpin };

    void Start()
    {
        Transform _transform = transform;
        Transform _bossTransform = _transform.parent;

        _mainBossScript = _bossTransform.gameObject.GetComponent<FireBossScript>();
        _animator = GetComponent<Animator>();

        _swordCollider = _sword.GetComponent<BoxCollider2D>();
        _swordCollider.enabled = false;
    }

    public void ActivateSwordCollision(int tagToUse)
    {
        _swordCollider.enabled = true;
        _sword.tag = _swordTags[tagToUse];
    }

    public void DeactivateSwordCollision()
    {
        _swordCollider.enabled = false;
    }

    public void StopRotate()
    {
        _mainBossScript._canRotate = false;
    }

    public void AllowRotate()
    {
        _mainBossScript._canRotate = true;
    }

    public void LaunchTargetedFireball()
    {
        _mainBossScript.LaunchFireball(false);
    }

    public void LaunchRandomFireball()
    {
        _mainBossScript.LaunchFireball(true);
    }

    public void FireballSound()
    {
        _mainBossScript.FireballSound();
    }

    public void SlamSound()
    {
        _mainBossScript.SwordSound();
    }
}
