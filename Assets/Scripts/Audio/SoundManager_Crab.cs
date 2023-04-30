using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using static CrabClaw;

public class SoundManager_Crab : SoundManager_Base
{
    public RandomSoundCollection collectionAttack;
    public RandomSoundCollection collectionDeflect;
    public RandomSoundCollection collectionLeap;
    public RandomSoundCollection collectionScuttle;
    public RandomSoundCollection collectionSnip;
    public AudioClip clawBlock;
    public AudioClip clawNeutral;

    public CrabBody myBody;
    public CrabClaw myClawLeft;
    public CrabClaw myClawRight;

    private const float clawSoundDelay = 0.2f;
    private float clawAttackCounter = 0.0f;
    private float clawDeflectCounter = 0.0f;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        myBody.Move += OnMove;
        myBody.Hop += OnHop;

        myClawLeft.StateChanged += OnClawStateChanged;
        myClawLeft.Blocked += OnBlocked;
        myClawLeft.Snipped += OnSnip;

        myClawRight.StateChanged += OnClawStateChanged;
        myClawRight.Blocked += OnBlocked;
        myClawRight.Snipped += OnSnip;
    }

    void OnMove()
    {
        PlayRandomClip(collectionScuttle, false);
    }

    void OnHop(CrabDirection direction)
    {
        PlayRandomClip(collectionLeap, false);
    }

    void OnClawStateChanged(ClawState newState)
    {
        if (newState == ClawState.Attacking && clawAttackCounter < 0.0f)
        {
            PlayRandomClip(collectionAttack, false);
            clawAttackCounter = clawSoundDelay;
        }
        else if (newState == ClawState.Blocking)
        {
            MyAudioSource.PlayOneShot(clawBlock, 0.3f);
        } 
        else if (newState == ClawState.Neutral || newState == ClawState.Stunned)
        {
            MyAudioSource.PlayOneShot(clawNeutral, 0.3f);
        }
    }

    void OnBlocked()
    {
        if (clawDeflectCounter < 0.0f)
        {
            PlayRandomClip(collectionDeflect, false);
            clawDeflectCounter = clawSoundDelay;
        }
    }

    void OnSnip()
    {
        PlayRandomClip(collectionSnip, false);
    }

    protected override void Update()
    {
        base.Update();

        clawAttackCounter -= Time.deltaTime;
        clawDeflectCounter -= Time.deltaTime;
    }
}
