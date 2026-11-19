// using UnityEngine;

// public class GameManager : MonoBehaviour
// {
//     public static GameManager Instance;

//     public int score = 0;

//     public TextMeshProUGUI finalScoreText;

//     public void addScore(int value)
//     {
//         score += value;
//         ScoreText.text = "Score: " + score;
//     }
//     public void GameOver()
//     {
//         GameOverPanel.setActive(true);
//         finalScoreText.text = "Final Score: " + score;
//     }
//     void Awake()
//     {
//         Instance = this;
//     }

//     public void AddScore(int value)
//     {
//         score += value;
//         Debug.Log("Score: " + score);
//     }
// }