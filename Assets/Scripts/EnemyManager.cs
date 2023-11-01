using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        GameObject.Find("EnemiesSpawner").GetComponent<EnemiesSpawnerManager>().DecreaseEnemyCountByOne();
    }


    private void startDyingAnimation()
    {
        // start animation
        Debug.Log("Enemy dying animation");
    }
}