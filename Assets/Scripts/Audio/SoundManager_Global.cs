using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using static CrabClaw;

public class SoundManager_Global : SoundManager_Base
{
    public RandomSoundCollection collectionStart;
    public RandomSoundCollection collectionSnip;
    public RandomSoundCollection collectionEnd;

    public GameManager myGameManager;

    //private const float clawSoundDelay = 0.2f;
    //private float clawAttackCounter = 0.0f;
    //private float clawDeflectCounter = 0.0f;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        //myGameManager.OnStart += OnBattleStart;
    }

    void OnBattleStart()
    {
        PlayRandomClip(collectionStart, true);
    }

    void OnSnip()
    {
        PlayRandomClip(collectionSnip, true);
    }

    void OnBattleEnd()
    {
        PlayRandomClip(collectionEnd, true);
    }

    protected override void Update()
    {
        base.Update();

        if (UnityEngine.Input.GetKeyDown(KeyCode.Q))
        {
            OnBattleStart();
        }

        //clawDeflectCounter -= Time.deltaTime;
    }
}
