using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSlider : MonoBehaviour
{
    private Slider audioSlider;

    private void Awake()
    {
        audioSlider = GetComponent<Slider>();
    }

    private void Start()
    {
        audioSlider.onValueChanged.AddListener(AudioManager.Instance.SetVolume);
    }

    private void OnDestroy()
    {
        audioSlider.onValueChanged.RemoveListener(AudioManager.Instance.SetVolume);
    }
}
