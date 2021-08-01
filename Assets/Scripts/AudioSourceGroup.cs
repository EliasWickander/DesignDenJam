using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class AudioSourceGroup : MonoBehaviour
{
    public AudioSource audioSource;
    public VolumeType type;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    
    private void Update()
    {
        float masterVolume = AudioManager.Instance.volumePerType[VolumeType.Master];
        float typeVolume = AudioManager.Instance.volumePerType[type];

        audioSource.volume = masterVolume * typeVolume;
    }
}
