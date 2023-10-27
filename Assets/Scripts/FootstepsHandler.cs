using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepsHandler : MonoBehaviour
{
    [SerializeField] private AudioSource footstepAudioSource;
    [SerializeField] private AudioClip[] footstepClips;
    [SerializeField] private float stepInterval = 0.5f;
    [SerializeField] private float minMoveMagnitude = 0.1f; // Minimum movement magnitude to trigger footstep sounds.

    private Transform legsTransform; // Reference to the "Legs" GameObject's transform.
    private Vector3 previousPosition; // Store the previous position for magnitude calculation.
    private float stepCooldown = 0f;

    private void Start()
    {
        legsTransform = transform; // Cache the transform of the "Legs" GameObject.
        previousPosition = legsTransform.position;
    }

    private void FixedUpdate()
    {
        // Calculate the movement vector based on the position difference between frames.
        Vector3 movement = legsTransform.position - previousPosition;
        // Calculate the magnitude of movement (how far the "Legs" GameObject moved in one frame).
        float movementMagnitude = movement.magnitude;

        if (movementMagnitude > minMoveMagnitude && Time.time > stepCooldown)
        {
            PlayFootstepSound();
            stepCooldown = Time.time + stepInterval;
        }

        // Update the previous position for the next frame.
        previousPosition = legsTransform.position;
    }

    private void PlayFootstepSound()
    {
        if (footstepClips.Length == 0 || footstepAudioSource == null)
        {
            return;
        }

        AudioClip randomClip = footstepClips[Random.Range(0, footstepClips.Length)];
        footstepAudioSource.PlayOneShot(randomClip);
    }
}