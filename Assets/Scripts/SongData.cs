using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "NewSongData", menuName = "SongData")]
public class SongData : ScriptableObject
{
    public float m_secondsBetweenBar;
    public float m_startOffset;
}
