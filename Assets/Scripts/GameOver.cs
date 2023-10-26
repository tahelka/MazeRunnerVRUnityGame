using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum eGameOver
{
    Win,
    Lose,
    Quit
}

public class GameOver : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_GameOverText;
    [SerializeField] private TextMeshProUGUI m_TimerText;
    [SerializeField] private Timer m_Timer;

    public void DisplayGameOverMenu(eGameOver i_GameOverReason)
    {
        if (i_GameOverReason == eGameOver.Win)
        {
            displayWonMessage();
        }
        else if (i_GameOverReason == eGameOver.Lose)
        {
            displayLostMessage();
        }
        else if (i_GameOverReason == eGameOver.Quit)
        {
            displayQuitMessage();
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

    private void displayQuitMessage()
    {
        m_GameOverText.text = "Maybe try an easier level";
    }

    private void displayTimer()
    {
        m_TimerText.text = m_Timer.GetCurrentTimerValue();
    }
}
