using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int score;
    public TextMeshProUGUI scoreText;

    public float matchTime = 120f;
    public TextMeshProUGUI timeText;
    float currentTime;
    public bool gameEnded;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        currentTime = matchTime;
        timeText.text = matchTime.ToString();
    }

    void Update()
    {
        if (gameEnded) return;

        currentTime -= Time.deltaTime;
        timeText.text = currentTime.ToString();
        if (currentTime <= 0)
        {
            EndGame();
        }
    }

    public void AddScore(int amount)
    {
        score += amount;
        scoreText.text = Mathf.CeilToInt(score).ToString();


    }

    void EndGame()
    {
        gameEnded = true;
        Time.timeScale = 0f;

        UIManager.Instance.ShowEndScreen(score);
    }
}