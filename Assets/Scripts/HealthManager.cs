using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [SerializeField] private HealthBar m_HealthBar;
    [SerializeField] private int m_MaxHealth;
    private int m_CurrentHealth;

    public event Action OnDeath;

    private void Awake()
    {
        m_HealthBar.SetMaxHealth(m_MaxHealth);
        m_CurrentHealth = m_MaxHealth;
    }

    public int GetHealth()
    {
        return m_CurrentHealth;
    }

    public void TakeDamage(int i_DamagePoints)
    {
        m_CurrentHealth -= i_DamagePoints;

        if (m_CurrentHealth < 0)
        {
            m_CurrentHealth = 0;
        }

        m_HealthBar.SetHealth(m_CurrentHealth);

        if(m_CurrentHealth == 0)
        {
            Die();
        }
    }

    public void Heal(int i_HealthPoints)
    {
        m_CurrentHealth += i_HealthPoints;

        if (m_CurrentHealth > m_MaxHealth)
        {
            m_CurrentHealth = m_MaxHealth;
        }

        m_HealthBar.SetHealth(m_CurrentHealth);
    }

    public void ResetHealth()
    {
        m_CurrentHealth = m_MaxHealth;
        m_HealthBar.SetHealth(m_MaxHealth);
    }

    private void Die()
    {
        OnDeath?.Invoke();
    }
}
