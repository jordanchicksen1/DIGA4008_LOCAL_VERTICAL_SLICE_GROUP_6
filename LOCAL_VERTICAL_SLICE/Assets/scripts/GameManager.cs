using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

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
    public bool darker;
    public Volume volume;
    Vignette vignette;

    [Header("Pitch work")]
    public bool raisePitch;
    public float pitchIncrease;

    void Awake()
    {
        Instance = this;
        musicManager = FindAnyObjectByType<MusicManager>();
        volume.profile.TryGet<Vignette>(out vignette);
        vignette.intensity.overrideState = true;
        vignette.color.overrideState = true;
        darker = true;
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

        if(vignette.intensity.value > 0.4f) 
        {
            darker = false;
        }

        BackgroundMusicPitch();
    }

    public void AddScore(int amount)
    {
        score += amount;
        scoreText.text = Mathf.CeilToInt(score).ToString();
        musicManager.SFX.PlayOneShot(musicManager.Collect);
        DirectLight.intensity -= 0.2f;

        if(DirectLight.intensity <= 0 && darker) 
        {
            Debug.Log("no lights");
            vignette.intensity.value += 0.1f;
        }

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