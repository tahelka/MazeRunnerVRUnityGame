using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;
public enum eGameState
{
    Idle,
    Playing,
    GameOver
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public eGameState CurrentGameState { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Set the initial game state
        setGameStateToIdle();
    }

    private void setGameStateToIdle()
    {
        CurrentGameState = eGameState.Idle;
        Debug.Log("Game state: Idle");
    }

    public void setGameStateToPlaying()
    {
        CurrentGameState = eGameState.Playing;
        Debug.Log("Game state: Playing");
    }

    private void setGameStateToGameOver()
    {
        CurrentGameState = eGameState.GameOver;
        Debug.Log("Game state: GameOver");
    }

    public void StartGame()
    {
        if (CurrentGameState == eGameState.Idle)
        {
            setGameStateToPlaying();
            //m_Player.position = Vector3.zero;
            // teleport to the START node


            // 3 seconds count down
            // timer appear in the sky
        }
    }
}
