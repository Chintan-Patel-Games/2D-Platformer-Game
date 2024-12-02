using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Tooltip("Enemy Health")]
    [SerializeField] int health = 5;

    [Tooltip("Enemy Move Speed")]
    [SerializeField] float moveSpeed;

    [Tooltip("Patrol Endpoint 1")]
    [SerializeField] GameObject pointA;

    [Tooltip("Patrol Endpoint 2")]
    [SerializeField] GameObject pointB;
    private Sounds[] footstepClips; // Array of footsteps sounds
    private Sounds[] attackClips; // Array of attack sounds
    private Animator enemyAnimator;
    private Transform currentPoint;
    private bool canAttack = true;

    private void Start()
    {
        enemyAnimator = GetComponent<Animator>();
        currentPoint = pointB.transform;
    }

    private void Update()
    {
        EnemyPatrol();
    }

    private void EnemyPatrol()
    {
        MoveEnemy();
        EndPointController();
    }

    private void MoveEnemy()
    {
        if (currentPoint.transform == pointB.transform)
        {
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
        }
    }

    private void EndPointController()
    {
        // Check Enmey reached at EndPoint 2
        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointB.transform)
        {
            FlipEnemy();
            currentPoint = pointA.transform;
        }

        // Check Enmey reached at EndPoint 1
        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointA.transform)
        {
            FlipEnemy();
            currentPoint = pointB.transform;
        }
    }

    private void FlipEnemy()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayerController>() && other as CapsuleCollider2D)
        {
            if (canAttack)
                StartCoroutine(Attack(other.gameObject.GetComponent<PlayerController>()));
        }
    }

    private IEnumerator Attack(PlayerController player)
    {
        canAttack = false;
        enemyAnimator.SetTrigger("attack");
        StartCoroutine(TakePlayerLives(player));

        // Wait for cooldown period
        yield return new WaitForSeconds(3f);
        canAttack = true;
    }

    private IEnumerator TakePlayerLives(PlayerController player)
    {
        yield return new WaitForSeconds(0.25f);
        if (player != null)
        {
            player.TakeLives();
        }
    }

    public void TakeDamage(int damage)
    {
        health = health - damage;
        if (health < 0)
        {
            StartCoroutine(HandleDeath());
        }
    }

    private IEnumerator HandleDeath()
    {
        SoundManager.Instance.Play(Sounds.chompDie);
        enemyAnimator.SetTrigger("isDead");
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    // This method is called via animation events
    public void PlayFootstepSounds()
    {
        footstepClips = Enum.GetValues(typeof(Sounds))
                        .Cast<Sounds>()
                        .Where(sound => sound.ToString().StartsWith("chompFoots"))
                        .ToArray();
                    
        if (footstepClips.Length > 0)
        {
            // Randomize footstep sounds
            Sounds clip = footstepClips[UnityEngine.Random.Range(0, footstepClips.Length)];
            SoundManager.Instance.Play(clip);
        }
    }

    // This method is called via animation events
    public void PlayAttackSounds()
    {
        attackClips = Enum.GetValues(typeof(Sounds))
                    .Cast<Sounds>()
                    .Where(sound => sound.ToString().StartsWith("chompAttack"))
                    .ToArray();

        if (attackClips.Length > 0)
        {
            // Randomize attack sounds
            Sounds clip = attackClips[UnityEngine.Random.Range(0, attackClips.Length)];
            SoundManager.Instance.Play(clip);
        }
    }
}