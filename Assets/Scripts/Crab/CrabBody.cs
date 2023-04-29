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
    float horizontalSpeed = 0.25f;

    [SerializeField]
    float pushedSpeed = 0.25f;

    [SerializeField]
    private Player player;

    public event Action Move;
    public event Action<CrabDirection> DirectionChanged;

    public int numEyes = 2;

    private CrabDirection crabDirection = CrabDirection.Forward;

    private Rigidbody2D rb2d;
    private CapsuleCollider2D cc2d;
    private Input.CrabInput input;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        cc2d = GetComponent<CapsuleCollider2D>();
        input = GetComponent<Input.CrabInput>();

        input.input.Move += OnMove;
        input.input.ChangeDirection += OnDirectionChanged;

        DirectionChanged?.Invoke(crabDirection);
    }

    void OnDirectionChanged(Vector2Int direction)
    {
        if (numEyes <= 0)
        {
            return;
        }

        crabDirection = ToCrabDirection(direction);
        DirectionChanged?.Invoke(crabDirection);
    }

    void OnMove()
    {
        if (numEyes <= 0)
        {
            return;
        }

        Vector2Int direction = ToVector2Int(crabDirection);
        float speed;

        if (direction.y > 0 || direction.y < 0)
        {
            speed = verticalSpeed;
        }
        else if (direction.x > 0 || direction.x < 0)
        {
            speed = horizontalSpeed;
        }
        else
        {
            speed = 0;
        }

        MoveTo(direction, speed);

        Move?.Invoke();
    }

    public void OnPushed()
    {
        MoveTo(ToVector2Int(CrabDirection.Backward), pushedSpeed);
    }

    void MoveTo(Vector2Int direction, float speed)
    {
        string otherPlayer;
        if (player == Player.Player1) otherPlayer = "Player2";
        else otherPlayer = "Player1";

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
