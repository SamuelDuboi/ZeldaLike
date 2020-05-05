using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;
using System;
using Management;

public class AudioManager : Singleton<AudioManager>
{
    public List<Sound> sounds = new List<Sound>();

    private void Awake()
    {
        foreach(Sound sound in sounds)
        {
            sound.source= gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.outputAudioMixerGroup = sound.group;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        MakeSingleton(false);
        Play("BackGroundMusic");
    }

   public void Play(string name)
    {
        Sound s = sounds.Find(Sound => Sound.name == name);
        if(s.name == null)
        {
            Debug.Log("Warning the sound" + name + "not found");
            return;
        }
        if (!s.source.isPlaying)
        {
            s.source.Play();
        }
    }

    public void Stop(string name)
    {
        Sound s = sounds.Find(Sound => Sound.name == name);
        s.source.Stop();
    }
    
}
