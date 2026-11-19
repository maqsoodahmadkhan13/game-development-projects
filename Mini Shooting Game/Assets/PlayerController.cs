using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float power = 1f;
    public float powerSpeed = 20f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Continuous power change
        power += powerSpeed * Time.deltaTime;

        if (power > 100f)
        {
            power = 1f;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        Vector2 mouseWorldPosition =
            Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 direction =
            (mouseWorldPosition - (Vector2)transform.position).normalized;

        rb.AddForce(direction * power, ForceMode2D.Impulse);
    }
}