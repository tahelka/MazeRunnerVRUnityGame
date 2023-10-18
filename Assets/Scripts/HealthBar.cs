using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider m_Slider;
    private int m_MaxHealth;

    public void SetMaxHealth(int i_MaxHealth)
    {
        m_MaxHealth = i_MaxHealth;
    }

    public void SetHealth(int i_Health)
    {
        m_Slider.value = (float)i_Health/m_MaxHealth;
    }

}
