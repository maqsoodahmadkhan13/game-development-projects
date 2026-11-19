using UnityEngine;

public class GoalZone : MonoBehaviour
{
    public int score = 0;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Game Over!");
            Time.timeScale = 0f;
        }
        else if (other.CompareTag("Square"))
        {
            Destroy(other.gameObject);
            score++;
            Debug.Log("Score: " + score);
        }
    }
}