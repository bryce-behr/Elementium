using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyScript : MonoBehaviour
{
    // Values
    protected float maxHealth;
    protected float currentHealth;
    protected float lastAttackTime;
    protected float cooldown;

    // Components
    protected Transform _transform;
    protected SoundEffectsScript soundEffects;
    protected Transform playerTransform;

    // Public Components


    // Public Objects
    public GameObject sceneManager;
    public GameObject player;

    protected void Init(float maxHealth)
    {
        //Set values
        this.maxHealth = maxHealth;
        currentHealth = this.maxHealth;
        lastAttackTime = Time.time;
        cooldown = 3;

        //Set components
        _transform = transform;
        soundEffects = sceneManager.GetComponent<SoundEffectsScript>();
        playerTransform = player.transform;
    }

    /* Player Attack Detection */
    protected float TakeDamage(Collider2D collision)
    {
        //bool gotHit = false;

        if (collision.CompareTag(TagList.PlayerBasicAttack))
        {
            currentHealth -= DataList.attackDamage[PPrefsList.GetValue(PPrefsList.SwordType, 0), (int)DataList.AttackType.BasicAttack];
            soundEffects.SwordClash();
        }

        if (collision.CompareTag(TagList.PlayerDashAttack))
        {
            currentHealth -= DataList.attackDamage[PPrefsList.GetValue(PPrefsList.SwordType, 0), (int)DataList.AttackType.DashAttack];
            soundEffects.SwordClash();
        }

        if (collision.CompareTag(TagList.PlayerChargeAttack))
        {
            currentHealth -= DataList.attackDamage[PPrefsList.GetValue(PPrefsList.SwordType, 0), (int)DataList.AttackType.ChargeAttack];
            soundEffects.SwordClash();
        }

        if (collision.CompareTag(TagList.PlayerLightning))
        {
            currentHealth -= 5;
        }

        if (collision.CompareTag(TagList.PlayerFire))
        {
            currentHealth -= 10;
        }

        return currentHealth;
    }

}
