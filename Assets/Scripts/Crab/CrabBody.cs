using System;
using System.Collections.Generic;
using UnityEngine;

public class CrabBody : MonoBehaviour
{
    // Crabs are faster side to side
    const float VERTICAL_SPEED = 0.5f;
    const float HORIZONTAL_SPEED = 0.25f;

    [SerializeField]
    private Input.InputManager inputManager;

    [SerializeField]
    private Player player;

    public event Action<CrabDirection> DirectionChanged;
    private Vector2 current_direction = Vector2.right;

    // Start is called before the first frame update
    void Start()
    {
        inputManager.Move += OnMove;
        inputManager.DirectionChanged += OnDirectionChanged;

        DirectionChanged?.Invoke(ToCrabDirection(current_direction));
    }

    void OnMove() 
    {
        if (current_direction.y > 0 || current_direction.y < 0)
        {
            transform.Translate(current_direction * VERTICAL_SPEED);
        }
        else if (current_direction.x > 0 || current_direction.x < 0)
        {
            transform.Translate(current_direction * HORIZONTAL_SPEED);
        }
    }

    void OnDirectionChanged(Vector2 direction)
    {
        current_direction = direction;
        DirectionChanged?.Invoke(ToCrabDirection(direction));
    }

    CrabDirection ToCrabDirection(Vector2 direction)
    {
        if (player == Player.Player1)
        {
            if (direction == Vector2.right) return CrabDirection.Forward;
            else if (direction == Vector2.left) return CrabDirection.Backward;
            else if (direction == Vector2.up) return CrabDirection.Left;
            else if (direction == Vector2.down) return CrabDirection.Right;
        }
        else
        {
            if (direction == Vector2.right) return CrabDirection.Backward;
            else if (direction == Vector2.left) return CrabDirection.Forward;
            else if (direction == Vector2.up) return CrabDirection.Right;
            else if (direction == Vector2.down) return CrabDirection.Left;
        }

        return CrabDirection.Forward;
    }
}
