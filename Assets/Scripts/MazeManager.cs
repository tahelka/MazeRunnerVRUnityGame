using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class MazeManager : MonoBehaviour
{
    [SerializeField] private MazeGenerator m_MazeGenerator;
    private List<GameLevel> m_GameLevels;
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

                // Perform game preparation and start the game
                gamePreparation();
                GameManager.Instance.StartGame();
                return;
            }
        }

        Debug.Log("No matching game level found for name: " + i_Name);
    }

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

            gamePreparation();
            GameManager.Instance.StartGame();
        }
    }

    private void gamePreparation()
    {
        m_MazeGenerator.GenerateMazeInstant(currentGameLevel.Rows, currentGameLevel.Cols);
        setObstaclesLevel();
        setEnemiesLevel();
    }

    private void setObstaclesLevel()
    {

    }

    private void setEnemiesLevel()
    {

    }
}
