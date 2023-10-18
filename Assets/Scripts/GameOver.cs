using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_GameOverText;
    [SerializeField] private TextMeshProUGUI m_TimerText;
    [SerializeField] private Timer m_Timer;

    public void DisplayGameOverMenu(bool isPlayerWon)
    {
        if (isPlayerWon)
        {
            displayWonMessage();
        }
        else
        {
            displayLostMessage();
        }

        displayTimer();
    }

    private void displayWonMessage()
    {
        if(m_GameOverText != null)
        {
            m_GameOverText.text = "GOOD JOB!";
        }
        else
        {
            Debug.Log("gameover text is null");
        }
    } 
    
    private void displayLostMessage()
    {
        m_GameOverText.text = "Better luck next time!";
    }

    private void displayTimer()
    {
        m_TimerText.text = m_Timer.GetCurrentTimerValue();
    }
}
