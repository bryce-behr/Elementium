using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{

    public GameObject _door;
    BoxCollider2D _doorCollider;
    Animator _animator;
    bool _alreadyUsedDoor;
    public string _bossType;
    public FireBossScript _fireBoss;
    public NewLightningBossScript _lightningBoss;
    public FinalBossScript _finalBoss;
    public WindBossScript _windBoss;

    public List<GameObject> _smallEnemies;
    public bool _haveSmallEnemies;
    bool _enemiesDefeated = false;

    // Start is called before the first frame update
    void Start()
    {
        _doorCollider = _door.GetComponent<BoxCollider2D>();
        _animator = _door.GetComponent<Animator>();
        if (_haveSmallEnemies)
        {
            _doorCollider.enabled = true;
            _animator.SetBool("Close Door", true);
        }
        else
        {
            _doorCollider.enabled = false;
            _animator.SetBool("Close Door", false);
            _enemiesDefeated = true;
        }
        switch (_bossType)
        {
            case "Lightning Boss":
                _alreadyUsedDoor = PPrefsList.GetBool(PPrefsList.DefeatedLightning, false);
                _lightningBoss.canFight = false;
                break;
            case "Fire Boss":
                _alreadyUsedDoor = PPrefsList.GetBool(PPrefsList.DefeatedFire, false);
                _fireBoss.canFight = false;
                break;
            case "Wind Boss":
                _alreadyUsedDoor = PPrefsList.GetBool(PPrefsList.DefeatedWind, false);
                _windBoss.canFight = false;
                break;
            case "Final Boss":
                _alreadyUsedDoor = PPrefsList.GetBool(PPrefsList.DefeatedFinal, false);
                _finalBoss.canFight = false;
                break;
            default:
                print("Please enter enemy type in the door trigger script");
                print("Valid Bosses: 'Lightning Boss', 'Fire Boss', 'Wind Boss', 'Final Boss'");
                _alreadyUsedDoor = false;
                break;
        }

        //Destroy all enemies if the door is already used
        if (_alreadyUsedDoor && _haveSmallEnemies)
        {
            for (int i = 0; i < _smallEnemies.Count; i++)
            {
                Destroy(_smallEnemies[i]);
                OpenDoor();
                _enemiesDefeated = true;
            }
        }
    }

    void Update()
    {
        if (_enemiesDefeated)
        {
            return;
        }

        bool foundEnemy = false;
        for (int i = 0; i < _smallEnemies.Count; i++)
        {
            if (_smallEnemies[i] != null)
            {
                foundEnemy = true;
                break;
            }
        }

        if (!foundEnemy)
        {
            OpenDoor();
            _enemiesDefeated = true;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (!_alreadyUsedDoor)
        {
            switch (_bossType)
            {
                case "Lightning Boss":
                    _lightningBoss.canFight = true;
                    break;
                case "Fire Boss":
                    _fireBoss.canFight = true;
                    break;
                case "Wind Boss":
                    _windBoss.canFight = true;
                    break;
                case "Final Boss":
                    _finalBoss.canFight = true;
                    break;
                default:
                    print("Please enter enemy type in the door trigger script");
                    print("Valid Bosses: 'Lightning Boss', 'Fire Boss', 'Wind Boss', 'Final Boss'");
                    break;
            }
            _doorCollider.enabled = true;
            _animator.SetBool("Close Door", true);
            _alreadyUsedDoor = true;
        }
    }

    public void OpenDoor()
    {
        _doorCollider.enabled = false;
        _animator.SetBool("Close Door", false);
    }

}
