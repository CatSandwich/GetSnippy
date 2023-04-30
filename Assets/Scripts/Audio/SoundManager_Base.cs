using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RandomSoundCollection
{
    public AudioClip[] clips;
    protected int lastClip = -1;

    public AudioClip GetRandomClip()
    {
        if (clips.Length == 0)
        {
            return null;
        }

        if (clips.Length == 1)
        {
            return clips[0];
        }

        if (lastClip == -1)
        {
            lastClip = UnityEngine.Random.Range(0, clips.Length);
            return clips[lastClip];
        }

        // Case where we have multiple clips and don't want to repeat the last one.
        int index = UnityEngine.Random.Range(0, clips.Length - 1);
        if (index == lastClip)
        {
            index = clips.Length - 1;
        }
        lastClip = index;
        return clips[lastClip];
    }
}

public class SoundManager_Base : MonoBehaviour
{
    protected AudioSource MyAudioSource;

    //public RandomSoundCollection Announcer_StartClips;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        MyAudioSource = GetComponent<AudioSource>();
    }

    protected virtual void Update()
    {
        // Test example
        //if (UnityEngine.Input.GetKeyDown(KeyCode.Q))
        //{
        //    PlayRandomClip(Announcer_StartClips);
        //}
    }

    protected void PlayRandomClip(RandomSoundCollection collection, bool onMainChannel)
    {
        AudioClip clip = collection.GetRandomClip();
        if (clip != null)
        {
            if (onMainChannel)
            {
                MyAudioSource.clip = clip;
                MyAudioSource.Play();
            }
            else
            {
                MyAudioSource.PlayOneShot(clip);
            }
        }
    }

    // Always plays on main channel
    protected void PlayRandomClipDelayed(RandomSoundCollection collection, float delay)
    {
        AudioClip clip = collection.GetRandomClip();
        if (clip != null)
        {
            MyAudioSource.clip = clip;
            MyAudioSource.PlayDelayed(delay);
        }
    }
}
