using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Created not to collide with EnemyScript
// Though implements a lot of the functionality there and much, much more
[RequireComponent(typeof(Rigidbody2D))]
public class LesserEnemyScript : MonoBehaviour
{
    Coroutine randomRotationCoroutine;
    Rigidbody2D _rigidbody;
    Transform _transform;
    Animator _animator;

    // can't make immutable for some reason
    Vector2 dirrectionFacing = Vector2.right;

    Vector2 objectToPlayerVector;
    Quaternion playerFollowRotation;

    [Header("General Attributes")]
    public int movementSpeed = 3;

    public float maxHealth = 10;
    float currentHealth;

    float lastAttackTime;
    public float cooldown = 3;

    [Header("Idle Variables")]
    public bool detectedPlayer = false;
    public float timeToWait = 1;
    public float percentChanceToRotate = 0.1f;
    public float percentChanceToWalk = 0.3f;
    public float timeToWalk = 1;

    [Header("Attack Variables")]
    public int lengthOfPlayerDetection = 10;
    public int meleeRange = 8;


    [Header("Scene Objects")]
    public GameObject sceneManager;
    public GameObject player;
    Transform playerTransform;
    public GameObject exclaimation;
    public GameObject healthBarInner;
    public GameObject healthBarOuter;

    SoundEffectsScript soundEffects;

    void Start()
    {
        currentHealth = maxHealth;
        lastAttackTime = Time.time;

        _transform = transform;
        _animator = GetComponent<Animator>();

        exclaimation.SetActive(false);
        healthBarInner.SetActive(false);
        healthBarOuter.SetActive(false);

        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.velocity = Vector2.zero;

        soundEffects = sceneManager.GetComponent<SoundEffectsScript>();
        playerTransform = player.transform;

        randomRotationCoroutine = StartCoroutine(RotateToRandomAngleAndMoveRandomly());
    }

    void Update()
    {
        dirrectionFacing = _transform.rotation * Vector2.right;
        objectToPlayerVector = _transform.position - playerTransform.position;

        if (detectedPlayer)
        {
            playerFollowRotation.SetFromToRotation(Vector2.left, objectToPlayerVector);
            _transform.rotation = playerFollowRotation;

            if (Time.time - lastAttackTime >= cooldown && Raycast(meleeRange, 0, LayerMask.GetMask("Player")))
            {
                StopMoving();

                //attack
                _animator.SetBool("DoSlash", true);
                Invoke("ResetAnimation", 0.5f);
                lastAttackTime = Time.time;
                cooldown = 1.5f;

            }
            else
                MoveInFacingDirection();
        }
        else
        {
            if (DetectPlayer())
            {
                PlayerDetected();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        float currentHealth = TakeDamage(collision);
        if (currentHealth <= 0)
        {
            Destroy(this.gameObject);
        }
        else if (currentHealth < maxHealth && !detectedPlayer)
        {
            // ensures the enemy turns around if player is there
            PlayerDetected();
        }
        else
            StartCoroutine(ShowHealthBar(currentHealth));

    }

    // helper functions

    void PlayerDetected()
    {
        detectedPlayer = true;
        StartCoroutine(FlashExclaimation());
        StopCoroutine(randomRotationCoroutine);
    }

    IEnumerator ShowHealthBar(float currentHealth)
    {
        float coolvar = 2 * currentHealth / maxHealth;
        healthBarInner.transform.localScale = new Vector3(coolvar, 1, 1);
        //healthBar.transform.
        healthBarOuter.SetActive(true);
        healthBarInner.SetActive(true);
        yield return new WaitForSeconds(.5f);
        healthBarOuter.SetActive(false);
        healthBarInner.SetActive(false);
    }

    // detects if player is in field of view using raycasts
    bool DetectPlayer()
    {
        bool didDetectPlayer = false;
        int[] angles = { 0, 15, -15, 30, -30, 45, -45 };

        for (int i = 0; i < angles.Length; i++)
        {
            if (Raycast(lengthOfPlayerDetection, angles[i], LayerMask.GetMask("Player")))
                didDetectPlayer = true;
        }

        return didDetectPlayer;
    }

    IEnumerator FlashExclaimation()
    {
        StartCoroutine(KeepObjectAbove(exclaimation.transform));
        yield return new WaitForSeconds(0.01f);
        exclaimation.SetActive(true);
        soundEffects.EnemySeesPlayer();
        yield return new WaitForSeconds(1);
        StopCoroutine(KeepObjectAbove(exclaimation.transform));
        exclaimation.SetActive(false);
    }

    IEnumerator KeepObjectAbove(Transform objectTransform)
    {
        Transform exclaimationTransform = objectTransform.transform;
        Quaternion rotation = Quaternion.Euler(0, 0, 0);
        WaitForSeconds wait = new WaitForSeconds(0.05f);
        Vector3 offset = new Vector3(0, 3.6f, 0);
        while (true)
        {
            objectTransform.position = _transform.position + offset;
            // stops it from rotating with the object
            objectTransform.rotation = rotation;
            yield return wait;
        }
    }


    // does a raycast in dirrection the enemy is facing offset by some angle
    RaycastHit2D Raycast(int length, int angle, LayerMask layerMask)
    {
        Vector2 dirrection = Quaternion.Euler(0, 0, angle) * dirrectionFacing;
        Debug.DrawRay(_transform.position, length * dirrection, Color.green);
        return Physics2D.Raycast(_transform.position, dirrection, length, layerMask);
    }

    IEnumerator RotateToRandomAngleAndMoveRandomly()
    {
        WaitForSeconds repeatAfterSeconds = new WaitForSeconds(timeToWait);
        WaitForSeconds waitForWalk = new WaitForSeconds(timeToWalk);
        while (true)
        {
            if (Random.value <= percentChanceToRotate)
            {
                Quaternion rotationQuaternion = Quaternion.Euler(0, 0, Random.Range(0, 360));
                _transform.rotation = rotationQuaternion;
            }
            else if (Random.value <= percentChanceToWalk && !Raycast(10, 0, LayerMask.GetMask("Walls")))
            {
                MoveInFacingDirection();
                Invoke("StopMoving", timeToWalk);
                yield return waitForWalk;
            }
            yield return repeatAfterSeconds;
        }
    }

    void HealthBarShow()
    {
    }

    // moves in the direction the enemy is facing
    void MoveInFacingDirection()
    {
        _rigidbody.velocity = movementSpeed * dirrectionFacing;
    }


    void StopMoving()
    {
        _rigidbody.velocity = Vector2.zero;
    }

    void ResetAnimation()
    {
        _animator.SetBool("DoSlash", false);
    }

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
            currentHealth -= 4;
        }

        if (collision.CompareTag(TagList.PlayerFire))
        {
            currentHealth -= 7;
        }

        return currentHealth;
    }

}
