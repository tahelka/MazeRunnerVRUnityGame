using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemiesSpawnerManager : MonoBehaviour
{
    [SerializeField] private int m_EnemyDuplicationCount = 3;
    [SerializeField] private List<GameObject> m_EasyEnemiesToSpawn;
    [SerializeField] private List<GameObject> m_AdvancedEnemiesToSpawn;
    [SerializeField] private int m_MaxEnemyCount;
    [SerializeField] private float m_SecondsToWaitBetweenSpawningEnemies;

    private int m_CurrentEnemyCount;
    private float m_NextSpawnTime;
    private List<GameObject> m_EasyEnemiesToSpawnStorage;
    private List<GameObject> m_AdvancedEnemiesToSpawnStorage;
    private MazeManager m_MazeManager;
    private Transform m_PointToSpawnEnemies;
    private bool m_IsFunctionRunning = false;

    public List<GameObject> EasyEnemiesToSpawnStorage { get { return m_EasyEnemiesToSpawnStorage; } }
    public List<GameObject> AdvancedEnemiesToSpawnStorage { get { return m_AdvancedEnemiesToSpawnStorage; } }

    void Start()
    {
        m_MazeManager = GameObject.Find("Maze Manager").GetComponent<MazeManager>();
        GameManager.OnPlayModeStart += InitializeSpawnSettings;
        GameManager.OnPlayModeStart += SetEnemiesSpawnerSettings;
        m_EasyEnemiesToSpawnStorage = new List<GameObject>();
        m_AdvancedEnemiesToSpawnStorage = new List<GameObject>();
        setStorageOfEasyEnemiesToSpawn();
        setStorageOfAdvancedEnemiesToSpawn();
    }

    public void InitializeSpawnSettings()
    {
        m_NextSpawnTime = Time.time + m_SecondsToWaitBetweenSpawningEnemies;
        m_CurrentEnemyCount = 0;
        m_PointToSpawnEnemies = m_MazeManager.transform.Find("Maze Generator").GetComponent<MazeGenerator>().PointToSpawnEnemies;
        unactivateEnemies(m_EasyEnemiesToSpawnStorage);
        unactivateEnemies(m_AdvancedEnemiesToSpawnStorage);
    }

    void Update()
    {
        if (GameManager.Instance.CurrentGameState == eGameState.Playing)
        {
            // Update active enemies's position on maze
            switch (m_MazeManager.CurrentGameLevel.Name)
            {
                case "Medium":
                    updateEnemiesAgentDestinationToMainCamera(m_EasyEnemiesToSpawnStorage);
                    if (!m_IsFunctionRunning && m_CurrentEnemyCount < m_MaxEnemyCount && Time.time > m_NextSpawnTime)
                    {
                        m_IsFunctionRunning = true;
                        // Spawn more enemies
                        SpawnEnemyOnStartMaze(m_EasyEnemiesToSpawnStorage);
                        m_IsFunctionRunning = false;
                    }
                    break;

                case "Hard":
                    updateEnemiesAgentDestinationToMainCamera(m_AdvancedEnemiesToSpawnStorage);
                    if (!m_IsFunctionRunning && m_CurrentEnemyCount < m_MaxEnemyCount && Time.time > m_NextSpawnTime)
                    {
                        m_IsFunctionRunning = true;
                        // Spawn more enemies                     
                        SpawnEnemyOnStartMaze(m_AdvancedEnemiesToSpawnStorage);                  
                        m_IsFunctionRunning = false;
                    }
                    break;
            }                  
        }                 
    }

    private void unactivateEnemies(List<GameObject> i_Enemies)
    {
        foreach (GameObject enemy in i_Enemies)
        {
            if (enemy.activeSelf)
            {
                enemy.SetActive(false);
            }
        }
    }

    public void SetEnemiesSpawnerSettings()
    {
        switch (m_MazeManager.CurrentGameLevel.Name)
        {
            case "Medium":
                m_MaxEnemyCount = 2;
                m_SecondsToWaitBetweenSpawningEnemies = 7;
                break;

            case "Hard":
                m_MaxEnemyCount = 3;
                m_SecondsToWaitBetweenSpawningEnemies = 5;
                break;
        }
       
    }

    //public void PrepareToSpawnEnemies(Transform i_PointToSpawnEnemies)
    //{
    //    m_PointToSpawnEnemies = i_PointToSpawnEnemies;
    //}

    public void SpawnEnemyOnStartMaze(List<GameObject> i_EnemyStorage)
    {
        // Get a random integer between 0 (inclusive) and i_EnemyStorage.Count (exclusive).
        int randomIndex = Random.Range(0, i_EnemyStorage.Count);
        if (!i_EnemyStorage[randomIndex].activeSelf) // if enemy is not active
        {
            //Debug.Log(i_EnemyStorage[randomIndex].GetComponent<Animator>().GetBool("isDead"));
            //if (i_EnemyStorage[randomIndex].GetComponent<Animator>().GetBool("isDead"))
            //{
            //    Debug.Log("enemy was dead");
            //    // enemy was in the game already and died
            //}
            initEnemySettings(i_EnemyStorage[randomIndex]);
            i_EnemyStorage[randomIndex].SetActive(true);
            //updateEnemyAgentDestinationToMainCamera(i_EnemyStorage[randomIndex]);
            m_CurrentEnemyCount++;
        }    
    }

    public void MakeEnemyDead(GameObject enemy)
    {
        enemy.GetComponent<Animator>().SetBool("isDead", true);
        // stop enemy to run after the player
        enemy.GetComponent<NavMeshAgent>().isStopped = true;
    }

    public void DecreaseEnemyCountByOneAndUpdateSpawnTimeNextEnemy()
    {
        m_CurrentEnemyCount--;
        InitializeSpawnSettings();
    }

    private void initEnemySettings(GameObject enemy)
    {
        enemy.transform.SetPositionAndRotation(m_PointToSpawnEnemies.position, m_PointToSpawnEnemies.rotation);
        enemy.GetComponent<HealthManager>().ResetHealth();
    }

    private void setStorageOfEnemiesToSpawn(List<GameObject> i_EnemyToSpawnList, List<GameObject> i_EnemyToSpawnListStorage, string i_NameOfStorage)
    {
         // Create a new empty GameObject
        GameObject StorageOfEnemiesToSpawn = new GameObject(i_NameOfStorage);

        // Find the GameObject with the name "EnemiesSpawner"
        GameObject enemiesSpawner = GameObject.Find("EnemiesSpawner");

        // Check if the GameObject was not found
        if (enemiesSpawner == null)
        {
            Debug.LogWarning("No GameObject with the name 'EnemiesSpawner' found.");
        }

        // Set the parent of the new GameObject to EnemiesSpawner
        StorageOfEnemiesToSpawn.transform.SetParent(enemiesSpawner.transform);

        foreach (GameObject enemy in i_EnemyToSpawnList)
        {
            for (int i = 0; i < m_EnemyDuplicationCount; i++)
            {
                GameObject duplicatedEnemy = Instantiate(enemy);
                duplicatedEnemy.SetActive(false);
                duplicatedEnemy.transform.SetParent(StorageOfEnemiesToSpawn.transform);
                i_EnemyToSpawnListStorage.Add(duplicatedEnemy);
            }
        }
    }

    private void updateEnemiesAgentDestinationToMainCamera(List<GameObject> i_Enemies)
    {
        foreach (GameObject enemy in i_Enemies)
        {
            if (enemy.activeSelf)
            {
                //updateEnemyAgentDestinationToMainCamera(enemy);
                enemy.GetComponent<NavMeshAgent>().destination = GameObject.Find("Main Camera").transform.position;
            }
        }
    }

    private void updateEnemyAgentDestinationToMainCamera(GameObject enemy)
    {
        enemy.GetComponent<NavMeshAgent>().destination = GameObject.Find("Main Camera").transform.position;
        //foreach (GameObject enemy in i_Enemies)
        //{
        //    if (enemy.active)
        //    {
        //        enemy.GetComponent<NavMeshAgent>().destination = GameObject.Find("Main Camera").transform.position;
        //    }
        //}   
    }

    private void setStorageOfEasyEnemiesToSpawn()
    {
        setStorageOfEnemiesToSpawn(m_EasyEnemiesToSpawn, m_EasyEnemiesToSpawnStorage, "StorageOfEasyEnemiesToSpawn");
    }

    private void setStorageOfAdvancedEnemiesToSpawn()
    {
        setStorageOfEnemiesToSpawn(m_AdvancedEnemiesToSpawn, m_AdvancedEnemiesToSpawnStorage, "StorageOfAdvancedEnemiesToSpawn");
    }
}
