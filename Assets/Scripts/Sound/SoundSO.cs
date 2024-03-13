using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/SoundSO")]
public class SoundSO : ScriptableObject
{
    [Serializable]
    public class Sound
    {
        public SoundEnum sound;
        public AudioClip[] clip;
    }

    public List<Sound> soundList;

}