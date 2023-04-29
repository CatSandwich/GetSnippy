using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class CrabBody : MonoBehaviour
{
    private static readonly Dictionary<Vector2, CrabDirection> Player1DirectionMapping = new()
    {
        [Vector2.up] = CrabDirection.Left,
        [Vector2.right] = CrabDirection.Forward,
        [Vector2.down] = CrabDirection.Right,
        [Vector2.left] = CrabDirection.Backward
    };

    private static readonly Dictionary<Vector2, CrabDirection> Player2DirectionMapping = new()
    {
        [Vector2.up] = CrabDirection.Right,
        [Vector2.right] = CrabDirection.Backward,
        [Vector2.down] = CrabDirection.Left,
        [Vector2.left] = CrabDirection.Forward
    };

    // Crabs are faster side to side
    [SerializeField]
    float verticalSpeed = 0.5f;

    [SerializeField]
    float horizontalSpeed = 0.25f;

    [SerializeField]
    private Input.InputManager inputManager;

    [SerializeField]
    private Player player;

    public event Action Move;

    public event Action<CrabDirection> DirectionChanged;

    private CrabDirection crabDirection = CrabDirection.Forward;

    private Rigidbody2D rb2d;
    private CapsuleCollider2D cc2d;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        cc2d = GetComponent<CapsuleCollider2D>();

        inputManager.Move += OnMove;
        inputManager.DirectionChanged += OnDirectionChanged;

        DirectionChanged?.Invoke(crabDirection);
    }

    void OnDirectionChanged(Vector2 direction)
    {
        crabDirection = ToCrabDirection(direction);
        DirectionChanged?.Invoke(crabDirection);
    }

    void OnMove()
    {
        Vector2 direction = ToVector2(crabDirection);
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

        String otherPlayer;
        if (player == Player.Player1) otherPlayer = "Player2";
        else otherPlayer = "Player1";

        LayerMask layerMask = LayerMask.GetMask("Water", otherPlayer);
        RaycastHit2D hit = Physics2D.CapsuleCast(rb2d.position, cc2d.size, cc2d.direction, transform.eulerAngles.z, direction, speed, layerMask);
        if (hit.collider == null)
        {
            rb2d.MovePosition(rb2d.position + direction * speed);
        }

        Move?.Invoke();
    }


    CrabDirection ToCrabDirection(Vector2 direction)
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

    Vector2 ToVector2(CrabDirection direction)
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
