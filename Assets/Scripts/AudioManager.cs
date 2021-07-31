using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    
    public AudioClip mainMenuMusic;
    public AudioClip levelMusic;

    private AudioSource audioSource;

    private AudioClip nextSound = null;

    private float transitionOutTimer = 0;
    private float transitionInTimer = 0;
    private bool isTransitioning = false;

    private float originVolume;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        else
        {
            DontDestroyOnLoad(this);
            Instance = this;
        }
        
        audioSource = GetComponent<AudioSource>();

    }

    private void Start()
    {
        LevelManager.Instance.OnSceneLoad += OnSceneChange;
        
        audioSource.clip = mainMenuMusic;
        nextSound = audioSource.clip;
        audioSource.Play();
        
    }

    private void Update()
    {
        HandleSoundTransition();
    }

    private void HandleSoundTransition()
    {
        if (isTransitioning)
        {
            if (nextSound != audioSource.clip)
            {
                if (transitionOutTimer < 1)
                {
                    transitionOutTimer = Mathf.Clamp(transitionOutTimer + Time.deltaTime, 0, 1);
                
                    audioSource.volume = Mathf.Lerp(originVolume, 0, transitionOutTimer);
                }
                else
                {
                    transitionOutTimer = 0;
                    audioSource.clip = nextSound;
                    audioSource.Play();
                }
            }
            else
            {
                if (transitionInTimer < 1)
                {
                    transitionInTimer = Mathf.Clamp(transitionInTimer + Time.deltaTime, 0, 1);
                    audioSource.volume = Mathf.Lerp(0, originVolume, transitionInTimer);
                }
                else
                {
                    transitionInTimer = 0;
                    isTransitioning = false;
                }
            }   
        }
    }
    public void SetVolume(float volume)
    {
        if (isTransitioning)
            return;
        
        audioSource.volume = volume;
    }

    public void ChangeSound(AudioClip sound)
    {
        if (sound == audioSource.clip)
            return;
        
        nextSound = sound;
        isTransitioning = true;
        originVolume = audioSource.volume;
    }

    private void OnSceneChange(string sceneName)
    {
        if (sceneName == "Main_Menu")
        {
            ChangeSound(mainMenuMusic);
        }
        else if (sceneName == "FinishedLevel")
        {
            ChangeSound(levelMusic);
        }
    }
}
