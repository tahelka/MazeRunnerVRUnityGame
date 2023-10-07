using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;

public class LeaderboardManager : MonoBehaviour
{
    private ScoreData m_ScoresData;
    public float m_PlayerScore;
    public GameObject m_LeaderBoardContent;
    [SerializeField] private Timer m_Timer;

    void Awake()
    {
        m_ScoresData = new ScoreData();

       /* // get the highscore value from the data.
        if (m_ScoresData != null && m_ScoresData.m_Scores.Count != 0)
        {
            m_HighScore = m_ScoresData.m_Scores.First().m_ElapsedTime;
        }*/
    }

    private void Start()
    {
        // Subscribe the SetupPlayerScore method to the OnGameSetup event in the GameManager instance.
        //GameManager.Instance.OnGameSetup += SetupPlayerScore;
        resetPlayerScore();
    }

    public void SetPlayerScore()
    {
        m_PlayerScore = m_Timer.GetCurrentElapsedTime();
    }

    private void resetPlayerScore()
    {
        m_PlayerScore = 0;
    }

    // This method returns a sorted leaderboard - the lowest m_ElapsedTime values, will be at the top of the leaderboard.
    public IEnumerable<Score> SortedHighScoreLeaderBoard()
    {
        return m_ScoresData.m_Scores.OrderBy(x => x.m_ElapsedTime);
    }


    // this method adds a Score to leaderBoard
    public void AddScoreToLeaderBoard(Score score)
    {
        m_ScoresData.m_Scores.Add(score);
        this.GetComponent<LeaderboardScore>().PresentSortedLeaderBoard();
    }

    // this method resets score array of the LeaderBoard
    public void ResetScoreLeaderBoard()
    {
        m_ScoresData.m_Scores?.Clear();
        Debug.Log("Score Data was cleared");
    }
}


[Serializable]
public class Score
{
    public string m_Name;
    public float m_ElapsedTime;

    public Score(string name, float elapsedTime)
    {
        this.m_Name = name;
        this.m_ElapsedTime = elapsedTime;
    }
}

[Serializable]
public class ScoreData
{
    public List<Score> m_Scores;

    public ScoreData()
    {
        m_Scores = new List<Score>();
    }
}
