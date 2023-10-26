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
        if (other.tag is "Enemy")
        {
            other.GetComponent<Animator>().SetTrigger("isDamage");
            other.GetComponent<HealthManager>().TakeDamage(m_DamagePoints);
            Debug.Log($"{other.name} got hit ({m_DamagePoints} Damage)");
            // other.GetComponent<Animator>().SetTrigger("Hit"); // Take damage animation

            // Instantiate blood particles
            /*Instantiate(
                m_HitParticle,
                new Vector3(
                    other.transform.position.x,
                    transform.position.y,
                    other.transform.position.z),
                    other.transform.rotation);*/

            if (other.GetComponent<HealthManager>().GetHealth() <= 0)
            {
                GameObject.Find("EnemiesSpawner").GetComponent<EnemiesSpawnerManager>().MakeEnemyDead(other.gameObject);
            }

        }
        else if(other.tag is "Player")
        {         
            other.GetComponent<HealthManager>().TakeDamage(m_DamagePoints);
            Debug.Log($"{other.name} got hit ({m_DamagePoints} Damage)");         
        }
    }
}
