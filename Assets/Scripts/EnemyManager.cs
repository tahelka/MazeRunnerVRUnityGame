using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private HealthManager m_healthManager;

    private void Awake()
    {
        m_healthManager.OnDeath += startDyingAnimation;
    }

    private void OnTriggerStay(Collider other)
    {
        if (tag == "Enemy" && !GetComponent<Animator>().GetBool("isDead") && other.tag == "Wall") // if its the enemy's collider and enemy isnt dead that gets into a wall
        {
            // reset path
            GetComponent<NavMeshAgent>().ResetPath();
        }
    }

    public void MakeEnemyDeactivate()
    {
        gameObject.SetActive(false);
        GameObject.Find("EnemiesSpawner").GetComponent<EnemiesSpawnerManager>().DecreaseEnemyCountByOneAndUpdateSpawnTimeNextEnemy();
    }

    private void startDyingAnimation()
    {
        // start animation
        Debug.Log("Enemy dying animation");
    }
}