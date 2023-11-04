using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    private HealthManager m_healthManager;

    private void Awake()
    {
        m_healthManager.OnDeath += startDyingAnimation;
    }

    public void MakeEnemyUnactive()
    {
        gameObject.SetActive(false);
        GameObject.Find("EnemiesSpawner").GetComponent<EnemiesSpawnerManager>().DecreaseEnemyCountByOneAndUpdateSpawnTimeNextEnemy();
    }

    public void MakeEnemyStepBack()
    {
        // Calculate the backward destination point.
        Vector3 backwardDestination = transform.position - transform.forward * 2.0f;

        // Set the destination for the NavMesh Agent to walk backward.
        gameObject.GetComponent<NavMeshAgent>().SetDestination(backwardDestination);
    }

    public void MakeEnemyGoAfterPlayer()
    {
        GameObject.Find("EnemiesSpawner").GetComponent<EnemiesSpawnerManager>().UpdateEnemyAgentDestinationToMainCamera(gameObject);
    }

    private void startDyingAnimation()
    {
        // start animation
        Debug.Log("Enemy dying animation");
    }
}