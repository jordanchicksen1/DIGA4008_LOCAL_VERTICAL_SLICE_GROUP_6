using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TitleScreenManager : MonoBehaviour
{
    [Header("Screens")]
    public GameObject[] screens;

    [Header("Buttons to highlight")]
    public Button[] defaultButtons;

    public float autoScreenTime = 6f;

    int currentScreen = 0;

    void Start()
    {
        Time.timeScale = 1f;

        ShowScreen(0);

        StartCoroutine(ShowTitleAfterDelay());
    }

    IEnumerator ShowTitleAfterDelay()
    {
        yield return new WaitForSeconds(6f);

        ShowScreen(1);
    }
    void ShowScreen(int index)
    {
        Debug.Log("Selecting button for screen: " + index);
        // Turn all screens off
        for (int i = 0; i < screens.Length; i++)
        {
            screens[i].SetActive(false);
        }

        // Turn the correct screen on
        screens[index].SetActive(true);

        currentScreen = index;

        // Only try to select a button if one exists for this screen
        if (index < defaultButtons.Length && defaultButtons[index] != null)
        {
            StartCoroutine(SelectButton(defaultButtons[index]));
        }
    }

    IEnumerator SelectButton(Button button)
    {
        yield return new WaitForEndOfFrame(); // wait until UI fully updates

        EventSystem.current.SetSelectedGameObject(null);

        button.Select();
    }

    public void StartTutorial()
    {
        StartCoroutine(TutorialSequence());
    }

    IEnumerator TutorialSequence()
    {
        for (int i = 1; i < screens.Length; i++)
        {
            ShowScreen(i);
            yield return new WaitForSeconds(autoScreenTime);
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Lvls");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}