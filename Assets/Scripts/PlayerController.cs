using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameCompleteController gameCompleteController;
    [SerializeField] GameOverController gameOverController;
    [SerializeField] UIController uiController;
    [SerializeField] float speed;
    [SerializeField] float jumpForce;
    private Animator playerAnimator;
    private Camera mainCamera;
    private Rigidbody2D rb2d;
    private SpriteRenderer spriteRenderer;
    private bool isWithGun = false;
    private bool isWalking = false;
    private bool isFalling = true;
    private bool isGrounded = false;
    private bool isCrouching = false;
    private bool isFacingRight = true;
    private bool isDead = false;
    public int lives = 3;
    [SerializeField] private AudioSource audioSource; // Reference to the AudioSource
    [SerializeField] private AudioClip jumpClip; // Array of jump sounds
    [SerializeField] private AudioClip landClip; // Array of land sounds
    [SerializeField] private AudioClip[] footstepClips; // Array of footstep sounds
    [SerializeField] private AudioClip[] hurtClips; // Array of hurt sounds

    private void Awake()
    {
        playerAnimator = gameObject.GetComponent<Animator>();
        mainCamera = Camera.main;
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerAnimator.SetBool("isFalling", isFalling);
        playerAnimator.SetBool("isGrounded", isGrounded);
    }

    public void Update()
    {
        float moveSpeed = Input.GetAxisRaw("Horizontal");
        bool crouch = Input.GetKey(KeyCode.LeftControl);
        bool jump = Input.GetKeyDown(KeyCode.Space);
        bool walk = Input.GetKeyDown(KeyCode.LeftShift);

        MoveCharacter(moveSpeed, jump, walk);
        MoveAnimation(moveSpeed, crouch, jump, walk);
    }

    private void MoveCharacter(float moveSpeed, bool jump, bool walk)
    {
        MovePos(moveSpeed);
        JumpPos(jump);
        WalkPos(moveSpeed, walk);
    }

    // Move Player position
    private void MovePos(float moveSpeed)
    {
        if (!isCrouching && !isDead)
        {
            Vector3 movePos = transform.position;
            movePos.x += moveSpeed * speed * Time.deltaTime;
            transform.position = movePos;
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

    // Move Player position
    private void WalkPos(float moveSpeed, bool walk)
    {
        if (!isCrouching && !isDead)
        {
            Vector3 movePos = transform.position;
            movePos.x += moveSpeed * speed * Time.deltaTime;
            transform.position = movePos;
        }
    }

    private void MoveAnimation(float moveSpeed, bool crouch, bool jump, bool walk)
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
        spriteRenderer.flipX = !facingRight;
    }


    // Crouch animation
    private void CrouchAnim(bool crouch)
    {
        if (crouch)
            Crouch(true);
        else
            Crouch(false);
    }

    public void Crouch(bool crouch)
    {
        playerAnimator.SetBool("isCrouching", crouch);
    }

    // Jump animation
    private void JumpAnim(bool jump)
    {
        if (jump)
        {
            playerAnimator.SetTrigger("Jump");
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.collider.CompareTag("Platform") && playerAnimator.speed == 0)
        {
            audioSource.PlayOneShot(landClip);
            playerAnimator.speed = 1;
            playerAnimator.SetBool("isGrounded", true);
            playerAnimator.SetBool("isFalling", false);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.collider.CompareTag("Platform"))
        {
            playerAnimator.SetBool("isGrounded", false);
        }

        if (other.collider.CompareTag("Platform") && rb2d.velocity.y < 0.25)
        {
            playerAnimator.SetBool("isGrounded", false);
            Falling();
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

    public void LevelComplete()
    {
        mainCamera.transform.parent = null;
        gameCompleteController.GameComplete();
        this.enabled = false;
    }

    public void PlayerDeath()
    {
        isDead = true;
        playerAnimator.SetBool("isDead", true);
        mainCamera.transform.parent = null;
        StartCoroutine(PlayerDiedUI());
        this.enabled = false;
    }

    private IEnumerator PlayerDiedUI()
    {
        yield return new WaitForSeconds(0.5f); 
        gameOverController.PlayerDied();
    }

    public void PickKey()
    {
        Debug.Log("Player picked up the key");
    }
}