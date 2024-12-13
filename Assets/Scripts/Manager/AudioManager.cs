using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System.Collections.Generic;
using System;

public class AudioManager : MonoBehaviour
{
    //handling volume
    [Header("Volume")]
    [Range(0,1)]
    public float masterVolume = 1;
    [Range(0,1)]
    public float musicVolume = 1;
    [Range(0,1)]
    public float sfxVolume = 1;

    //Initializing Bus from FMOD.Studio
    private Bus _masterBus;
    private Bus _musicBus;
    private Bus _sfxBus;

    public static AudioManager instance { get; private set;}

    //List of every event instances
    private List<EventInstance> _eventInstances;

    void Awake()
    {
        if(instance != null)
            Debug.Log("Found more than one Audio Manager in scene");
        instance = this;

        //Initiializing list 
        _eventInstances = new List<EventInstance>();

        //setting up buses
        _masterBus = RuntimeManager.GetBus("bus:/");
        _musicBus = RuntimeManager.GetBus("bus:/Music");
        _sfxBus = RuntimeManager.GetBus("bus:/SFX");
    }

    void Update()
    {
        //handling volume here
        _masterBus.setVolume(masterVolume);
        _musicBus.setVolume(musicVolume);
        _sfxBus.setVolume(sfxVolume);
    }

    /**
    *   Method to play only once a sound
    **/
    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }

    /**
    *   Permit to create an event instance from a reference
    **/
    public EventInstance CreateInstance(EventReference eventReference)
    {
        EventInstance _evInstance = RuntimeManager.CreateInstance(eventReference);
        _eventInstances.Add(_evInstance);
        return _evInstance;
    }
}
