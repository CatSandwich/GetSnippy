using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionIndicator : MonoBehaviour
{
    [SerializeField]
    CrabBody crabBody;

    private static readonly Dictionary<Vector2, Vector3> DirectionRotations = new()
    {
        [Vector2.up] = new Vector3(0, 0, 180),
        [Vector2.right] = new Vector3(0, 0, 90),
        [Vector2.down] = new Vector3(0, 0, 0),
        [Vector2.left] = new Vector3(0, 0, 270)
    };

    private void Awake()
    {
        crabBody.DirectionChanged += OnDirectionChanged;
    }

    void OnDirectionChanged(Vector2 direction)
    {
        transform.eulerAngles = DirectionRotations[direction];
    }
}
