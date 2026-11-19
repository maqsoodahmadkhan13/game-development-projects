using UnityEngine;

public class PlayerMovements : MonoBehaviour
{
    Rigidbody2D rb;
    public float movementSpeed = 8f;
    float movementDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        movementDirection = Input.GetAxisRaw("Horizontal");
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(movementDirection * movementSpeed, 0f);
    }
}