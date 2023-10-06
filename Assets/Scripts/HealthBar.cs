using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider m_Slider;

    public void SetMaxHealth(int i_MaxHealth)
    {
        m_Slider.maxValue = i_MaxHealth;
        m_Slider.value = i_MaxHealth;
    }

    public void SetHealth(int i_Health)
    {
        m_Slider.value = i_Health;
    }
}
