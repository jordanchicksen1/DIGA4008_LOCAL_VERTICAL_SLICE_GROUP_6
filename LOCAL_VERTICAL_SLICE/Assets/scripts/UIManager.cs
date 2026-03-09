using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject endPanel;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI ratingText;

    [Header("Buttons")]
    public Button retryButton;
    public Button quitButton;

    void Awake()
    {
        Instance = this;
    }

    public void ShowEndScreen(int score)
    {
        endPanel.SetActive(true);

        scoreText.text = "Score: " + score;

        ratingText.text = GetRating(score);

        StartCoroutine(SelectRetryButton());

        SwitchToUIControls();
    }

    IEnumerator SelectRetryButton()
    {
        yield return new WaitForEndOfFrame();

        EventSystem.current.SetSelectedGameObject(null);
        retryButton.Select();
    }

    void SwitchToUIControls()
    {
        PlayerInput[] players = FindObjectsByType<PlayerInput>(FindObjectsSortMode.None);

        foreach (PlayerInput player in players)
        {
            player.SwitchCurrentActionMap("UI");
        }
    }

    string GetRating(int score)
    {
        //if your score is above a specific amount, give a specific rating
        if (score >= 200) return "AMAZING!";
        if (score >= 150) return "GREAT!";
        if (score >= 100) return "Good!";
        return "Better luck next time!";
    }

    public void Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}