using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyCollision : MonoBehaviour
{
    [SerializeField] private Transform m_Head;
    [SerializeField] private Transform m_Feet;
    
    void Update()
    {
        gameObject.transform.position = new Vector3(m_Head.position.x, m_Feet.position.y, m_Head.position.z);
    }
}
