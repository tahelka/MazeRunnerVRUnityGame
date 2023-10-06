using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_TimerText;
    private float m_ElapsedTime;
    private string m_CurrentTimeValue;
    private bool isTimerRunning = false;

    public void StartTimer()
    {
        isTimerRunning = true;
        m_ElapsedTime = 0f;
    }

    public void StopTimer()
    {
        isTimerRunning = false;
        setCurrentTimerValue();
    }

    public void ResetTimer()
    {
        m_ElapsedTime = 0f;
        UpdateTimerDisplay();
    }

    void Update()
    {
        if (isTimerRunning)
        {
            m_ElapsedTime += Time.deltaTime;
            UpdateTimerDisplay();
        }
    }

    private void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(m_ElapsedTime / 60);
        int seconds = Mathf.FloorToInt(m_ElapsedTime % 60);
        m_TimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void setCurrentTimerValue()
    {
        int minutes = Mathf.FloorToInt(m_ElapsedTime / 60);
        int seconds = Mathf.FloorToInt(m_ElapsedTime % 60);
        m_CurrentTimeValue = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public string GetCurrentTimerValue()
    {
        return m_CurrentTimeValue;
    }

    public void Pause()
    {
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        Time.timeScale = 1f;
    }
}