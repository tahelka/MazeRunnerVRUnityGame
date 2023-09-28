using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class MazeManager : MonoBehaviour
{
    [SerializeField] private MazeGenerator m_MazeGenerator;
    [SerializeField] private Transform m_Player;
    [SerializeField] private GameObject m_StarterRoom;
    private List<GameLevel> m_GameLevels;
    private MazeNode m_StartNode;
    private MazeNode m_EndNode;
    public GameLevel currentGameLevel { get; private set; }

    public void Awake()
    {
        m_GameLevels = new List<GameLevel>
        {
            new("Easy", 5, 5),
            new("Medium", 7, 7),
            new("Hard", 10, 10)
        };
    }

    public void SetGameLevel(string i_Name)
    {
        foreach (GameLevel gameLevel in m_GameLevels)
        {
            if (i_Name == gameLevel.Name)
            {
                currentGameLevel = gameLevel;
                mazePreparation();
                GameManager.Instance.StartGame();
                return;
            }
        }

        Debug.Log("No matching game level found for name: " + i_Name);
    }

    // Not in use right now
    public void SetCustomGameLevel(string i_Name, int i_Rows, int i_Cols)
    {
        bool isProperLevel = true;
        bool isNewLevel = true;

        foreach (GameLevel gameLevel in m_GameLevels)
        {
            if (i_Name == gameLevel.Name)
            {
                if (i_Rows != gameLevel.Rows || i_Cols != gameLevel.Cols)
                {
                    Debug.Log("Entered wrong level");
                    isProperLevel = false;
                    break;
                }

                isNewLevel = false;
            }
        }

        if (isProperLevel)
        {
            if (isNewLevel)
            {
                m_GameLevels.Add(new GameLevel(i_Name, i_Rows, i_Cols));
            }

            foreach (GameLevel gameLevel in m_GameLevels)
            {
                if (i_Name == gameLevel.Name)
                {
                    currentGameLevel = gameLevel;
                }
            }

            mazePreparation();
            GameManager.Instance.StartGame();
        }
    }

    private void mazePreparation()
    {
        m_MazeGenerator.GenerateMazeInstant(currentGameLevel.Rows, currentGameLevel.Cols); // should also include obstacles and enemies
        m_StartNode = m_MazeGenerator.StartNode;
        m_Player.position = m_StartNode.transform.position;
    }

    public void EndTriggerEntered()
    {
        m_Player.position = m_StarterRoom.transform.position;
    }
}
