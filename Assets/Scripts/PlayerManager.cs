using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private MazeManager m_MazeManager;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "EndTrigger")
        {
            Debug.Log("End Trigger was activated");
            m_MazeManager.EndTriggerEntered();
        }
    }
}
