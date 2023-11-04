using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackHandler : MonoBehaviour
{
    [SerializeField] private int m_DamagePoints;
    private List<string> m_AttacksAnimations = new List<string>();
    // [SerializeField] private GameObject m_HitParticle;

    private void Awake()
    {
        m_AttacksAnimations.Add("Attack1");
        m_AttacksAnimations.Add("Attack2");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (tag == "Enemy" && !GetComponent<Animator>().GetBool("isDead")) // if its the enemy's collider and enemy isnt dead

        {
            if (other.tag is "Player") // if player got into the enemy's collider
            {
                // take off health points from the player
                other.GetComponent<HealthManager>().TakeDamage(m_DamagePoints);
                Debug.Log($"{other.name} got hit ({m_DamagePoints} Damage)");
                doSpiderAttackAnimation();

                // Instantiate blood particles
                /*Instantiate(
                    m_HitParticle,
                    new Vector3(
                        other.transform.position.x,
                        transform.position.y,
                        other.transform.position.z),
                        other.transform.rotation);*/

            }
            else if (other.tag == "Weapon") // if weapon got into enemy's collider
            {
                HealthManager healthManagerComponent = GetComponent<HealthManager>();
                // take off health points from the enemy
                healthManagerComponent.TakeDamage(m_DamagePoints);
                Debug.Log($"{name} got hit ({m_DamagePoints} Damage)");

                if (healthManagerComponent.GetHealth() <= 0)
                {
                    // do the death animation of enemy
                    GameObject.Find("EnemiesSpawner").GetComponent<EnemiesSpawnerManager>().MakeEnemyDead(gameObject);
                }
                else
                {
                    // do the taking damage animation of enemy
                    GetComponent<Animator>().SetTrigger("isDamage");
                }
            }
        }          
    }

    private void doSpiderAttackAnimation()
    {
        // Get a random integer between 0 (inclusive) and m_AttacksAnimations.Count (exclusive).
        int randomIndex = Random.Range(0, m_AttacksAnimations.Count);
        GetComponent<Animator>().SetTrigger(m_AttacksAnimations[randomIndex]);
    }
}
