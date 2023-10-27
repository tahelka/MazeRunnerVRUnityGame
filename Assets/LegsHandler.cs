using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegsHandler : MonoBehaviour
{
    [SerializeField] private Transform m_PlayerHead; // Reference to the player (e.g., VR rig or camera) that the "legs" should follow.

    private Transform legsTransform; // Reference to the "legs" GameObject's transform.

    private void Start()
    {
        legsTransform = transform; // Cache the transform of the "legs" GameObject.
    }

    private void Update()
    {
        if (m_PlayerHead != null)
        {
            // Calculate the new position for the "legs" GameObject.
            Vector3 newPosition = new Vector3(m_PlayerHead.position.x, legsTransform.position.y, m_PlayerHead.position.z);

            // Set the position of the "legs" GameObject to maintain the y-position.
            legsTransform.position = newPosition;
        }
    }
}