using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabClaw : MonoBehaviour
{
    enum ClawSide
    {
        Left,
        Right
    }

    enum ClawState
    {
        Neutral,
        Blocking,
        WindingUp,
        Lunging,
        Snipping,
        Recoiling
    }

    private static readonly Dictionary<ClawState, float> stateTimes = new()
    {
        [ClawState.Neutral] = 0,
        [ClawState.Blocking] = 0,
        [ClawState.WindingUp] = 0.2f,
        [ClawState.Lunging] = 0.2f,
        [ClawState.Snipping] = 0.2f,
        [ClawState.Recoiling] = 0.2f,
    };

    [SerializeField]
    private CrabBody crabBody;

    [SerializeField]
    private Player player;

    [SerializeField]
    private ClawSide clawSide;

    ClawState state = ClawState.Neutral;
    CrabDirection crabDirection;

    float timer = 0.0f;

    private void Awake()
    {
        crabBody.DirectionChanged += OnDirectionChanged;
    }

    private void Update()
    {
        if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
        {
            OnAttack();
        }

        if (timer <= 0)
        {
            if (state == ClawState.WindingUp)
            {
                ChangeState(ClawState.Lunging);
            }
            else if (state == ClawState.Lunging)
            {
                ChangeState(ClawState.Snipping);
            }
            else if (state == ClawState.Snipping || state == ClawState.Recoiling)
            {
                ChangeState(ClawState.Neutral);
                OnDirectionChanged(crabDirection);
            }
        }
        else
        {
            timer -= Time.deltaTime;
        }

        UpdateClaw();
    }

    // Set claw state based on CrabDirection
    void OnDirectionChanged(CrabDirection direction)
    {
        crabDirection = direction;

        if (state == ClawState.Neutral || state == ClawState.Blocking)
        {
            if (direction == CrabDirection.Forward)
            {
                ChangeState(ClawState.Neutral);
            }
            else if (direction == CrabDirection.Backward)
            {
                ChangeState(ClawState.Blocking);
            }
            else if (direction == CrabDirection.Left)
            {
                if (clawSide == ClawSide.Left) ChangeState(ClawState.Blocking);
                else ChangeState(ClawState.Neutral);
            }
            else if (direction == CrabDirection.Right)
            {
                if (clawSide == ClawSide.Left) ChangeState(ClawState.Neutral);
                else ChangeState(ClawState.Blocking);
            }
        }
    }

    void OnAttack()
    {
        if (state == ClawState.Neutral)
        {
            ChangeState(ClawState.WindingUp);
        }
    }

    void ChangeState(ClawState newState)
    {
        state = newState;

        timer = stateTimes[newState];
    }

    // Update claw position and rotation based on state and side.
    private void UpdateClaw()
    {
        bool defaultRotation = true;

        if (state == ClawState.Neutral)
        {
            float yPos = clawSide == ClawSide.Left ? 0.6f : -0.6f;

            transform.localPosition = new Vector3(0.6f, yPos, 0);
            transform.localEulerAngles = new Vector3(0, 0, 270);
        }
        else if (state == ClawState.Blocking)
        {
            defaultRotation = false;

            if (clawSide == ClawSide.Left)
            {
                transform.localPosition = new Vector3(0.9f, 0.4f, 0);
                transform.localEulerAngles = new Vector3(0, 0, 200);
            }
            else
            {
                transform.localPosition = new Vector3(0.9f, -0.4f, 0);
                transform.localEulerAngles = new Vector3(0, 0, 340);
            }
        }
        else if (state == ClawState.WindingUp)
        {
            if (clawSide == ClawSide.Left)
            {
                transform.localPosition = new Vector3(0.5f, 0.6f, 0);
            }
            else
            {
                transform.localPosition = new Vector3(0.5f, -0.6f, 0);
            }
        }
        else if (state == ClawState.Lunging)
        {
            if (clawSide == ClawSide.Left)
            {
                transform.localPosition = new Vector3(0.9f, 0.6f, 0);
            }
            else
            {
                transform.localPosition = new Vector3(0.9f, -0.6f, 0);
            }
        }
        else if (state == ClawState.Snipping)
        {
            if (clawSide == ClawSide.Left)
            {
                transform.localPosition = new Vector3(1.2f, 0.6f, 0);
            }
            else
            {
                transform.localPosition = new Vector3(1.2f, -0.6f, 0);
            }
        }
        else if (state == ClawState.Recoiling)
        {
            if (clawSide == ClawSide.Left)
            {
                transform.localPosition = new Vector3(0.5f, 0.6f, 0);
            }
            else
            {
                transform.localPosition = new Vector3(0.5f, -0.6f, 0);
            }
        }

        if (defaultRotation)
        {
            transform.localEulerAngles = new Vector3(0, 0, 270);
        }
    }

    // If we are attacking and hit a claw, recoil.
    // Else, if we hit an eye and we are snipping, kill that eye.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        CrabClaw hitClaw = collision.gameObject.GetComponent<CrabClaw>();
        
        if (state != ClawState.Neutral && state != ClawState.Blocking && hitClaw != null)
        {
            ChangeState(ClawState.Recoiling);
        }
        else
        {
            CrabEye hitEye = collision.gameObject.GetComponent<CrabEye>();

            if (hitEye != null && hitEye.GetPlayer() != player)
            {
                if (state == ClawState.Snipping)
                {
                    hitEye.Die();
                }
            }
        }
    }
}
