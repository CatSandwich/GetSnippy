using System;
using System.Collections.Generic;
using UnityEngine;

public class CrabBody : MonoBehaviour
{
    // Crabs are faster side to side
    const float VERTICAL_SPEED = 0.5f;
    const float HORIZONTAL_SPEED = 0.25f;

    [SerializeField]
    Input.InputManager inputManager;

    public event Action<Vector2> DirectionChanged;
    Vector2 current_direction = Vector2.right;

    // Start is called before the first frame update
    void Start()
    {
        inputManager.Move += OnMove;
        inputManager.DirectionChanged += OnDirectionChanged;
        inputManager.Out += OnOut;
        inputManager.In += OnIn;

        DirectionChanged?.Invoke(current_direction);
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
        DirectionChanged?.Invoke(direction);
    }

    void OnOut()
    {
        Debug.Log("OnOut");
    }
    void OnIn()
    {
        Debug.Log("OnIn");
    }
}
