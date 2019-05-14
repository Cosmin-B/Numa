using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_SoundManager
{
    private Dictionary<string, AudioClip> _sounds;

    public Game_SoundManager()
    {
        _sounds = new Dictionary<string, AudioClip>();
    }

    public AudioClip GetAudioClip(string fileName)
    {

        AudioClip clip = null;

        foreach (KeyValuePair<string, AudioClip> entry in _sounds)
        {
            if (entry.Key == fileName)
            {
                clip = entry.Value;
                break;
            }
        }

        if (clip == null)
        {
            string path = "Audio/" + fileName;

            clip = Resources.Load(path, typeof(AudioClip)) as AudioClip;

            _sounds.Add(fileName, clip);
        }

        return clip;
    }

    public void PlaySoundForPlayer(AudioClip clip, bool loop = false)
    {
        AudioSource audioSource = Player_SoundHolder.audioSource;

        Debug.Assert(audioSource != null);
        Debug.Assert(clip != null);

        audioSource.loop = loop;

        audioSource.clip = clip;
        audioSource.Play();
    }

    public void StopPlaySoundForPlayer()
    {
        AudioSource audioSource = Player_SoundHolder.audioSource;

        Debug.Assert(audioSource != null);

        audioSource.loop = false;
        audioSource.Stop();
    }

    public void PlaySound(AudioSource audioSource, AudioClip clip)
    {
        Debug.Assert(audioSource != null);
        Debug.Assert(clip != null);

        audioSource.clip = clip;
        audioSource.Play();
    }
}
