using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using static CrabClaw;

public class SoundManager_Global : SoundManager_Base
{
    public GameManager myGameManager;

    public RandomSoundCollection collectionStart;
    public RandomSoundCollection collectionSnipFirstCrab;
    public RandomSoundCollection collectionSnipSecondCrab;
    public RandomSoundCollection collectionEnd;

    public AudioClip bigWaveClip;

    private int snipCount = 0;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        myGameManager.GameStart += OnBattleStart;
        myGameManager.CrabSpawn += OnCrabSpawn;
        myGameManager.GameEnd += OnBattleEnd;
        myGameManager.EndWave += OnEndWave;
    }

    void OnBattleStart()
    {
        snipCount = 0;

        if (bigWaveClip)
        {
            MyAudioSource.PlayOneShot(bigWaveClip, 0.4f);
        }

        PlayRandomClipDelayed (collectionStart, 3.0f);
    }

    void OnCrabSpawn(CrabBody newCrab)
    {
        newCrab.LostFirstEye += OnSnip;
    }

    void OnSnip()
    {
        ++snipCount;

        if (snipCount == 1)
        {
            PlayRandomClip(collectionSnipFirstCrab, true);
        }
        else
        {
            PlayRandomClip(collectionSnipSecondCrab, true);

        }
    }

    void OnBattleEnd()
    {
        PlayRandomClip(collectionEnd, true);
    }

    void OnEndWave()
    {
        if (bigWaveClip)
        {
            MyAudioSource.PlayOneShot(bigWaveClip, 0.4f);
        }
    }

    protected override void Update()
    {
        base.Update();

        //clawDeflectCounter -= Time.deltaTime;
    }
}
