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
    private Animator playerAnimator;
    private Rigidbody2D rd2d;
    private bool isGrounded = true;
    private bool isCrouching = false;
    private int lives = 3;

    private void Awake()
    {
        playerAnimator = gameObject.GetComponent<Animator>();
        rd2d = gameObject.GetComponent<Rigidbody2D>();
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
        if (jump && isGrounded)
        {
            rd2d.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Force);
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

        // Flipping the player animation
        Vector3 scale = transform.localScale;
        if (moveSpeed < 0)
        {
            scale.x = -1f * Mathf.Abs(scale.x);
        }
        else if (moveSpeed > 0)
        {
            scale.x = Mathf.Abs(scale.x);
        }
        transform.localScale = scale;
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
        if (jump) { playerAnimator.SetTrigger("Jump"); }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.collider.CompareTag("Platform") && playerAnimator.speed == 0)
        {
            playerAnimator.speed = 1;
            isGrounded = true;
            playerAnimator.SetBool("isGrounded", true);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.collider.CompareTag("Platform"))
        {
            isGrounded = false;
            playerAnimator.SetBool("isGrounded", false);
        }
    }

    public void TakeLives()
    {
        lives--;
        UpdateLives();

        if (lives <=0)
            KillPlayer();
    }

    private void UpdateLives()
    {
        if (lives == 2)
        {
            Debug.Log("Player has 2 lives");
            live3.enabled = false;
        }
        else if (lives == 1)
        {
            Debug.Log("Plaer has 1 lives");
            live2.enabled = false;
        }
        else
        {
            live1.enabled = false;
            KillPlayer();
        }
    }

    public void KillPlayer()
    {
        Debug.Log("Player died");
        ReloadLevel();
    }

    private void ReloadLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void Falling()
    {
        playerAnimator.speed = 0;
        isGrounded = false;
    }

    public void PickKey()
    {
        Debug.Log("Player Picked up the key");
        scoreController.IncreaseScore(10);
    }
}