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

public class SoundManager : MonoBehaviour
{
    protected AudioSource MyAudioSource;
    // AudioSource AudioSource_Announcer;

    public AudioClip Clip_WalkSingle;
    public AudioClip Clip_WalkMulti;

    public RandomSoundCollection Announcer_StartClips;

    //public AudioClip[] Announcer_StartClips;

    // Start is called before the first frame update
    void Start()
    {
        MyAudioSource = GetComponent<AudioSource>();
        //MyAudioSource.Play(); // Play the sound associated with the source

        // Play other sounds.  Multiple can play at once.
        //MyAudioSource.PlayOneShot(Clip_WalkSingle);
        //MyAudioSource.PlayOneShot(Clip_WalkMulti);
    }

    void Update()
    {
        if (UnityEngine.Input.GetKeyDown(KeyCode.Q))
        {
            PlayRandomClip(Announcer_StartClips);
        }
    }

    void PlayRandomClip(RandomSoundCollection collection)
    {
        AudioClip clip = collection.GetRandomClip();
        if (clip != null)
        {
            MyAudioSource.clip = clip;
            MyAudioSource.Play();
        }
    }
}
