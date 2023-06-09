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
        Attacking,
        Stunned
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
        [AttackingState.Lunging] = 0.05f,
        [AttackingState.Snipping] = 0.3f,
        [AttackingState.Recoiling] = 0.3f,
    };

    private float stunnedTime = 1;

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
    private GameObject clawSnipPoint;

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

    float attackTimer = 0.0f;
    float stunnedTimer = 0.0f;

    bool clawAnimDirty = false;
    bool isFullyDead = false;

    public bool IsBlocking()
    {
        return clawState == ClawState.Blocking;
    }

    private void Start()
    {
        if (input.inputManager != null)
        {
            if (clawSide == ClawSide.Left)
            {
                input.inputManager.LungeLeft += OnAttack;
            }
            else
            {
                input.inputManager.LungeRight += OnAttack;
            }

            input.inputManager.ChangeClawPose += OnChangeClawPose;
        }

        if (input.playerInput != null)
        {
            if (clawSide == ClawSide.Left)
            {
                input.playerInput.LungeLeft += OnAttack;
            }
            else
            {
                input.playerInput.LungeRight += OnAttack;
            }

            input.playerInput.ChangeClawPose += OnChangeClawPose;
        }

        if (input.gamepadInput != null)
        {
            if (clawSide == ClawSide.Left)
            {
                input.gamepadInput.LungeLeft += OnAttack;
            }
            else
            {
                input.gamepadInput.LungeRight += OnAttack;
            }

            input.gamepadInput.ChangeClawPose += OnChangeClawPose;
        }

        crabBody.Stunned += OnStunned;
        crabBody.Died += OnDead;
    }

    private void Update()
    {
        if (isFullyDead)
        {
            return;
        }

        if (clawState == ClawState.Stunned)
        {
            if (stunnedTimer <= 0)
            {
                clawState = ClawState.Neutral;
                ResetClaw();
            }
            else
            {
                stunnedTimer -= Time.deltaTime;
            }
        }

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
                    ResetClaw();
                }
            }
            else
            {
                attackTimer -= Time.deltaTime;
            }
        }    

        UpdateClaw();
    }

    void ResetClaw()
    {
        ChangeClawState(ClawState.Neutral);
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

        // See if we have any claw or eye contact
        CrabClaw clawContact = null;
        List<CrabEye> eyeContact = new List<CrabEye>();

        foreach (Collider2D collider in colliders)
        {
            CrabClaw hitClaw = collider.gameObject.GetComponentInParent<CrabClaw>();
            CrabEye hitEye = collider.gameObject.GetComponent<CrabEye>();

            if ((hitClaw && hitClaw.isActiveAndEnabled) || (hitEye && hitEye.IsBlocked()))
            {
                clawContact = hitClaw;
                break;
            }
            else if (hitEye && !hitEye.IsDead())
            {
                eyeContact.Add(hitEye);
            }
        }

        // Resolve our contacts
        if (clawContact)
        {
            clawContact.crabBody.OnPushed();
            ChangeAttackingState(AttackingState.Recoiling);
            Blocked?.Invoke();
        }
        else if (eyeContact.Count > 0)
        {
            int closestEyeIndex = 0;
            float closestEyeDistance = 1000;

            for (int i = 0; i < eyeContact.Count; ++i)
            {
                float distance = Vector3.Distance(eyeContact[i].transform.position, clawSnipPoint.transform.position);

                if (distance < closestEyeDistance)
                {
                    closestEyeIndex = i;
                    closestEyeDistance = distance;
                }
            }

            eyeContact[closestEyeIndex].GetSnipped();
            Snipped?.Invoke();
        }
        else
        {
            Miss?.Invoke();
        }
    }

    void OnChangeClawPose(CrabClawPose newPose)
    {
        if (isFullyDead || clawState == ClawState.Attacking)
        {
            return;
        }

        if (newPose == CrabClawPose.Open)
        {
            ChangeClawState(ClawState.Neutral);
        }
        else if (newPose == CrabClawPose.Closed)
        {
            ChangeClawState(ClawState.Blocking);
        }
        else if (newPose == CrabClawPose.Left  && clawSide == ClawSide.Left ||
                 newPose == CrabClawPose.Right && clawSide == ClawSide.Right)
        {
            ChangeClawState(ClawState.Neutral);
        }
        else if (newPose == CrabClawPose.Left  && clawSide == ClawSide.Right ||
                 newPose == CrabClawPose.Right && clawSide == ClawSide.Left)
        {
            ChangeClawState(ClawState.Blocking);
        }
    }

    void OnAttack()
    {
        if (isFullyDead)
        {
            return;
        }

        if (clawState == ClawState.Neutral)
        {
            ChangeClawState(ClawState.Attacking);
            ChangeAttackingState(AttackingState.WindingUp);
        }
    }

    void OnStunned()
    {
        ChangeClawState(ClawState.Stunned);
    }

    void OnDead()
    {
        ChangeClawState(ClawState.Neutral);
        UpdateClaw();

        isFullyDead = true;
    }

    void ChangeClawState(ClawState newState)
    {
        if (clawState != ClawState.Stunned)
        {
            if (clawState != newState)
            {
                StateChanged?.Invoke(newState);
            }
            clawState = newState;

            if (clawState != ClawState.Attacking)
            {
                ChangeAttackingState(AttackingState.None);
            }

            if (clawState == ClawState.Stunned)
            {
                stunnedTimer = stunnedTime;
            }

            clawAnimDirty = true;
        }
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
        else if (clawState == ClawState.Stunned)
        {
            clawBack.SetActive(true);
        }
    }
}
