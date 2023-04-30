using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CrabBody : MonoBehaviour
{
    private static readonly Dictionary<Vector2Int, CrabDirection> Player1DirectionMapping = new()
    {
        [Vector2Int.up] = CrabDirection.Left,
        [Vector2Int.right] = CrabDirection.Forward,
        [Vector2Int.down] = CrabDirection.Right,
        [Vector2Int.left] = CrabDirection.Backward
    };

    private static readonly Dictionary<Vector2Int, CrabDirection> Player2DirectionMapping = new()
    {
        [Vector2Int.up] = CrabDirection.Right,
        [Vector2Int.right] = CrabDirection.Backward,
        [Vector2Int.down] = CrabDirection.Left,
        [Vector2Int.left] = CrabDirection.Forward
    };

    // Crabs are faster side to side
    [SerializeField]
    float verticalSpeed = 0.5f;

    [SerializeField]
    float pushedSpeed = 0.25f;

    [SerializeField]
    float forwardHopDistance= 0.25f;

    [SerializeField]
    float backHopDistance = 0.5f;

    [SerializeField]
    float hopTime = 0.5f;

    [SerializeField]
    private Player player;

    [SerializeField]
    private CrabEye leftEye;

    [SerializeField]
    private CrabEye rightEye;

    public event Action Move;
    public event Action Hop;
    public event Action<CrabDirection> ChangeDirection;

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
        }
        input.playerInput.Move += OnMove;
        input.playerInput.ChangeDirection += OnChangeDirection;

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

        crabDirection = ToCrabDirection(direction);

        if (hopTimer <= 0)
        {
            if (crabDirection == CrabDirection.Forward)
            {
                // Hop forward
                MoveTo(ToVector2Int(CrabDirection.Forward), forwardHopDistance);
                hopTimer = hopTime;
                Hop?.Invoke();
            }
            else if (crabDirection == CrabDirection.Backward)
            {
                // Hop Backward
                MoveTo(ToVector2Int(CrabDirection.Backward), backHopDistance);
                hopTimer = hopTime;
                Hop?.Invoke();
            }
        }

        ChangeDirection?.Invoke(crabDirection);
    }

    void OnMove()
    {
        if (numEyes <= 0)
        {
            return;
        }

        Vector2Int direction = ToVector2Int(crabDirection);

        if (direction.y > 0 || direction.y < 0)
        {
            MoveTo(direction, verticalSpeed);

            Move?.Invoke();
        }
    }

    public void OnPushed()
    {
        MoveTo(ToVector2Int(CrabDirection.Backward), pushedSpeed);
    }

    public void OnEyeSnipped()
    {
        numEyes -= 1;
    }

    void MoveTo(Vector2Int direction, float speed)
    {
        string otherPlayer;
        if (player == Player.Player1) otherPlayer = "Player2 Body";
        else otherPlayer = "Player1 Body";

        LayerMask layerMask = LayerMask.GetMask("Water", otherPlayer);
        RaycastHit2D hit = Physics2D.CapsuleCast(rb2d.position, cc2d.size, cc2d.direction, transform.eulerAngles.z, direction, speed, layerMask);
        if (hit.collider == null)
        {
            Vector2 vec2 = direction;
            rb2d.MovePosition(rb2d.position + vec2 * speed);
        }
    }

    CrabDirection ToCrabDirection(Vector2Int direction)
    {
        if (player == Player.Player1)
        {
            return Player1DirectionMapping[direction]; 
        }
        else
        {
            return Player2DirectionMapping[direction];
        }
    }

    Vector2Int ToVector2Int(CrabDirection direction)
    {
        if (player == Player.Player1)
        {
            return Player1DirectionMapping.FirstOrDefault(x => x.Value == direction).Key;
        }
        else
        {
            return Player2DirectionMapping.FirstOrDefault(x => x.Value == direction).Key;
        }
    }
}
