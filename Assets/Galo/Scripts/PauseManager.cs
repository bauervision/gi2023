using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static PauseManager instance;
    public GameObject PausePanel;
    bool isPaused;

    private void Awake() { instance = this; }

    public void ToggleGamePause()
    {
        isPaused = !isPaused;
        PausePanel.SetActive(isPaused);
        Time.timeScale = isPaused ? 0 : 1;
    }

    public void ToggleInGamePause()
    {
        isPaused = !isPaused;
        PausePanel.SetActive(isPaused);
        Time.timeScale = isPaused ? 0 : 1;
        // get the current level task
        PausePanel.transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "...your current objective is...";
        PausePanel.transform.GetChild(0).transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "GAME PAUSED";
    }


    public void ToggleGamePauseComplete(string finalText)
    {
        isPaused = !isPaused;
        PausePanel.SetActive(isPaused);
        Time.timeScale = isPaused ? 0 : 1;
        //set the text
        PausePanel.transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = finalText;
        PausePanel.transform.GetChild(0).transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "LEVEL COMPLETE!";
    }

}
