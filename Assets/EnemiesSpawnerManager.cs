using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesSpawnerManager : MonoBehaviour
{
    [SerializeField] private int m_EnemyDuplicationCount = 3;
    [SerializeField] private List<GameObject> m_EasyEnemiesToSpawn;
    [SerializeField] private List<GameObject> m_AdvancedEnemiesToSpawn;
    [SerializeField] private Transform m_PointToSpawnEnemies;
    [SerializeField] private int m_MaxEnemyCount = 2;
    [SerializeField] private float m_SecondsToWaitBetweenSpawningEnemies = 5;
    private int m_EnemyCount;
    private List<GameObject> m_EasyEnemiesToSpawnStorage = new List<GameObject>();
    private List<GameObject> m_AdvancedEnemiesToSpawnStorage = new List<GameObject>();


    // Start is called before the first frame update
    void Start()
    {
        setStorageOfEasyEnemiesToSpawn();
        setStorageOfAdvancedEnemiesToSpawn();

        //m_PointToSpawnEnemies = GameObject.Find("Maze Generator").GetComponent<MazeGenerator>().StartNode.transform.Find("StarterPointOfPlayer");
        //StartCoroutine(SpawnEnemyOnStartMaze(m_EasyEnemiesToSpawnStorage));
        //StartCoroutine(SpawnEnemyOnStartMaze(m_AdvancedEnemiesToSpawnStorage));

    }

    private void FixedUpdate()
    {
        
    }

    IEnumerator SpawnEnemyOnStartMaze(List<GameObject> i_EnemyStorage)
    {
        while(m_EnemyCount < m_MaxEnemyCount)
        {
            // Get a random integer between 0 (inclusive) and i_EnemyStorage.Count (exclusive).
            int randomIndex = Random.Range(0, i_EnemyStorage.Count);
            i_EnemyStorage[randomIndex].transform.SetPositionAndRotation(m_PointToSpawnEnemies.position, m_PointToSpawnEnemies.rotation);
            i_EnemyStorage[randomIndex].SetActive(true);
            yield return new WaitForSeconds(m_SecondsToWaitBetweenSpawningEnemies);
            m_EnemyCount++;
        }
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

    private void setStorageOfEasyEnemiesToSpawn()
    {
        setStorageOfEnemiesToSpawn(m_EasyEnemiesToSpawn, m_EasyEnemiesToSpawnStorage, "StorageOfEasyEnemiesToSpawn");
    }

    private void setStorageOfAdvancedEnemiesToSpawn()
    {
        setStorageOfEnemiesToSpawn(m_AdvancedEnemiesToSpawn, m_AdvancedEnemiesToSpawnStorage, "StorageOfAdvancedEnemiesToSpawn");
    }
}
