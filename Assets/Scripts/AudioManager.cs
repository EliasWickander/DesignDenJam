using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    
    public AudioClip mainMenuMusic;
    public AudioClip levelMusic;

    [Range(0, 1)]
    public float masterStartSound = 0.5f;
    
    [Range(0, 1)]
    public float musicStartSound = 0.5f;
    
    [Range(0, 1)]
    public float sfxStartSound = 0.5f;
    
    [Range(0, 1)]
    public float voiceStartSound = 0.5f;

    private AudioSource audioSource;

    public Dictionary<VolumeType, float> volumePerType = new Dictionary<VolumeType, float>();

    public event Action<VolumeType> OnVolumeChange;
    
    private AudioClip nextSound = null;

    private float transitionOutTimer = 0;
    private float transitionInTimer = 0;
    private bool isTransitioning = false;

    private float originVolume;

    private string nextScene;
    private bool transitioningScene = false;

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

        nextScene = SceneManager.GetActiveScene().name;
        audioSource = GetComponent<AudioSource>();

        for (int i = 0; i < 4; i++)
        {
            volumePerType.Add((VolumeType)i, 1);
        }
    }

    private void Start()
    {
        LevelManager.Instance.OnSceneLoad += OnSceneChange;
        
        SetVolume(VolumeType.Master, masterStartSound);
        SetVolume(VolumeType.Music, musicStartSound);
        SetVolume(VolumeType.SFX, sfxStartSound);
        SetVolume(VolumeType.Voice, voiceStartSound);

        ChangeToSceneSound(SceneManager.GetActiveScene().name);
        nextSound = audioSource.clip;

    }

    private void Update()
    {
        if (transitioningScene)
        {
            if (nextScene == SceneManager.GetActiveScene().name)
            {
                transitioningScene = false;
                LevelManager.Instance.OnSceneLoad += OnSceneChange;
            }   
        }

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
    public void SetVolume(VolumeType type, float volume)
    {
        if (isTransitioning)
            return;

        volumePerType[type] = volume;
        OnVolumeChange?.Invoke(type);
    }

    public void ChangeSound(AudioClip sound)
    {
        if (sound == audioSource.clip)
        {
            return;   
        }

        if (audioSource.clip == null)
        {
            audioSource.clip = sound;
            audioSource.Play();
            return;
        }

        nextSound = sound;
        isTransitioning = true;
        originVolume = audioSource.volume;
    }

    private void OnSceneChange(string sceneName)
    {
        transitioningScene = true;
        nextScene = sceneName;
        ChangeToSceneSound(sceneName);
    }

    private void ChangeToSceneSound(string sceneName)
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
