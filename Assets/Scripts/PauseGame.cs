using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PauseGame : MonoBehaviour
{
    private bool isPaused = false;
    [SerializeField] private TextMeshProUGUI m_PauseButtonText;

    public void PauseButtonPressed()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            Pause();
        }
    }

    void Pause()
    {
        Time.timeScale = 0;
        isPaused = true;
        m_PauseButtonText.text = "Resume";
    }

    void ResumeGame()
    {
        Time.timeScale = 1;
        isPaused = false;
        m_PauseButtonText.text = "Pause";
    }
}
