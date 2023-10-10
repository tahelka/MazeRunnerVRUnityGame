using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class MazeManager : MonoBehaviour
{
    [SerializeField] private MazeGenerator m_MazeGenerator;
    [SerializeField] private Transform m_Player;
    [SerializeField] private Transform m_StarterRoom;
    private List<GameLevel> m_GameLevels;

    public GameLevel CurrentGameLevel { get; private set; }

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
                CurrentGameLevel = gameLevel;
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
                    CurrentGameLevel = gameLevel;
                }
            }

            mazePreparation();
            GameManager.Instance.StartGame();
        }
    }

    private void mazePreparation()
    {
        // Generate the maze
        m_MazeGenerator.GenerateMazeInstant(CurrentGameLevel ,CurrentGameLevel.Rows, CurrentGameLevel.Cols);

        // Move player to the start of the maze
        movePlayerToStartNode();
    }

    private void movePlayerToStartNode()
    {
        if (m_Player != null)
        {
            // Retrieve the script component attached to the "m_Player" game object
            PlayerController playerControllerScript = m_Player.GetComponent<PlayerController>();

            if (playerControllerScript != null)
            {
                // Call the method on the script component
                Transform starterPointOfPlayer = m_MazeGenerator.StartNode.transform.Find("StarterPointOfPlayer");
                playerControllerScript.ResetPosition(starterPointOfPlayer);
            }
            else
            {
                Debug.LogWarning("Player Controller script component not found on the m_Player game object.");
            }
        }
        else
        {
            Debug.LogWarning("m_Player game object not found.");
        }
    }

    public void EndTriggerEntered()
    {
        // Move player to the starter room
        movePlayerToStarterRoom();

        // Delete the maze
        m_MazeGenerator.DeleteMaze();
    }

    private void movePlayerToStarterRoom()
    {
        if (m_Player != null)
        {
            // Retrieve the script component attached to the "m_Player" game object
            PlayerController playerControllerScript = m_Player.GetComponent<PlayerController>();

            if (playerControllerScript != null)
            {
                // Call the method on the script component
                Transform starterPointOfPlayer = m_StarterRoom.Find("StarterPointOfPlayer");
                playerControllerScript.ResetPosition(starterPointOfPlayer);
            }
            else
            {
                Debug.LogWarning("Player Controller script component not found on the m_Player game object.");
            }
        }
        else
        {
            Debug.LogWarning("m_Player game object not found.");
        }
    }
}
