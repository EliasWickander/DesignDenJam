using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum VolumeType
{
    Master,
    Music,
    SFX,
    Voice,
}
public class AudioSlider : MonoBehaviour
{
    private Slider audioSlider;
    public VolumeType type;
    
    private void Awake()
    {
        audioSlider = GetComponent<Slider>();
    }

    private void Start()
    {
        audioSlider.onValueChanged.AddListener(SetVolumeOfType);
        
        UpdateSlider(type);
        AudioManager.Instance.OnVolumeChange += UpdateSlider;
    }

    private void OnDestroy()
    {
        audioSlider.onValueChanged.RemoveListener(SetVolumeOfType);
    }

    private void SetVolumeOfType(float volume)
    {
        AudioManager.Instance.SetVolume(type, volume);
    }

    private void UpdateSlider(VolumeType type)
    {
        if (this.type == type)
        {
            audioSlider.value = AudioManager.Instance.volumePerType[type];
        }
    }
}
