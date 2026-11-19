using UnityEngine;

public class CrabEnemy : MonoBehaviour
{
    bool isDead = false;

    Rigidbody2D rb;

    [SerializeField] float moveSpeed = 2f;
    float direction = 1f;

    public Transform wallCheck;
    public float wallCheckRadius = 0.2f;
    public LayerMask wallLayer;
    bool onWall;

    public Transform player;
    [SerializeField] float chaseRange = 5f;

    public CrabState currentState;

    [SerializeField] float bounceForce = 8f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentState = CrabState.Patrol;
    }

    void Update()
    {
       if (player == null)
{
    rb.linearVelocity = Vector2.zero;
    return;
}
        float distance = Vector2.Distance(transform.position, player.position);

        switch (currentState)
        {
            case CrabState.Patrol:
                Patrol();
                if (distance < chaseRange)
                    currentState = CrabState.Chase;
                break;

            case CrabState.Chase:
                ChasePlayer();
                if (distance >= chaseRange)
                    currentState = CrabState.Patrol;
                break;
        }
    }

    private void ChasePlayer()
    {
        float playerDir = player.position.x > transform.position.x ? 1f : -1f;
        direction = playerDir;

        rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);
    }

    private void Patrol()
    {
        rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);

        onWall = Physics2D.OverlapCircle(wallCheck.position, wallCheckRadius, wallLayer);

        if (onWall)
        {
            direction *= -1;
            Flip();
        }
    }

    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x = direction;
        transform.localScale = scale;
    }

    // =========================
    // TOP HIT (HEAD COLLIDER)
    // =========================
    void OnTriggerEnter2D(Collider2D other)
{
    if (isDead) return;

    if (!other.CompareTag("Player"))
        return;

    // TOP HIT CHECK (player must be above enemy)
    if (other.transform.position.y > transform.position.y)
    {
        isDead = true;

        Debug.Log("Enemy died from top!");

        rb.linearVelocity = Vector2.zero;

        // stop enemy completely
        currentState = CrabState.Dead;

        // destroy enemy after short delay
        Destroy(gameObject, 0.5f);
    }
}

    // =========================
    // SIDE HIT (BODY COLLISION)
    // =========================
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;

        if (!collision.gameObject.CompareTag("Player"))
            return;

        Debug.Log("Player hit from side!");

        PlayerController pc = collision.gameObject.GetComponent<PlayerController>();

        if (pc != null)
        {
            pc.DieFromSide();
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (wallCheck != null)
            Gizmos.DrawWireSphere(wallCheck.position, wallCheckRadius);
    }

    public void DieFromTop()
{
    if (isDead) return;

    isDead = true;

    Debug.Log("Enemy died from top!");

    rb.linearVelocity = Vector2.zero;

    // stop movement completely
    currentState = CrabState.Dead;

    Destroy(gameObject, 0.5f);
}
}

public enum CrabState
{
    Patrol,
    Chase,
    Dead
}