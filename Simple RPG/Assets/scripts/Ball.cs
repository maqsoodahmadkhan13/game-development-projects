using UnityEngine;

public class Ball : MonoBehaviour
{
    public float startSpeed = 5f;
    public float speedIncreaseRate = 0.1f;

    private Rigidbody2D rb;
    private bool hasStarted = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!hasStarted && Input.GetKeyDown(KeyCode.Space))
        {
            StartBall();
        }
    }

    void StartBall()
    {
        hasStarted = true;

        float randomX = Random.Range(-1f, 1f);
        Vector2 direction = new Vector2(randomX, -1f).normalized;

        rb.linearVelocity = direction * startSpeed;
    }

    void FixedUpdate()
    {
        if (hasStarted)
        {
            rb.linearVelocity *= (1 + speedIncreaseRate * Time.fixedDeltaTime);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Paddle"))
        {
            float hitPoint = transform.position.x - collision.transform.position.x;
            rb.linearVelocity = new Vector2(hitPoint * 5f, Mathf.Abs(rb.linearVelocity.y));
        }
    }
}