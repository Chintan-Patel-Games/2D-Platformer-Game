using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] GameObject pointA;
    [SerializeField] GameObject pointB;

    [Tooltip("Reference to the AudioSource")]
    [SerializeField] AudioSource audioSource;

    [Tooltip("Array of attack sounds")]
    [SerializeField] AudioClip[] attackClips;

    [Tooltip("Array of footsteps sounds")]
    [SerializeField] AudioClip[] footstepClips;
    private Animator enemyAnimator;
    private Transform currentPoint;
    private int health = 5;
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
        if (currentPoint.transform == pointB.transform)
        {
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
        }

        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointB.transform)
        {
            Flip();
            currentPoint = pointA.transform;
        }

        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointA.transform)
        {
            Flip();
            currentPoint = pointB.transform;
        }
    }

    private void Flip()
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
        yield return new WaitForSeconds(0.15f);
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
        SoundManager.Instance.Play(Sounds.enemyDied);
        enemyAnimator.SetTrigger("isDead");
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    // This method is called via animation events
    public void PlayFootstepSounds()
    {
        if (footstepClips.Length > 0)
        {
            // Randomize footstep sound for variety
            AudioClip clip = footstepClips[Random.Range(0, footstepClips.Length)];
            audioSource.PlayOneShot(clip);
        }
    }

    public void PlayAttackSounds()
    {
        if (attackClips.Length > 0)
        {
            // Randomize footstep sound for variety
            AudioClip clip = attackClips[Random.Range(0, attackClips.Length)];
            audioSource.PlayOneShot(clip);
        }
    }
}