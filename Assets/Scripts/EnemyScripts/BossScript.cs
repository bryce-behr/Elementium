using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BossScript : EnemyScript
{
    // Values
    protected float rageHealthThreshold;
    protected bool rageMode = false;

    // Public Values
    public bool canFight = false;

    // Public Components
    public BossDefeatManagerScript bossDefeatManager;
    public GameObject rageParticles;
    public GameObject defeatParticles;

    // UI Elements
    public Text _name;
    public GameObject _healthBar;
    protected GameObject _UI;
    public Sprite levelUpDetails;


    protected void InitBoss(float maxHealth, float rageHealthThreshold)
    {
        //Call the Init function for general enemies
        Init(maxHealth);

        //Instantiate healthBar
        _UI = Instantiate(_healthBar);
        _UI.transform.SetParent(GameObject.Find("Canvas").transform, false);

        //Set values
        this.rageHealthThreshold = rageHealthThreshold;
    }

    protected virtual void Update()
    {
        if (!canFight)
            return;

        //if (!PPrefsList.GetBool(PPrefsList.DefeatedFire, false))
            _UI.GetComponent<Slider>().value = GetHealthPercent();

        //Activate rage mode at a certain point.
        if (currentHealth <= rageHealthThreshold && !rageMode)
        {
            rageMode = true;
            rageParticles.SetActive(true);
            EnterRageMode();
        }

        //Destroy the boss if their health gets too low.
        if (currentHealth <= 0)
        {
            BossDeath();
            soundEffects.PlayerVictory();
            bossDefeatManager.BossDefeated();
            player.GetComponent<PlayerScript>()._invincible = true;
            Instantiate(defeatParticles, _transform.position, Quaternion.identity);

            if (levelUpDetails != null)
            {
                player.GetComponent<PlayerScript>().RefreshPlayerStats(levelUpDetails);
            }
            else
            {
                player.GetComponent<PlayerScript>().RefreshPlayerStats(null);
            }

            Destroy(gameObject);
        }
    }

    protected void RemoveBossInfo()
    {
        _name.color = Color.clear;
        Destroy(_UI);
    }

    protected float GetHealthPercent()
    {
        return currentHealth / maxHealth;
    }

    protected virtual void EnterRageMode() { }

    protected virtual void BossDeath() { }

}
