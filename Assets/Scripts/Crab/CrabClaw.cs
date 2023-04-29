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
        Snipping,
        Recoiling
    }

    [SerializeField]
    private CrabBody crabBody;

    [SerializeField]
    private Player player;

    [SerializeField]
    private ClawSide clawSide;

    ClawState state = ClawState.Neutral;

    private void Awake()
    {
        crabBody.DirectionChanged += OnDirectionChanged;
    }

    // Set claw state based on CrabDirection
    void OnDirectionChanged(CrabDirection direction)
    {
        if (direction == CrabDirection.Forward)
        {
            state = ClawState.Neutral;
        }
        else if (direction == CrabDirection.Backward)
        {
            state = ClawState.Blocking;
        }
        else if (direction == CrabDirection.Left)
        {
            if (clawSide == ClawSide.Left) state = ClawState.Blocking;
            else state = ClawState.Neutral;
        }
        else if (direction == CrabDirection.Right)
        {
            if (clawSide == ClawSide.Left) state = ClawState.Neutral;
            else state = ClawState.Blocking;
        }

        UpdateClaw();
    }

    // Update claw position and rotation based on state and side.
    private void UpdateClaw()
    {
        if (state == ClawState.Neutral)
        {
            float yPos = clawSide == ClawSide.Left ? 0.6f : -0.6f;

            transform.localPosition = new Vector3(0.6f, yPos, 0);
            transform.localEulerAngles = new Vector3(0, 0, 270);
        }
        else if (state == ClawState.Blocking)
        {
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
    }

    // If we hit a claw, recoil.
    // Else, if we hit an eye and we are snipping, kill that eye.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        CrabClaw hitClaw = collision.gameObject.GetComponent<CrabClaw>();
        
        if (hitClaw != null)
        {
            state = ClawState.Recoiling;
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
