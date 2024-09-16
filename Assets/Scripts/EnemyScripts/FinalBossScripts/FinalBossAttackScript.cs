using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBossAttackScript : MonoBehaviour
{
    FinalBossScript _mainBossScript;

    void Start()
    {
        Transform _transform = transform;
        Transform _bossTransform = _transform.parent;

        _mainBossScript = _bossTransform.gameObject.GetComponent<FinalBossScript>();
    }

    public void StopMove()
    {
        _mainBossScript._canMove = false;
    }

    public void AllowMove()
    {
        _mainBossScript._canMove = true;
    }

    public void LaunchTargetedFireball()
    {
        _mainBossScript.LaunchFireball(false);
    }

    public void FireballSound()
    {
        _mainBossScript.FireballSound();
    }

    public void SlamSound()
    {
        _mainBossScript.SwordSound();
    }

    public void ElecSound()
    {
        _mainBossScript.ElecSound();
    }

}
