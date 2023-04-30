using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CrabBody : MonoBehaviour
{
    private static readonly Dictionary<Vector2Int, CrabDirection> Player1InputMapping = new()
    {
        [Vector2Int.up] = CrabDirection.Forward,
        [Vector2Int.right] = CrabDirection.Right,
        [Vector2Int.down] = CrabDirection.Backward,
        [Vector2Int.left] = CrabDirection.Left
    };

    private static readonly Dictionary<Vector2Int, CrabDirection> Player2InputMapping = new()
    {
        [Vector2Int.up] = CrabDirection.Forward,
        [Vector2Int.right] = CrabDirection.Right,
        [Vector2Int.down] = CrabDirection.Backward,
        [Vector2Int.left] = CrabDirection.Left
    };

    private static readonly Dictionary<CrabDirection, Vector2Int> Player1WorldMapping = new()
    {
        [CrabDirection.Forward] = Vector2Int.right,
        [CrabDirection.Right] = Vector2Int.down,
        [CrabDirection.Backward] = Vector2Int.left,
        [CrabDirection.Left] = Vector2Int.up
    };

    private static readonly Dictionary<CrabDirection, Vector2Int> Player2WorldMapping = new()
    {
        [CrabDirection.Forward] = Vector2Int.left,
        [CrabDirection.Right] = Vector2Int.up,
        [CrabDirection.Backward] = Vector2Int.right,
        [CrabDirection.Left] = Vector2Int.down
    };

    // Distances are in pixels
    public static float PIXELS_PER_UNIT = 23;
    int verticalDistance = 12;
    int pushedDistance = 12;
    int forwardHopDistance = 12;
    int backHopDistance = 12;

    float hopTime = 0.5f;

    [SerializeField]
    private Player player;

    [SerializeField]
    private CrabEye leftEye;

    [SerializeField]
    private CrabEye rightEye;

    public event Action Move;
    public event Action<CrabDirection> Hop;
    public event Action<CrabDirection> ChangeDirection;
    public event Action Out;
    public event Action In;
    public event Action Stunned;
    public event Action Died;
    public event Action LostFirstEye;

    private int numEyes = 2;

    private CrabDirection crabDirection = CrabDirection.Forward;
    private float hopTimer = 0;

    private Rigidbody2D rb2d;
    private CapsuleCollider2D cc2d;
    private Input.CrabInput input;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        cc2d = GetComponent<CapsuleCollider2D>();
        input = GetComponent<Input.CrabInput>();

        if (input.inputManager != null)
        {
            input.inputManager.Move += OnMove;
            input.inputManager.ChangeDirection += OnChangeDirection;
            input.inputManager.Out += OnOut;
            input.inputManager.In += OnIn;
        }
        if (input.playerInput != null)
        {
            input.playerInput.Move += OnMove;
            input.playerInput.ChangeDirection += OnChangeDirection;
            input.playerInput.Out += OnOut;
            input.playerInput.In += OnIn;
        }

        leftEye.Snipped += OnEyeSnipped;
        rightEye.Snipped += OnEyeSnipped;

        ChangeDirection?.Invoke(crabDirection);
    }

    private void Update()
    {
        if (hopTimer > 0)
        {
            hopTimer -= Time.deltaTime;
        }
    }

    void OnChangeDirection(Vector2Int direction)
    {
        if (numEyes <= 0)
        {
            return;
        }

        CrabDirection newDirection = InputToCrabDirection(direction);

        if (newDirection == CrabDirection.Left || newDirection == CrabDirection.Right)
        {
            // Set new movement direction
            crabDirection = newDirection;
            ChangeDirection?.Invoke(newDirection);
        }
        else if (hopTimer <= 0)
        {
            // Hops
            if (newDirection == CrabDirection.Forward)
            {
                MoveTo(CrabDirectionToWorldDirection(CrabDirection.Forward), forwardHopDistance);
                hopTimer = hopTime;
                Hop?.Invoke(newDirection);
            }
            else if (newDirection == CrabDirection.Backward)
            {
                MoveTo(CrabDirectionToWorldDirection(CrabDirection.Backward), backHopDistance);
                hopTimer = hopTime;
                Hop?.Invoke(newDirection);
            }
        }
    }

    void OnMove()
    {
        if (numEyes <= 0)
        {
            return;
        }

        Vector2Int direction = CrabDirectionToWorldDirection(crabDirection);

        if (direction.y > 0 || direction.y < 0)
        {
            MoveTo(direction, verticalDistance);

            Move?.Invoke();
        }
    }

    void OnOut()
    {
        crabDirection = CrabDirection.Forward;
        ChangeDirection?.Invoke(crabDirection);

        Out?.Invoke();
    }

    void OnIn()
    {
        crabDirection = CrabDirection.Forward;
        ChangeDirection?.Invoke(crabDirection);

        In?.Invoke();
    }

    public void OnPushed()
    {
        if (numEyes <= 0)
        {
            return;
        }

        bool isStunned = MoveTo(CrabDirectionToWorldDirection(CrabDirection.Backward), pushedDistance);

        if (isStunned)
        {
            Stunned?.Invoke();
        }
    }

    public void OnEyeSnipped()
    {
        numEyes -= 1;

        if (numEyes <= 0)
        {
            Died?.Invoke();
        }
        else
        {
            LostFirstEye?.Invoke();
        }
    }

    // Returns if we should be stunned
    bool MoveTo(Vector2Int direction, int distanceInPixels)
    {
        float distanceInUnits = distanceInPixels / PIXELS_PER_UNIT;

        string otherPlayer;
        if (player == Player.Player1) otherPlayer = "Player2 Body";
        else otherPlayer = "Player1 Body";

        LayerMask layerMask = LayerMask.GetMask("Water", otherPlayer);
        RaycastHit2D hit = Physics2D.CapsuleCast(rb2d.position, cc2d.size, cc2d.direction, transform.eulerAngles.z, direction, distanceInUnits, layerMask);
        if (hit.collider == null)
        {
            Vector2 vec2 = direction;
            rb2d.MovePosition(rb2d.position + vec2 * distanceInUnits);
        }
        else
        {
            if (hit.collider.gameObject.GetComponent<Barrier>())
            {
                return true;
            }
        }

        return false;
    }

    CrabDirection InputToCrabDirection(Vector2Int inputDirection)
    {
        if (player == Player.Player1)
        {
            return Player1InputMapping[inputDirection]; 
        }
        else
        {
            return Player2InputMapping[inputDirection];
        }
    }

    Vector2Int CrabDirectionToWorldDirection(CrabDirection crabDirection)
    {
        if (player == Player.Player1)
        {
            return Player1WorldMapping[crabDirection];
        }
        else
        {
            return Player2WorldMapping[crabDirection];
        }
    }
}
