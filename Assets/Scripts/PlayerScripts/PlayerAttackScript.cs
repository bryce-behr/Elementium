using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* This file has methods that are called by the character's Animator.
 * It drives the character's actions.
 */

public class PlayerAttackScript : MonoBehaviour
{
    //Components
    PlayerScript _mainPlayerScript;
    GameObject _sword;
    BoxCollider2D _swordCollider;
    Animator _animator;

    string[] _swordTags = { TagList.PlayerBasicAttack, TagList.PlayerChargeAttack, TagList.PlayerDashAttack };

    void Start()
    {
        Transform _transform = transform;
        Transform _playerTransform = _transform.parent;

        _mainPlayerScript = _playerTransform.gameObject.GetComponent<PlayerScript>();
        _animator = GetComponent<Animator>();

        _sword = _transform.GetChild(2).GetChild(0).gameObject;
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

    public void StopDash()
    {
        _mainPlayerScript._isDashing = false;
    }

    public void EnableChargeAttack()
    {
        _mainPlayerScript._isCharged = true;
    }

    public void HealSpell()
    {
        _mainPlayerScript.HealPlayer(4);
    }

    //Decommit the player from their actions. Return to idle.
    public void DecommitPlayer()
    {
        _animator.SetBool("DoBasicAttack", false);
        _animator.SetBool("DoDash", false);
        _animator.SetBool("DoDashAttack", false);
        _animator.SetBool("Charge", false);
        _animator.SetBool("ChargeAttack", false);
        _animator.SetBool("DoHeal", false);
        _animator.SetBool("DoFire", false);
        _animator.SetBool("DoLightning", false);
        _mainPlayerScript._isCommitted = false;
        _mainPlayerScript._isDashing = false;
        _mainPlayerScript._isCharged = false;
    }


}
