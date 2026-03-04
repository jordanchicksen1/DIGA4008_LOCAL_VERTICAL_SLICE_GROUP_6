using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private MusicManager musicManager;

    [Header("UI Stuff")]
    public int score;
    public TextMeshProUGUI scoreText;

    public float matchTime = 120f;
    public TextMeshProUGUI timeText;
    float currentTime;
    public bool gameEnded;

    [Header("Lights")]
    public Light DirectLight;

    [Header("Pitch work")]
    public bool raisePitch;
    public float pitchIncrease;

    void Awake()
    {
        Instance = this;
        musicManager = FindAnyObjectByType<MusicManager>();
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
        timeText.text = Mathf.CeilToInt(currentTime).ToString();
        if (currentTime <= 0)
        {
            EndGame();
        }

        BackgroundMusicPitch();
    }

    public void AddScore(int amount)
    {
        score += amount;
        scoreText.text = Mathf.CeilToInt(score).ToString();
        musicManager.SFX.PlayOneShot(musicManager.Collect);
        DirectLight.intensity -= 0.2f;

    }

    void EndGame()
    {
        gameEnded = true;
        Time.timeScale = 0f;

        UIManager.Instance.ShowEndScreen(score);
    }

    public void BackgroundMusicPitch() 
    {
        //Debug.Log(currentTime);
        if (currentTime <= 35) 
        {
            //Debug.Log("pitch up");
            raisePitch = true;

        }

        if(currentTime <= 34) 
        {
            raisePitch = false;
        }


        if (raisePitch) 
        {
            musicManager.BackgroundMusic.pitch += pitchIncrease * Time.deltaTime;
        }
    }
}