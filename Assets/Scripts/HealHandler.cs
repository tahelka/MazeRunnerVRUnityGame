using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealHandler : MonoBehaviour
{
    [SerializeField] private int m_HealPoints;
    // [SerializeField] private GameObject m_HealParticle;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag is "Player")
        {
            other.GetComponent<HealthManager>().Heal(m_HealPoints);
            Debug.Log($"{other.name} got healed ({m_HealPoints} health)");

            // Instantiate healing particles
            /*Instantiate(
                m_HealParticle,
                new Vector3(
                    other.transform.position.x,
                    transform.position.y,
                    other.transform.position.z),
                    other.transform.rotation);*/

            Destroy(gameObject);
        }
    }
}
