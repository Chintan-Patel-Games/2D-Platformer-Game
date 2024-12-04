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
    private Transform currentPoint;

    // IEnemy Properties
    public int Health { get; set; }
    public float MoveSpeed { get; set; }
    public GameObject PointA { get; set; }
    public GameObject PointB { get; set; }
    public Sounds[] FootstepClips { get; set; }
    public Sounds[] AttackClips { get; set; }
    public bool CanAttack => canAttack;

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
        if (other.gameObject.GetComponent<PlayerController>() && other as CapsuleCollider2D)
        {
            if (canAttack)
                StartCoroutine(Attack(other.gameObject.GetComponent<PlayerController>()));
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
        // enemyAnimator.SetTrigger("isDead");
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
    public virtual void PlayAttackSounds() { }
    public virtual void PlayFootstepSounds() { }
}