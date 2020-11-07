using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

public class BeatQueue : MonoBehaviour
{
    [SerializeField] GameObject m_audioSourceObject;
    [SerializeField] float m_threshold;
    [SerializeField] uint m_beatDivisor;
    [SerializeField] uint m_countBeats;
    [SerializeField] uint m_lookAheadTime;

    SongData m_songData;

    bool m_hitBeat;

    float m_timeSinceLastBeat;
    List<float> m_timeToBeats;

    // Components
    AudioSource m_audioSource;

    // Start is called before the first frame update
    void Start()
    {
        // Get components
        m_audioSource = m_audioSourceObject.GetComponent<AudioSource>();

        Assert.IsTrue(m_beatDivisor > 0u);
        m_songData = m_audioSourceObject.GetComponent<Song>().GetSongData();
        m_timeSinceLastBeat = Mathf.Infinity;
        m_timeToBeats = new List<float>();
        m_hitBeat = false;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateQueue();

        if(Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if(HitBeat() && !m_hitBeat)
            {
                Debug.Log("Hit");
                m_hitBeat = true;
            }
            else
            {
                Debug.Log("Miss");
            }
        }
    }

    void UpdateQueue()
    {
        // Reset the queue
        m_timeToBeats.Clear();

        float currentTime = m_audioSource.time + m_songData.m_startOffset;
        float timeSinceLastBar = currentTime % m_songData.m_secondsBetweenBar;
        float timeLeftOnTrack = m_audioSource.clip.length - m_audioSource.time;
        float timeBetweenBeats = m_songData.m_secondsBetweenBar / (float)m_beatDivisor;

        // 1. Get what 'beat' division we are on
        float timeSinceLastBeat = 0.0f;
        float beatTime = 0.0f;
        uint currentBeat = 0;
        while(beatTime < timeSinceLastBar)
        {
            // Get what beat of the bar we are on
            currentBeat = (currentBeat + 1u) % m_beatDivisor;
            timeSinceLastBeat = timeSinceLastBar - beatTime;
            beatTime += timeBetweenBeats;
        }

        // 2. Find time since last beat
        float oldTimeSinceLastBeat = m_timeSinceLastBeat;
        m_timeSinceLastBeat = timeSinceLastBeat;
        for(uint i = 0; i <= m_beatDivisor; ++i)
        {
            currentBeat = (currentBeat - i) % m_beatDivisor;
            int currentBeatBinary = 1 << (int)currentBeat;
            if((currentBeatBinary & m_countBeats) != 0)
            {
                break;
            }

            m_timeSinceLastBeat += timeBetweenBeats;
        }

        // 3. Get time to next beats
        float lookAheadTime = beatTime - timeSinceLastBar;
        while (lookAheadTime < m_lookAheadTime)
        {
            if(lookAheadTime > timeLeftOnTrack)
            {
                break;
            }

            currentBeat = (currentBeat + 1u) % m_beatDivisor;
            int currentBeatBinary = 1 << (int)currentBeat;
            if ((currentBeatBinary & m_countBeats) != 0)
            {
                m_timeToBeats.Add(lookAheadTime);
            }

            lookAheadTime += timeBetweenBeats;
        }
    }

    // Register whether whatever action we just did hit a beat
    bool HitBeat()
    {
        // Make sure we have the latest beat info as this is an important calculation
        UpdateQueue();

        float timeSinceLastBeat = GetTimeSinceLastBeat();
        float timeTillNextBeat = GetTimeToNextBeat();

        if(m_hitBeat && timeSinceLastBeat > m_threshold)
        {
            m_hitBeat = false;
        }

        if (timeSinceLastBeat <= m_threshold || timeTillNextBeat <= m_threshold)
        {
            return true;
        }

        return false;
    }

    float GetTimeToNextBeat()
    {
        if(m_timeToBeats.Count > 0)
        {
            return m_timeToBeats[0];
        }

        return Mathf.Infinity;
    }

    float GetTimeSinceLastBeat()
    {
        return m_timeSinceLastBeat;
    }
}
