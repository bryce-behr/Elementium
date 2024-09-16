using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(SoundEffectsScript))]
public class HeadScript : MonoBehaviour
{
    NewLightningBossScript _mainBossScript;
    public GameObject _sceneManager;
    private SoundEffectsScript _soundEffectsScript;
//    private SoundEffectsScript _soundScript;

    void Start()
    {
        _mainBossScript = transform.parent.gameObject.GetComponent<NewLightningBossScript>();
        _soundEffectsScript = _sceneManager.GetComponent<SoundEffectsScript>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _mainBossScript.TakeDamage(collision);

        //bool gotHit = false;

        /*if (collision.CompareTag(TagList.PlayerBasicAttack))
        {
            _mainBossScript.TakeDamage(DataList.attackDamage[PPrefsList.GetValue(PPrefsList.SwordType, 0), (int)DataList.AttackType.BasicAttack]);
            //gotHit = true;
            _soundEffectsScript.SwordClash();
        }

        if (collision.CompareTag(TagList.PlayerDashAttack))
        {
            _mainBossScript.TakeDamage(DataList.attackDamage[PPrefsList.GetValue(PPrefsList.SwordType, 0), (int)DataList.AttackType.DashAttack]);
            //gotHit = true;
            _soundEffectsScript.SwordClash();
        }

        if (collision.CompareTag(TagList.PlayerChargeAttack))
        {
            _mainBossScript.TakeDamage(DataList.attackDamage[PPrefsList.GetValue(PPrefsList.SwordType, 0), (int)DataList.AttackType.ChargeAttack]);
            //gotHit = true;
            _soundEffectsScript.SwordClash();
        }

        if (collision.CompareTag(TagList.PlayerLightning))
        {
            _mainBossScript.TakeDamage(4);
        }

        if (collision.CompareTag(TagList.PlayerFire))
        {
            _mainBossScript.TakeDamage(0.2f);
        }*/
    }

    public void StopRotate()
    {
        _mainBossScript.StopRotate();
    }

    public void AllowRotate()
    {
        _mainBossScript.AllowRotate();
    }

    public void FireBeam()
    {
        _mainBossScript.LightningBlastInitiate();
    }

}
