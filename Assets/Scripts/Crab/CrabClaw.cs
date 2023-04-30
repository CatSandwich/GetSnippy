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
    public event Action Blocked;
    public event Action Snipped;
    public event Action Miss;

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
        if (input.inputManager != null)
        {

            input.inputManager.Lunge += OnAttack;
            input.inputManager.Out += OnOut;
            input.inputManager.In += OnIn;
        }

        input.playerInput.Lunge += OnAttack;
        input.playerInput.Out += OnOut;
        input.playerInput.In += OnIn;
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
                    Snip();
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

    // Call this when we enter Snipping attackState
    // If we hit a claw, recoil.
    // Else, if we hit an eye, kill that eye.
    void Snip()
    {
        UpdateClaw();

        Collider2D snipCollider = clawSnip.GetComponent<Collider2D>();
        List<Collider2D> colliders = new List<Collider2D>();

        // Make contact filter
        ContactFilter2D contactFilter = new ContactFilter2D();
        string otherPlayer;
        if (player == Player.Player1) otherPlayer = "Player2 Parts";
        else otherPlayer = "Player1 Parts";
        contactFilter.SetLayerMask(LayerMask.GetMask(otherPlayer));

        snipCollider.OverlapCollider(contactFilter, colliders);

        // See if we have any claw or eye contact (max of 1 at a time)
        CrabClaw clawContact = null;
        CrabEye eyeContact = null;

        foreach (Collider2D collider in colliders)
        {
            Debug.Log(collider.gameObject);

            CrabClaw hitClaw = collider.gameObject.GetComponentInParent<CrabClaw>();
            CrabEye hitEye = collider.gameObject.GetComponent<CrabEye>();

            if (hitClaw && hitClaw.isActiveAndEnabled)
            {
                clawContact = hitClaw;
                break;
            }
            else if (hitEye && !hitEye.IsDead())
            {
                eyeContact = hitEye;
            }
        }

        // Resolve our contacts
        if (clawContact)
        {
            clawContact.crabBody.OnPushed();
            ChangeAttackingState(AttackingState.Recoiling);
            Blocked?.Invoke();
        }
        else if (eyeContact)
        {
            eyeContact.GetSnipped();
            Snipped?.Invoke();
        }
        else
        {
            Miss?.Invoke();
        }
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
}
