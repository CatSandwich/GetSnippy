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

    public enum ClawState
    {
        Neutral,
        Blocking,
        Attacking
    }

    enum AttackingState
    {
        None,
        WindingUp,
        Lunging,
        Snipping,
        Recoiling
    }

    private static readonly Dictionary<AttackingState, float> stateTimes = new()
    {
        [AttackingState.None] = 0,
        [AttackingState.WindingUp] = 0.3f,
        [AttackingState.Lunging] = 0.1f,
        [AttackingState.Snipping] = 0.3f,
        [AttackingState.Recoiling] = 0.3f,
    };

    [SerializeField]
    private CrabBody crabBody;

    [SerializeField]
    private Input.CrabInput input;

    [SerializeField]
    private GameObject clawNeutral;

    [SerializeField]
    private GameObject clawBlock;

    [SerializeField]
    private GameObject clawBack;

    [SerializeField]
    private GameObject clawLunge;

    [SerializeField]
    private GameObject clawSnip;

    [SerializeField]
    private Player player;

    [SerializeField]
    private ClawSide clawSide;

    public event Action<ClawState> StateChanged;

    ClawState clawState = ClawState.Neutral;
    AttackingState attackingState = AttackingState.None;
    CrabDirection crabDirection;

    float attackTimer = 0.0f;

    bool clawAnimDirty = false;

    CrabBody GetBody()
    {
        return crabBody;
    }

    private void Awake()
    {
        crabBody.ChangeDirection += OnChangeDirection;
    }

    private void Start()
    {
        input.input.Lunge += OnAttack;
        input.input.Out += OnOut;
        input.input.In += OnIn;
    }

    private void Update()
    {
        if (clawState == ClawState.Attacking)
        {
            if (attackTimer <= 0)
            {
                if (attackingState == AttackingState.WindingUp)
                {
                    ChangeAttackingState(AttackingState.Lunging);
                }
                else if (attackingState == AttackingState.Lunging)
                {
                    ChangeAttackingState(AttackingState.Snipping);
                }
                else if (attackingState == AttackingState.Snipping || attackingState == AttackingState.Recoiling)
                {
                    ChangeClawState(ClawState.Neutral);
                    OnChangeDirection(crabDirection);
                }
            }
            else
            {
                attackTimer -= Time.deltaTime;
            }
        }    

        UpdateClaw();
    }

    // Set claw state based on CrabDirection
    void OnChangeDirection(CrabDirection direction)
    {
        crabDirection = direction;

        if (clawState != ClawState.Attacking)
        {
            if (direction == CrabDirection.Forward || direction == CrabDirection.Backward)
            {
                ChangeClawState(ClawState.Neutral);
            }
            else if (direction == CrabDirection.Left)
            {
                if (clawSide == ClawSide.Right) ChangeClawState(ClawState.Blocking);
                else ChangeClawState(ClawState.Neutral);
            }
            else if (direction == CrabDirection.Right)
            {
                if (clawSide == ClawSide.Left) ChangeClawState(ClawState.Blocking);
                else ChangeClawState(ClawState.Neutral);
            }
        }
    }

    void OnOut()
    {
        ChangeClawState(ClawState.Neutral);
    }

    void OnIn()
    {
        ChangeClawState(ClawState.Blocking);
    }

    void OnAttack()
    {
        if (clawState == ClawState.Neutral)
        {
            ChangeClawState(ClawState.Attacking);
            ChangeAttackingState(AttackingState.WindingUp);
        }
    }

    void ChangeClawState(ClawState newState)
    {
        clawState = newState;

        if (clawState != ClawState.Attacking)
        {
            ChangeAttackingState(AttackingState.None);
        }

        clawAnimDirty = true;

        StateChanged?.Invoke(clawState);
    }

    void ChangeAttackingState(AttackingState newState)
    {
        attackingState = newState;

        attackTimer = stateTimes[newState];

        clawAnimDirty = true;
    }

    // Update which claw frame is active based on clawState and attackingState.
    private void UpdateClaw()
    {
        if (!clawAnimDirty)
        {
            return;
        }

        clawAnimDirty = false;

        clawNeutral.SetActive(false);
        clawBlock.SetActive(false);
        clawBack.SetActive(false);
        clawLunge.SetActive(false);
        clawSnip.SetActive(false);

        if (clawState == ClawState.Neutral)
        {
            clawNeutral.SetActive(true);
        }
        else if (clawState == ClawState.Blocking)
        {
            clawBlock.SetActive(true);
        }
        else if (clawState == ClawState.Attacking)
        {
            if (attackingState == AttackingState.WindingUp || attackingState == AttackingState.Recoiling)
            {
                clawBack.SetActive(true);
            }
            else if (attackingState == AttackingState.Lunging)
            {
                clawLunge.SetActive(true);
            }
            else if (attackingState == AttackingState.Snipping)
            {
                clawSnip.SetActive(true);
            }
        }
    }

    // If we are attacking and hit a claw, recoil.
    // Else, if we hit an eye and we are snipping, kill that eye.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (clawState == ClawState.Attacking)
        {
            CrabClaw hitClaw = collision.gameObject.GetComponent<CrabClaw>();

            if (hitClaw)
            {
                hitClaw.crabBody.OnPushed();
                ChangeAttackingState(AttackingState.Recoiling);
            }
            else
            {
                CrabEye hitEye = collision.gameObject.GetComponent<CrabEye>();

                if (hitEye != null && hitEye.GetPlayer() != player)
                {
                    if (attackingState == AttackingState.Snipping)
                    {
                        hitEye.Die();
                    }
                }
            }
        }
    }
}
