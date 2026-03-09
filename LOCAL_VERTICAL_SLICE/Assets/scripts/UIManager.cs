using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject endPanel;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI ratingText;

    void Awake()
    {
        Instance = this;
    }

    public void ShowEndScreen(int score)
    {
        //update players on how their round went
        endPanel.SetActive(true);
        scoreText.text = "Score: " + score;

        //give a rating based on how many points they got in the round
        ratingText.text = GetRating(score);
    }

    string GetRating(int score)
    {
        //if your score is above a specific amount, give a specific rating
        if (score >= 200) return "AMAZING!";
        if (score >= 150) return "GREAT!";
        if (score >= 100) return "Good!";
        return "Better luck next time!";
    }
}