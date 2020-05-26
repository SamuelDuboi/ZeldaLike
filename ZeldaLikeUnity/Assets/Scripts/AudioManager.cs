using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;
using System;
using Management;

public class AudioManager : Singleton<AudioManager>
{
    public AudioMixer masterMixer;
    public List<Sound> sounds = new List<Sound>();

    int FallEffect = 0;

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
        MakeSingleton(true);
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

    public void SpecialPlay(string name)
    {
        Sound s = sounds.Find(Sound => Sound.name == name);
        if (s.name == null)
        {
            Debug.Log("Warning the sound" + name + "not found");
            return;
        }
        s.source.Play();
    }

    public void Stop(string name)
    {
        Sound s = sounds.Find(Sound => Sound.name == name);
        if (s.name == null)
        {
            Debug.Log("Warning the sound" + name + "not found");
            return;
        }
        s.source.Stop();
    }

    public void Pause()
    {
        foreach(Sound s in sounds)
        {
            if(s.isMusic != true)
            {
                s.source.Pause();
            }
        }
    }

    public void UnPause()
    {
        foreach (Sound s in sounds)
        {
            if (s.name != "BackGroundMusic")
            {
                s.source.UnPause();
            }
        }
    }
    public void StopAll()
    {
        foreach (Sound s in sounds)
        {
            if (s.isMusic != true)
            {
                s.source.Stop();
            }
        }
    }

    public void StopAllMusic()
    {
        foreach (Sound s in sounds)
        {
            s.source.Stop();
        }
    }

    public void Fall(string name = "Inoh_Chute")
    {
        Sound s = sounds.Find(Sound => Sound.name == name);
        if(FallEffect == 0)
        {
            s.source.Play();
            FallEffect++;
        }
        else if (FallEffect > 0 && FallEffect < 3)
        {
            FallEffect++;
        }
        else if (FallEffect >= 3) 
        {
            FallEffect = 0;
        }
    }

    public void OptionSound(float volume)
    {
        masterMixer.SetFloat("MasterVolume", volume);
    }

}
