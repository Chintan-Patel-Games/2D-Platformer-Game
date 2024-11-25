using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] ScoreController scoreController;
    [SerializeField] GameCompleteController gameCompleteController;
    [SerializeField] GameOverController gameOverController;
    [SerializeField] float speed;
    [SerializeField] float jumpForce;
    [SerializeField] Image live1;
    [SerializeField] Image live2;
    [SerializeField] Image live3;
    private Animator playerAnimator;
    private Camera mainCamera;
    private Rigidbody2D rd2d;
    private SpriteRenderer spriteRenderer;
    private bool isFalling = true;
    private bool isGrounded = false;
    private bool isCrouching = false;
    private bool isFacingRight = true;
    private bool isDead = false;
    private int lives = 3;
    [SerializeField] private AudioSource audioSource; // Reference to the AudioSource
    [SerializeField] private AudioClip[] footstepClips; // Array of footstep sounds

    private void Awake()
    {
        playerAnimator = gameObject.GetComponent<Animator>();
        mainCamera = Camera.main;
        rd2d = gameObject.GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerAnimator.SetBool("isFalling", isFalling);
        playerAnimator.SetBool("isGrounded", isGrounded);
    }

    public void Update()
    {
        float moveSpeed = Input.GetAxisRaw("Horizontal");
        bool crouch = Input.GetKey(KeyCode.LeftControl);
        bool jump = Input.GetKeyDown(KeyCode.Space);

        MoveCharacter(moveSpeed, jump);
        MoveAnimation(moveSpeed, crouch, jump);
    }

    private void MoveCharacter(float moveSpeed, bool jump)
    {
        MovePos(moveSpeed);
        JumpPos(jump);
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
            SoundManager.Instance.Play(Sounds.playerJump);
            rd2d.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
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
            SoundManager.Instance.Play(Sounds.playerLand);
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

        if (other.collider.CompareTag("Platform") && rd2d.velocity.y < 0.25)
        {
            playerAnimator.SetBool("isGrounded", false);
            Falling();
        }
    }

    private void Falling()
    {
        playerAnimator.speed = 0;
        playerAnimator.SetBool("isFalling", true);
    }

    public void TakeLives()
    {
        lives--;
        UpdateLives();

        if (lives <= 0)
            PlayerDeath();
    }

    private void UpdateLives()
    {
        if (lives == 2)
        {
            live3.enabled = false;
        }
        else if (lives == 1)
        {
            live2.enabled = false;
        }
        else if (lives == 0)
        {
            live1.enabled = false;
        }
        else
        {
            live3.enabled = false;
            live2.enabled = false;
            live1.enabled = false;
        }
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
        gameOverController.PlayerDied();
        this.enabled = false;
    }

    public void PickKey()
    {
        scoreController.IncreaseScore(10);
    }
}