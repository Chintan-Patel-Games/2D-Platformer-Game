using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour, IEnemy
{
    [SerializeField] protected int health;
    [SerializeField] private float moveSpeed;
    [SerializeField] private GameObject pointA;
    [SerializeField] private GameObject pointB;
    protected Animator enemyAnimator;
    protected bool canAttack = true;
    protected bool isInRange = false;
    private Transform currentPoint;

    // IEnemy Properties
    public int Health { get; set; }
    public float MoveSpeed { get; set; }
    public GameObject PointA { get; }
    public GameObject PointB { get; }
    public Sounds[] FootstepClips { get; set; }
    public Sounds[] AttackClips { get; set; }
    public bool CanAttack => canAttack;
    public bool IsInRange => isInRange;

    private void Start()
    {
        enemyAnimator = GetComponent<Animator>();
        currentPoint = pointB.transform;
    }

    private void Update()
    {
        EnemyPatrol();
    }

    public void EnemyPatrol()
    {
        MoveEnemy();
        EndPointController();
    }

    public void MoveEnemy()
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

    public void EndPointController()
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

    public void FlipEnemy()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        PlayerController playerController = other.gameObject.GetComponent<PlayerController>();
        CircleCollider2D circleCollider = GetComponent<CircleCollider2D>();
        if (playerController != null && other is CapsuleCollider2D)
        {
            if (other.IsTouching(circleCollider))
            {
                if (canAttack)
                    StartCoroutine(Attack(playerController));
            }
        }
    }

    // Trigger detection methods for playing sounds
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other is CapsuleCollider2D)
        {
            isInRange = true;
        }
    }

    // Trigger detection methods for stopping sounds
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other is CapsuleCollider2D)
        {
            isInRange = false;
        }
    }

    public virtual IEnumerator Attack(PlayerController player)
    {
        yield return new WaitForSeconds(3f);
    }

    public IEnumerator TakePlayerLives(PlayerController player)
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

    public virtual IEnumerator HandleDeath()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
    
    public virtual void PlayAttackSounds() { }
    public virtual void PlayFootstepSounds() { }
}