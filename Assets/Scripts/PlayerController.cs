using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Animator animator;

    // Update is called once per frame
    void Update()
    {
        PlayerMoveControls();
        PlayerCrouchControl();
    }

    private void PlayerMoveControls()
    {
        float speed = Input.GetAxisRaw("Horizontal");
        animator.SetFloat("Speed", Mathf.Abs(speed));

        Vector3 scale = transform.localScale;

        if (speed < 0)
        {
            scale.x = -1f * Mathf.Abs(scale.x);
        }
        else if (speed > 0)
        {
            scale.x = Mathf.Abs(scale.x);
        }
        transform.localScale = scale;
    }

    private void PlayerCrouchControl()
    {
        if (Input.GetKey(KeyCode.C))
        {
            animator.SetBool("Crouch", true);
        }
        else
        {
            animator.SetBool("Crouch", false);
        }
    }
}