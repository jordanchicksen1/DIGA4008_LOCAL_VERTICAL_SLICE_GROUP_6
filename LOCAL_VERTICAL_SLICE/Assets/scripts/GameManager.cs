using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private MusicManager musicManager;
  

    [Header("UI Stuff")]
    public int score;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI countdownText;

    [Header("Timing")]
    public float matchTime = 120f;
    public TextMeshProUGUI timeText;
    float currentTime;
    public bool gameEnded;
    public float startCountdown = 3f;
    public bool gameStarted = false;

    [Header("Countdown Animation")]
    public float countdownPopScale = 1.6f;
    public float countdownPopSpeed = 8f;

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
        StartCoroutine(StartCountdown());
    }

    IEnumerator StartCountdown()
    {
        gameStarted = false;

        int count = 3;

        while (count > 0)
        {
            countdownText.text = count.ToString();

            // Pop animation
            StartCoroutine(PopCountdown());

            yield return new WaitForSeconds(1f);
            count--;
        }

        countdownText.text = "GO!";
        StartCoroutine(PopCountdown());

        yield return new WaitForSeconds(1f);

        countdownText.gameObject.SetActive(false);

        gameStarted = true;
    }

    IEnumerator PopCountdown()
    {
        countdownText.transform.localScale = Vector3.one * countdownPopScale;

        float t = 0;

        while (t < 1)
        {
            
            countdownText.transform.localScale = Vector3.Lerp(
                countdownText.transform.localScale,
                Vector3.one,
                countdownPopSpeed * Time.deltaTime
            );

            t += Time.deltaTime;
            yield return null;
        }

        countdownText.transform.localScale = Vector3.one;
    }

    void Update()
    {
        if (gameEnded) return;

        // don't start the round timer until countdown finishes
        if (!gameStarted) return;

        currentTime -= Time.deltaTime;
        timeText.text = Mathf.CeilToInt(currentTime).ToString();

        if (currentTime <= 0)
        {
            EndGame();
        }

        if (vignette.intensity.value > 0.4f)
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