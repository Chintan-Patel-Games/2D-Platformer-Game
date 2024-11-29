using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameOverController gameOverController;
    [SerializeField] UIController uiController;
    [SerializeField] ParticleSystem bulletParticle;

    [Tooltip("Movement Speed")]
    [SerializeField] float speed;

    [Tooltip("Jump Force")]
    [SerializeField] float jumpForce;

    [Tooltip("Staff Damage")]
    [SerializeField] int staffDmg = 10;

    [Tooltip("Reference to the AudioSource")]
    [SerializeField] AudioSource audioSource;

    [Tooltip("Array of jump sounds")]
    [SerializeField] AudioClip jumpClip;

    [Tooltip("Array of land sounds")]
    [SerializeField] AudioClip landClip;

    [Tooltip("Array of footstep sounds")]
    [SerializeField] AudioClip[] footstepClips;

    [Tooltip("Array of hurt sounds")]
    [SerializeField] AudioClip[] hurtClips;

    [Tooltip("Array of melee sounds")]
    [SerializeField] AudioClip[] meleeClips;

    [Tooltip("Array of bullet sounds")]
    [SerializeField] AudioClip[] bulletClips;

    private Animator playerAnimator;
    private Rigidbody2D rb2d;
    private CapsuleCollider2D capsuleCollider2D;
    private BoxCollider2D boxCollider2D;
    private bool isFacingRight = true;
    private bool isDead = false;
    private int lives = 3;
    public int Lives { get { return lives; } }
    private int keys = 0;
    public int Keys { get { return keys; } }

    private void Awake()
    {
        playerAnimator = gameObject.GetComponent<Animator>();
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        capsuleCollider2D = gameObject.GetComponent<CapsuleCollider2D>();
        boxCollider2D = gameObject.GetComponent<BoxCollider2D>();
        playerAnimator.SetBool("isFalling", true);
        playerAnimator.SetBool("isGrounded", false);
    }

    public void Update()
    {
        float moveSpeed = Input.GetAxisRaw("Horizontal");
        bool crouch = Input.GetKey(KeyCode.LeftControl);
        bool jump = Input.GetKeyDown(KeyCode.Space);
        bool shoot = Input.GetMouseButtonDown(0);
        bool melee = Input.GetKeyDown(KeyCode.F);
        // bool interact = Input.GetKeyDown(KeyCode.E);
        bool weaponSwitch = Input.GetKeyDown(KeyCode.Q);

        MoveCharacter(moveSpeed, jump);
        MoveAnimation(moveSpeed, crouch, jump);
        WeaponShoot(shoot);
        WeaponsAnimation(melee, weaponSwitch);
    }

    private void MoveCharacter(float moveSpeed, bool jump)
    {
        MovePos(moveSpeed);
        JumpPos(jump);
    }

    // Move Run Player position
    private void MovePos(float moveSpeed)
    {
        if (playerAnimator.GetBool("isCrouching") == false && !isDead)
        {
            Vector3 movePos = transform.position;
            movePos.x += moveSpeed * speed * Time.deltaTime;
            transform.position = movePos;
        }
    }

    private void WeaponShoot(bool shoot)
    {
        if (shoot && playerAnimator.GetBool("isWithGun") == true)
        {
            bulletParticle.Emit(1);

            if (bulletClips.Length > 0)
            {
                // Randomize melee sound for variety
                AudioClip clip = bulletClips[Random.Range(0, bulletClips.Length)];
                audioSource.PlayOneShot(clip);
            }

            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, 10f);
            if (hit.collider != null)
            {
                EnemyController enemy = hit.collider.GetComponent<EnemyController>();
                if (enemy != null)
                {
                    enemy.TakeDamage(1);
                }
            }
        }
    }

    // Animations for Weapons Controls
    private void WeaponsAnimation(bool melee, bool weaponSwitch)
    {
        if (melee)
        {
            playerAnimator.SetTrigger("staffAttack");

            if (meleeClips.Length > 0)
            {
                // Randomize melee sound for variety
                AudioClip clip = meleeClips[Random.Range(0, meleeClips.Length)];
                audioSource.PlayOneShot(clip);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            playerAnimator.SetBool("isWithGun", false);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            playerAnimator.SetBool("isWithGun", true);
        }
        else if (weaponSwitch)
        {
            // Toggle between weapons
            bool isWithGun = playerAnimator.GetBool("isWithGun");
            isWithGun = !isWithGun;
            playerAnimator.SetBool("isWithGun", isWithGun);
        }
    }

    // Jump Player position
    private void JumpPos(bool jump)
    {
        if (jump && playerAnimator.GetBool("isGrounded") == true)
        {
            audioSource.PlayOneShot(jumpClip);
            rb2d.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        }
    }

    private void MoveAnimation(float moveSpeed, bool crouch, bool jump)
    {
        RunAnim(moveSpeed);
        CrouchAnim(crouch);
        JumpAnim(jump);
    }

    // Moving the Player animation
    private void RunAnim(float moveSpeed)
    {
        playerAnimator.SetFloat("Speed", Mathf.Abs(moveSpeed));

        // Update flip state only if the direction changes
        if (moveSpeed < 0 && isFacingRight)
        {
            FlipPlayer(false); // Flip to face left
        }
        else if (moveSpeed > 0 && !isFacingRight)
        {
            FlipPlayer(true); // Flip to face right
        }
    }

    private void FlipPlayer(bool facingRight)
    {
        isFacingRight = facingRight;
        transform.localRotation = Quaternion.Euler(0f, facingRight ? 0f : 180f, 0f);
    }


    // Crouch animation
    private void CrouchAnim(bool crouch)
    {
        if (crouch && playerAnimator.GetBool("isWithGun") == false)
            Crouch(true);
        else if (crouch && playerAnimator.GetBool("isWithGun") == true)
        {
            Crouch(true);
            bulletParticle.transform.localPosition = new Vector2(0f, 0.8f);
        }
        else
        {
            Crouch(false);
            bulletParticle.transform.localPosition = new Vector2(0f, 1.54f);
        }
    }

    public void Crouch(bool crouch)
    {
        playerAnimator.SetBool("isCrouching", crouch);
    }

    // Jump animation
    private void JumpAnim(bool jump)
    {
        if (jump && playerAnimator.GetBool("isWithGun") == false && playerAnimator.GetBool("isCrouching") == false)
        {
            playerAnimator.SetTrigger("Jump");
        }
        else if (jump && playerAnimator.GetBool("isWithGun") == true && playerAnimator.GetBool("isCrouching") == false)
        {
            playerAnimator.SetTrigger("JumpWithGun");
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.otherCollider == capsuleCollider2D && other.collider.CompareTag("Platform") && playerAnimator.speed == 0)
        {
            audioSource.PlayOneShot(landClip);
            playerAnimator.speed = 1;
            playerAnimator.SetBool("isGrounded", true);
            playerAnimator.SetBool("isFalling", false);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.otherCollider == capsuleCollider2D && other.collider.CompareTag("Platform"))
        {
            playerAnimator.SetBool("isGrounded", false);
        }

        if (other.otherCollider == capsuleCollider2D && other.collider.CompareTag("Platform") && rb2d.velocity.y < 0.25)
        {
            playerAnimator.SetBool("isGrounded", false);
            Falling();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.otherCollider == boxCollider2D && other.gameObject.GetComponent<EnemyController>())
        {
            other.gameObject.GetComponent<EnemyController>().TakeDamage(staffDmg);
        }
    }

    // This method is called via animation events
    public void PlayFootstep()
    {
        if (footstepClips.Length > 0)
        {
            // Randomize footstep sound for variety
            AudioClip clip = footstepClips[Random.Range(0, footstepClips.Length)];
            audioSource.PlayOneShot(clip);
        }
    }

    // This method is called via animation events
    public void PlayerHurt()
    {
        if (hurtClips.Length > 0)
        {
            // Randomize footstep sound for variety
            AudioClip clip = hurtClips[Random.Range(0, hurtClips.Length)];
            audioSource.PlayOneShot(clip);
        }
        transform.position = transform.localPosition;
    }

    // This method is called via animation events
    private void Falling()
    {
        playerAnimator.speed = 0;
        playerAnimator.SetBool("isFalling", true);
    }

    public void TakeLives()
    {
        lives--;
        playerAnimator.SetTrigger("takeDamage");
        uiController.UpdateLives();

        if (lives <= 0)
            PlayerDeath();
    }

    public void PlayerDeath()
    {
        isDead = true;
        playerAnimator.SetBool("isDead", true);
        this.enabled = false;
        StartCoroutine(PlayerDiedUI());
    }

    private IEnumerator PlayerDiedUI()
    {
        yield return new WaitForSeconds(0.5f);
        gameOverController.PlayerDied();
    }

    public void PickKey()
    {
        if (keys < 3)
            keys++;
        else
            return;
        uiController.UpdateKeys();
    }
}