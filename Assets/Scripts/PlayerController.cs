using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public ScoreController scoreController;
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private Image live1;
    [SerializeField] private Image live2;
    [SerializeField] private Image live3;
    [SerializeField] private GameObject deathUIPanel;
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
        if (!isCrouching)
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

    private void FlipPlayer(bool facingRight)
    {
        isFacingRight = facingRight;
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
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

    public void PlayerDeath()
    {
        isDead = true;
        Time.timeScale = 0;
        mainCamera.transform.parent = null;
        deathUIPanel.gameObject.SetActive(true);
        rd2d.constraints = RigidbodyConstraints2D.FreezePosition;
        ReloadLevel();
    }

    private void ReloadLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void PickKey()
    {
        Debug.Log("Player Picked up the key");
        scoreController.IncreaseScore(10);
    }
}