using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BeatManager : MonoBehaviour
{
    [SerializeField] float m_secondsBetweenBeat;
    [SerializeField] float m_startOffset;
    [SerializeField] float m_threshold;
    bool m_beatRequired;
    bool m_hitBeat;

    // Components
    AudioSource m_audioSource;

    // Start is called before the first frame update
    void Start()
    {
        m_audioSource = GetComponent<AudioSource>();
        m_beatRequired = false;
        m_hitBeat = false;
    }

    // Update is called once per frame
    void Update()
    {
        float currentTime = m_audioSource.time + m_startOffset;
        float timeSinceLastBeat = currentTime % m_secondsBetweenBeat;

        // End of our safety net, check if we hit otherwise register a miss
       if(m_beatRequired && timeSinceLastBeat > m_threshold)
        {
            // Register miss
            if(!m_hitBeat)
            {
                Debug.Log("Miss!");
            }
            m_hitBeat = false;
            m_beatRequired = false;
        }

        // Start of a new beat period
        if(timeSinceLastBeat <= m_threshold)
        {
            m_beatRequired = true;
        }
        
        // Only register the hit the once
        if(!m_hitBeat && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (timeSinceLastBeat <= m_threshold || (m_secondsBetweenBeat - timeSinceLastBeat) <= m_threshold)
            {
                Debug.Log("Hit!");
                m_hitBeat = true;
            }
        }
    }

}
