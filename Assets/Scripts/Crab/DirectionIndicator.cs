using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionIndicator : MonoBehaviour
{
    [SerializeField]
    private CrabBody crabBody;

    private static readonly Dictionary<CrabDirection, Vector3> DirectionRotations = new()
    {
        [CrabDirection.Left] = new Vector3(0, 0, 180),
        [CrabDirection.Forward] = new Vector3(0, 0, 90),
        [CrabDirection.Right] = new Vector3(0, 0, 0),
        [CrabDirection.Backward] = new Vector3(0, 0, 270)
    };

    private void Awake()
    {
        crabBody.ChangeDirection += OnChangeDirection;
    }

    void OnChangeDirection(CrabDirection direction)
    {
        transform.localEulerAngles = DirectionRotations[direction];
    }
}
