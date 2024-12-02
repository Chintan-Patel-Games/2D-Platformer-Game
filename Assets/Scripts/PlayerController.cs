using System;
using System.Collections;
using System.Linq;
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

    private Animator playerAnimator;
    private Rigidbody2D rb2d;
    private CapsuleCollider2D capsuleCollider2D;
    private BoxCollider2D boxCollider2D;
    private Sounds jumpClip; // Array of jump sounds
    private Sounds landClip; // Array of land sounds
    private Sounds[] footstepClips; // Array of footstep sounds
    private Sounds[] hurtClips; // Array of hurt sounds
    private Sounds[] meleeClips; // Array of melee sounds
    private Sounds[] bulletClips; // Array of bullet sounds
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

            bulletClips = Enum.GetValues(typeof(Sounds))
                            .Cast<Sounds>()
                            .Where(sound => sound.ToString().StartsWith("playerRanged"))
                            .ToArray();

            if (bulletClips.Length > 0)
            {
                // Randomize bullet shot sound for variety
                Sounds clip = bulletClips[UnityEngine.Random.Range(0, bulletClips.Length)];
                SoundManager.Instance.Play(clip);
            }

            // RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, 10f);
            // if (hit.collider != null)
            // {
            //     EnemyController enemy = hit.collider.GetComponent<EnemyController>();
            //     if (enemy != null)
            //     {
            //         enemy.TakeDamage(1);
            //     }
            // }
        }
    }

    // Animations for Weapons Controls
    private void WeaponsAnimation(bool melee, bool weaponSwitch)
    {
        if (melee)
        {
            playerAnimator.SetTrigger("staffAttack");

            meleeClips = Enum.GetValues(typeof(Sounds))
                            .Cast<Sounds>()
                            .Where(sound => sound.ToString().StartsWith("playerMelee"))
                            .ToArray();

            if (meleeClips.Length > 0)
            {
                // Randomize melee sound for variety
                Sounds clip = meleeClips[UnityEngine.Random.Range(0, meleeClips.Length)];
                SoundManager.Instance.Play(clip);
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
            jumpClip = Sounds.playerjump;
            SoundManager.Instance.Play(jumpClip);
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
            landClip = Sounds.playerLand;
            SoundManager.Instance.Play(landClip);
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
        footstepClips = Enum.GetValues(typeof(Sounds))
                            .Cast<Sounds>()
                            .Where(sound => sound.ToString().StartsWith("playerFoots"))
                            .ToArray();

        if (footstepClips.Length > 0)
        {
            // Randomize footstep sound for variety
            Sounds clip = footstepClips[UnityEngine.Random.Range(0, footstepClips.Length)];
            SoundManager.Instance.Play(clip);
        }
    }

    // This method is called via animation events
    public void PlayerHurt()
    {
        hurtClips = Enum.GetValues(typeof(Sounds))
                            .Cast<Sounds>()
                            .Where(sound => sound.ToString().StartsWith("playerHurt"))
                            .ToArray();
                            
        if (hurtClips.Length > 0)
        {
            // Randomize footstep sound for variety
            Sounds clip = hurtClips[UnityEngine.Random.Range(0, hurtClips.Length)];
            SoundManager.Instance.Play(clip);
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