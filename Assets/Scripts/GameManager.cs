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
    [SerializeField] private MazeManager m_MazeManager;
    [SerializeField] private GameObject m_EnemiesAndObsticlesManager;
    [SerializeField] private Timer m_Timer;
    [SerializeField] private HealthManager m_HealthManager;
    [SerializeField] private GameOver m_GameOver;
    public static GameManager Instance { get; private set; }
    public eGameState CurrentGameState { get; private set; }
    public string PlayerName { get; private set; }

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
        GamePreparation();
    }

    public void GamePreparation()
    {
        m_Timer.ResetTimer();
        m_HealthManager.ResetHealth();
    }

    private void setGameStateToIdle()
    {
        CurrentGameState = eGameState.Idle;
        Debug.Log("Game state: Idle");
    }

    private void setGameStateToPlaying()
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
            // Start the timer
            m_Timer.StartTimer();
        }

        // spawnEnemiesOnMaze();
    }


    public void EndGame()
    {
        setGameStateToGameOver();
        m_MazeManager.EndTriggerEntered();
        m_Timer.StopTimer();
        m_GameOver.DisplayGameOverMenu(m_HealthManager.GetHealth());
    }

    private void spawnEnemiesOnMaze()
    {
        EnemiesSpawnerManager enemiesSpawnerManagerScript = m_EnemiesAndObsticlesManager.GetComponentInChildren<EnemiesSpawnerManager>();
        Transform pointToSpawnEnemies = m_MazeManager.GetComponentInChildren<MazeGenerator>().StartNode.transform.Find("StarterPointOfPlayer");
        // Start the coroutine with a 10-second delay before spawning enemies.
        StartCoroutine(executeAfterDelay(10.0f));
        StartCoroutine(enemiesSpawnerManagerScript.SpawnEnemyOnStartMaze(enemiesSpawnerManagerScript.EasyEnemiesToSpawnStorage, pointToSpawnEnemies));
        StartCoroutine(enemiesSpawnerManagerScript.SpawnEnemyOnStartMaze(enemiesSpawnerManagerScript.AdvancedEnemiesToSpawnStorage, pointToSpawnEnemies));
    }

    private IEnumerator executeAfterDelay(float delay)
    {
        Debug.Log("Coroutine started. Waiting for " + delay + " seconds.");

        yield return new WaitForSeconds(delay);

        // Code to be executed after the delay
        Debug.Log("Coroutine resumed after " + delay + " seconds.");
    }

    private IEnumerator ExecuteAfterDelay(float delay)
    {
        Debug.Log("Coroutine started. Waiting for " + delay + " seconds.");

        yield return new WaitForSeconds(delay);

        // Code to be executed after the delay
        Debug.Log("Coroutine resumed after " + delay + " seconds.");
    }

    public void SetPlayerName(string name)
    {
        if (name != string.Empty)
        {
            PlayerName = name;
            Debug.Log($"Player name set to: {PlayerName}");
        }
    }
}
