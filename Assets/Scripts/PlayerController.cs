using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    private Animator playerAnimator;
    private Rigidbody2D rd2d;
    private bool isGrounded = false;

    private void Awake()
    {
        playerAnimator = gameObject.GetComponent<Animator>();
        rd2d = gameObject.GetComponent<Rigidbody2D>();
    }

    public void Update()
    {
        float moveSpeed = Input.GetAxisRaw("Horizontal");
        bool jump = Input.GetKeyDown(KeyCode.Space);
        bool crouch = Input.GetKey(KeyCode.LeftControl);

        MoveCharacter(moveSpeed, jump);
        MoveAnimation(moveSpeed, jump, crouch);
    }

    private void MoveCharacter(float moveSpeed, bool jump)
    {
        // Move Player position
        Vector3 movePos = transform.position;
        movePos.x += moveSpeed * speed * Time.deltaTime;
        transform.position = movePos;

        // Jump Player position
        if (jump && !isGrounded)
        {
            rd2d.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Force);
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.transform.tag == "platform") { isGrounded = true; }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.transform.tag == "platform") { isGrounded = false; }
    }

    private void MoveAnimation(float moveSpeed, bool jump, bool crouch)
    {
        // Moving the Player animation
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

        // Jump animation
        if (jump)
            playerAnimator.SetTrigger("Jump");

        // Crouch animation
        if (crouch)
            Crouch(true);
        else
            Crouch(false);
    }

    public void Crouch(bool crouch)
    {
        playerAnimator.SetBool("Crouch", crouch);
    }
}