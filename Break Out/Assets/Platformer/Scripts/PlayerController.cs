using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 12f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    private bool isGrounded;

    [Header("Attack Settings")]
    [SerializeField] private bool isAttacking;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private float lastAttackTime;

    Animator animator;
    private Rigidbody2D rb;

    bool isDead = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update() 
    {
        if (isDead) return;

        HandleMovement();
        HandleJump();
        HandleAttack();
    }

    void FixedUpdate()
    {
        if (groundCheck != null)
        {
            isGrounded = Physics2D.OverlapCircle(
                groundCheck.position, 
                groundCheckRadius, 
                groundLayer
            );
        }
    }

    void HandleAttack()
    {
        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            isAttacking = true;
            lastAttackTime = 0f;
            animator.SetTrigger("Attack");
        }

        lastAttackTime += Time.deltaTime;

        if (lastAttackTime >= attackCooldown)
        {
            isAttacking = false;
        }
    }

    bool isIdle;

    private void HandleMovement()
    {
        float horizInput = Input.GetAxisRaw("Horizontal");

        rb.linearVelocity = new Vector2(horizInput * moveSpeed, rb.linearVelocity.y);

        if (horizInput > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            isIdle = false;
        }
        else if (horizInput < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            isIdle = false;
        }
        else
        {
            isIdle = true;
        }

        animator.SetBool("IsIdle", isIdle);
    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    // ✅ FIXED COLLISION (IMPORTANT)
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Enemy") || isDead)
            return;

        // Check if player is ABOVE enemy
        if (transform.position.y > collision.transform.position.y + 0.5f)
        {
            Debug.Log("Top hit!");

            // Kill enemy
            CrabEnemy enemy = collision.gameObject.GetComponent<CrabEnemy>();
            if (enemy != null)
            {
                enemy.DieFromTop();
            }

            // ❗ STOP PLAYER AFTER HIT
            FreezePlayer();
        }
        else
        {
            Debug.Log("Side hit!");
            Die();
        }
    }

    // ✅ FREEZE PLAYER (TOP HIT)
    void FreezePlayer()
    {
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Static;

        isDead = true;

        animator.SetTrigger("Die"); // optional
    }

    // ✅ PLAYER DEATH
    void Die()
    {
        if (isDead) return;

        isDead = true;

        Debug.Log("Player died!");

        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Static;

        animator.SetTrigger("Die");
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }

    public void BounceFromEnemy(float bounceForce)
{
    rb.linearVelocity = Vector2.zero;
    rb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
}

public void DieFromSide()
{
    if (isDead) return;

    isDead = true;

    Debug.Log("PLAYER DEAD CONFIRMED");

    // stop movement
    rb.linearVelocity = Vector2.zero;

    // freeze physics
    rb.bodyType = RigidbodyType2D.Static;

    // disable ALL visuals in entire player hierarchy
    foreach (Transform t in GetComponentsInChildren<Transform>())
    {
        SpriteRenderer sr = t.GetComponent<SpriteRenderer>();
        if (sr != null)
            sr.enabled = false;

        Collider2D col = t.GetComponent<Collider2D>();
        if (col != null)
            col.enabled = false;
    }

    // stop script
    this.enabled = false;
}

}