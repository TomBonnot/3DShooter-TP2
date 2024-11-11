using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System.Collections.Generic;
using System;

public class AudioManager : MonoBehaviour
{
    [Header("Volume")]
    [Range(0,1)]
    public float masterVolume = 1;
    [Range(0,1)]
    public float musicVolume = 1;
    [Range(0,1)]
    public float sfxVolume = 1;

    private Bus _masterBus;
    private Bus _musicBus;
    private Bus _sfxBus;

    public static AudioManager instance { get; private set;}

    private List<EventInstance> _eventInstances;

    void Awake()
    {
        if(instance != null)
            Debug.Log("Found more than one Audio Manager in scene");
        instance = this;
        _eventInstances = new List<EventInstance>();
        _masterBus = RuntimeManager.GetBus("bus:/");
        _musicBus = RuntimeManager.GetBus("bus:/Music");
        _sfxBus = RuntimeManager.GetBus("bus:/SFX");
    }

    void Start()
    {
        
    }

    void Update()
    {
        _masterBus.setVolume(masterVolume);
        _musicBus.setVolume(musicVolume);
        _sfxBus.setVolume(sfxVolume);
    }

    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }

    public EventInstance CreateInstance(EventReference eventReference)
    {
        EventInstance _evInstance = RuntimeManager.CreateInstance(eventReference);
        _eventInstances.Add(_evInstance);
        return _evInstance;
    }
}
