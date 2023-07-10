using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public Sound[] Sounds;

    bool _isMuted = false;


    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        foreach (Sound s in Sounds)
        {
            s.Source = gameObject.AddComponent<AudioSource>();
            s.Source.playOnAwake = false;
            s.Source.clip = s.Clip;
            s.Source.volume = s.Volume;
            s.Source.pitch = s.Pitch;
            s.Source.loop = s.Loop;
        }
        DontDestroyOnLoad(gameObject);
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown("m"))
        {
            ToggleMute();
        }
    }

    public void ToggleMute()
    {
        _isMuted = !_isMuted;
        if (_isMuted)
        {
            AudioListener.volume = 0f;
        }
        else
        {
           AudioListener.volume = 1f; 
        }   
    }

    public void Play(string name)
    {
        Sound s = Array.Find(Sounds, sound => sound.Name == name);
        if (s == null)
        {
            Debug.LogWarning("Unable to find sound with name " + name);
            return;
        }
        s.Source.Play();
    }

    public void StopAllSounds()
    {
        foreach (Sound s in Sounds)
        {
            s.Source.Stop();
        }
    }
}
