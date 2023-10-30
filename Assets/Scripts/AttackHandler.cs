using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackHandler : MonoBehaviour
{
    [SerializeField] private int m_DamagePoints;
    // [SerializeField] private GameObject m_HitParticle;

    private void OnTriggerEnter(Collider other)
    {
        if (tag == "Enemy" && other.tag is "Player") // if player got into the enemy's collider
        {
            // take off health points from the player
            other.GetComponent<HealthManager>().TakeDamage(m_DamagePoints);
            Debug.Log($"{other.name} got hit ({m_DamagePoints} Damage)");

            // Instantiate blood particles
            /*Instantiate(
                m_HitParticle,
                new Vector3(
                    other.transform.position.x,
                    transform.position.y,
                    other.transform.position.z),
                    other.transform.rotation);*/

        }
        else if(tag == "Enemy" && other.tag == "Weapon") // if weapon got into enemy's collider
        {
            // do the taking damage animation of enemy
            GetComponent<Animator>().SetTrigger("isDamage");

            HealthManager healthManagerComponent = GetComponent<HealthManager>();
            // take off health points from the enemy
            healthManagerComponent.TakeDamage(m_DamagePoints);
            Debug.Log($"{name} got hit ({m_DamagePoints} Damage)");

            if (healthManagerComponent.GetHealth() <= 0)
            {
                // do the death animation of enemy
                GameObject.Find("EnemiesSpawner").GetComponent<EnemiesSpawnerManager>().MakeEnemyDead(gameObject);
            }
        }
    }
}
