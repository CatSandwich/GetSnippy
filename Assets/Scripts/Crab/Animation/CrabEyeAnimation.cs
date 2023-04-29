using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabEyeAnimation : MonoBehaviour
{
    enum EyeAnimState
    {
        Neutral,
        Blocking,
        Attacking,
        Dead
    }

    [SerializeField]
    private float bobTime = 0.5f;

    [SerializeField]
    private CrabClaw crabClaw;

    [SerializeField]
    private Sprite neutralSprite;

    [SerializeField]
    private Sprite neutralBobSprite;

    [SerializeField]
    private Sprite guardSprite;

    [SerializeField]
    private Sprite guardBobSprite;

    [SerializeField]
    private Sprite lungeSprite;

    [SerializeField]
    private Sprite deadSprite;

    [SerializeField]
    private Sprite deadBobSprite;

    private CrabEye crabEye;
    private SpriteRenderer spriteRenderer;

    private EyeAnimState state = EyeAnimState.Neutral;
    private float bobTimer = 0;
    private bool isBob = false;

    private void Awake()
    {
        bobTimer = bobTime;

        crabEye = GetComponent<CrabEye>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        crabEye.Died += OnEyeDied;
        crabClaw.StateChanged += OnClawStateChanged;
    }

    private void Update()
    {
        if (bobTimer <= 0)
        {
            isBob = !isBob;

            UpdateEyeSprite();
            bobTimer += bobTime;
        }
        else
        {
            bobTimer -= Time.deltaTime;
        }
    }

    void OnEyeDied()
    {
        state = EyeAnimState.Dead;
    }

    void OnClawStateChanged(CrabClaw.ClawState clawState)
    {
        if (state != EyeAnimState.Dead)
        {
            if (clawState == CrabClaw.ClawState.Neutral)
            {
                state = EyeAnimState.Neutral;
            }
            else if (clawState == CrabClaw.ClawState.Blocking)
            {
                state = EyeAnimState.Blocking;
            }
            else if (clawState == CrabClaw.ClawState.Attacking)
            {
                state = EyeAnimState.Attacking;
            }
        }

        UpdateEyeSprite();
    }

    void UpdateEyeSprite()
    {
        if (state == EyeAnimState.Neutral)
        {
            if (isBob)
            {
                spriteRenderer.sprite = neutralBobSprite;
            }
            else
            {
                spriteRenderer.sprite = neutralSprite;
            }
        }
        else if (state == EyeAnimState.Blocking)
        {
            if (isBob)
            {
                spriteRenderer.sprite = guardBobSprite;
            }
            else
            {
                spriteRenderer.sprite = guardSprite;
            }                
        }
        else if (state == EyeAnimState.Attacking)
        {
            spriteRenderer.sprite = lungeSprite;
        }
        else if (state == EyeAnimState.Dead)
        {
            if (isBob)
            {
                spriteRenderer.sprite = deadBobSprite;
            }
            else
            {
                spriteRenderer.sprite = deadSprite;
            }
        }
    }
}
