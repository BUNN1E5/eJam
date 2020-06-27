using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    public static AudioManager Instance
    {
        get
        {
            if (instance != null)
            {
                return instance;
            }

            GameObject managerObj = new GameObject("AudioManager");
            instance = managerObj.AddComponent<AudioManager>();
            return instance;
        }
    }

    // Initialized audio sources for manager
    #region
    private AudioSource musicSource1;
    private AudioSource musicSource2;
    private AudioSource sfxSource;
    #endregion

    private void Awake()
    {
        musicSource1 = this.gameObject.AddComponent<AudioSource>();
        musicSource2 = this.gameObject.AddComponent<AudioSource>();
        sfxSource = this.gameObject.AddComponent<AudioSource>();

        // Assigning audio mixer to each audio source
        AudioMixer MasterMixer = Resources.Load<AudioMixer>("Master");
        string _MixerGroup1 = "MusicSource1";
        string _MixerGroup2 = "MusicSource2";
        string _MixerGroup3 = "SFX";
        musicSource1.outputAudioMixerGroup = MasterMixer.FindMatchingGroups(_MixerGroup1)[0];
        musicSource2.outputAudioMixerGroup = MasterMixer.FindMatchingGroups(_MixerGroup2)[0];
        sfxSource.outputAudioMixerGroup = MasterMixer.FindMatchingGroups(_MixerGroup3)[0];
    }

    // Play one shot audio clips
    public void PlaySFX(AudioClip audioClip, float volume, float pitch)
    {
        sfxSource.volume = volume;
        sfxSource.pitch = pitch;
        sfxSource.PlayOneShot(audioClip);
    }

    // Play MusicSource1 with option to loop
    public void PlayMusicSource1(AudioClip audioClip, float volume, bool loop = false)
    {
        musicSource1.clip = audioClip;
        musicSource1.volume = volume;
        musicSource1.loop = loop;
        musicSource1.Play();
    }

    // Play MusicSource2 with option to loop
    public void PlayMusicSource2(AudioClip audioClip, float volume, bool loop = false)
    {
        musicSource2.clip = audioClip;
        musicSource2.volume = volume;
        musicSource2.loop = loop;
        musicSource2.Play();
    }

    // Stop MusicSource1
    public void StopMusicSource1()
    {
        musicSource1.Stop();
    }

    // Stop MusicSource2
    public void StopMusicSource2()
    {
        musicSource1.Stop();
    }

    // Pause MusicSource1
    public void PauseMusicSource1()
    {
        musicSource1.Pause();
    }

    // Pause MusicSource2
    public void PauseMusicSource2()
    {
        musicSource2.Pause();
    }

    // change MusicSource1 volume
    public void MusicSource1Vol(float volume)
    {
        musicSource1.volume = volume;
    }

    // change MusicSource2 volume
    public void MusicSource2Vol(float volume)
    {
        musicSource2.volume = volume;
    }
}
