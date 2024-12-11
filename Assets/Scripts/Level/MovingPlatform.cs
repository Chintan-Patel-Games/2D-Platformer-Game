using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private Vector3 pointA; // Start position (Point A)
    [SerializeField] private Vector3 pointB; // End position (Point B)
    [SerializeField] private float speed = 2f; // Movement speed
    [SerializeField] private float waitTime = 1f; // Time to wait at each position

    private bool movingToB = true; // Track direction of movement

    private void Start()
    {
        // Initialize platform at Point A
        transform.position = pointA;
        StartCoroutine(MovePlatform());
    }

    private System.Collections.IEnumerator MovePlatform()
    {
        while (true)
        {
            // Determine the target position
            Vector3 targetPosition = movingToB ? pointB : pointA;

            // Move the platform to the target position
            while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
                yield return null;
            }

            // Snap to the exact position
            transform.position = targetPosition;

            // Switch direction and wait
            movingToB = !movingToB;
            yield return new WaitForSeconds(waitTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Parent the player to the platform so they move with it
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Detach the player from the platform when they leave
            collision.transform.SetParent(null);
        }
    }
}