using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LeaderboardScore : MonoBehaviour
{
    [SerializeField] private LeaderboardRow m_Row;
    private const int k_MaximumPlayersOnLeaderBoard = 3;
    private LeaderboardManager m_LeaderboardManager;
    private GameObject m_LeaderboardContent;

    void Awake()
    {
        if(m_Row == null)
        {
            Debug.Log("m_Row is null");
        }
    }

    void Start()
    {
        getMembersComponents();
        PresentSortedLeaderBoard();
    }

    // this method gets components of members
    private void getMembersComponents()
    {
        m_LeaderboardManager = GameObject.Find("Leaderboard Manager").GetComponent<LeaderboardManager> ();
        if (m_LeaderboardManager == null)
        {
            Debug.Log("m_LeaderboardManager is null");
        }
        m_LeaderboardContent = m_LeaderboardManager.m_LeaderBoardContent;
        if (m_LeaderboardContent == null)
        {
            Debug.Log("m_LeaderboardContent is null");
        }
    }

    // this method sort highScore leaderBoard and present it in a table format
    public void PresentSortedLeaderBoard()
    {
        removeContentRows();
        var scores = m_LeaderboardManager.SortedHighScoreLeaderBoard().ToArray();
        addContentRows(scores);

        Debug.Log("Presented the leaderboard");
    }

    // this method adds content object rows to leaderboard
    private void addContentRows(Score[] scores)
    {
        for (int i = 0; i < scores.Length && i < k_MaximumPlayersOnLeaderBoard; i++)
        {
            LeaderboardRow row = Instantiate(m_Row, m_LeaderboardContent.transform).GetComponent<LeaderboardRow>();
            row.Rank.text = (i + 1).ToString();
            row.Name.text = scores[i].m_Name;

            int minutes = Mathf.FloorToInt(scores[i].m_ElapsedTime / 60);
            int seconds = Mathf.FloorToInt(scores[i].m_ElapsedTime % 60);
            row.Time.text = string.Format("{0:00}:{1:00}", minutes, seconds);

            Debug.Log($"Added row:{row.Rank.text},{row.Name.text},{row.Time.text}");
        }
    }

    // this method resets the leaderboard.
    public void ResetLeaderboard()
    {
        GameObject.Find("Leaderboard Manager").GetComponent< LeaderboardManager > ()?.ResetScoreLeaderBoard();
        removeContentRows();
    }

    // Clear the content object by destroying all its child objects
    private void removeContentRows()
    {
        foreach (Transform child in m_LeaderboardContent.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
