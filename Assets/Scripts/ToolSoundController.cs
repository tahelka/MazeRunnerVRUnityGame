using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ToolSoundController : MonoBehaviour
{
    [SerializeField] private Transform tipTransform; // Reference to the tip transform.
    private AudioSource m_AudioSource;
    [SerializeField] private AudioClip m_AudioClip;
    [SerializeField] private bool m_UseVelocity = true;
    [SerializeField] private float m_VelocityThreshold = 15f; // Adjust this value to control the velocity threshold.
    [SerializeField] private float m_DistanceThreshold = 1f; // Adjust this value to control the distance threshold.
    
    [SerializeField] private float m_MinVelocity = 15;
    [SerializeField] private float m_MaxVelocity = 30;
    [SerializeField] private float m_MaxVolume = 1.0f; // Maximum volume for the sound.

    private XRGrabInteractable grabInteractable; // Reference to the XRGrabInteractable component.
    private Vector3 previousTipPosition; // Store the previous tip position.
    private float distanceTraveled; // Cumulative distance traveled during a swing.
    private bool isSwinging; // Track if a swing is in progress.


    private void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();

        // Get the XRGrabInteractable component.
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    private void Update()
    {
        // Check if the tool is currently held by the player.
        if (IsHeldByPlayer())
        {
            Vector3 tipVelocity = (tipTransform.position - previousTipPosition) / Time.deltaTime;

            if (m_UseVelocity)
            {
                float tipVelocityMagnitude = tipVelocity.magnitude;
                float volume = Mathf.InverseLerp(m_MinVelocity, m_MaxVelocity, tipVelocityMagnitude) * m_MaxVolume;
                //Debug.Log($"tipVelocityMagnitude: {tipVelocityMagnitude}");
                //Debug.Log($"volume: {volume}");

                // Check if the tool's velocity exceeds the velocity threshold.
                if (tipVelocityMagnitude > m_VelocityThreshold)
                {
                    if (!isSwinging)
                    {
                        isSwinging = true;
                        distanceTraveled = 0f; // Reset distance traveled at the start of the swing.
                    }

                    // Calculate the distance traveled during the current swing.
                    distanceTraveled += Vector3.Distance(tipTransform.position, previousTipPosition);

                    if (!m_AudioSource.isPlaying && distanceTraveled >= m_DistanceThreshold)
                    {
                        m_AudioSource.PlayOneShot(m_AudioClip, volume);
                    }
                }
                else
                {
                    isSwinging = false; // Reset swing state when the tip's velocity drops.
                }
            }
            else
            {
                m_AudioSource.PlayOneShot(m_AudioClip);
            }

            // Store the current tip position for the next frame.
            previousTipPosition = tipTransform.position;
        }
        else
        {
            isSwinging = false; // Reset swing state if not held by the player.
        }
    }

    private bool IsHeldByPlayer()
    {
        // Implement logic to check if the tool is held by the player using the XRGrabInteractable component.
        return (grabInteractable != null && grabInteractable.isSelected);
    }
}
