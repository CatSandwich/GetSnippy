using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class CrabBody : MonoBehaviour
{
    // Crabs are faster side to side
    const float VERTICAL_SPEED = 0.5f;
    const float HORIZONTAL_SPEED = 0.25f;

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

    [SerializeField]
    private Input.InputManager inputManager;

    [SerializeField]
    private Player player;

    public event Action Move;

    public event Action<CrabDirection> DirectionChanged;
    private CrabDirection crabDirection = CrabDirection.Forward;

    // Start is called before the first frame update
    void Start()
    {
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

        if (direction.y > 0 || direction.y < 0)
        {
            transform.Translate(direction * VERTICAL_SPEED);
        }
        else if (direction.x > 0 || direction.x < 0)
        {
            transform.Translate(direction * HORIZONTAL_SPEED);
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
