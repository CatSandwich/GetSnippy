using System.Collections.Generic;
using UnityEngine;

namespace Input
{
    public class Vector2IntPreprocessor
    {
        protected readonly Dictionary<Vector2Int, Vector2Int> Map = new();

        public Vector2Int this[Vector2Int input]
        {
            set => Map[input] = value;
        }

        public Vector2Int Process(Vector2Int input)
        {
            return Map.TryGetValue(input, out Vector2Int output)
                       ? output
                       : input;
        }
    }

    public class Vector2IntRotatePreprocessor : Vector2IntPreprocessor
    {
        public static readonly Vector2IntRotatePreprocessor Left = new(1);
        public static readonly Vector2IntRotatePreprocessor Right = new(7);

        private static readonly Vector2Int[] Positions =
        {
            new(0, 1),
            new(1, 1),
            new(1, 0),
            new(1, -1),
            new(0, -1),
            new(-1, -1),
            new(-1, 0),
            new(-1, 1)
        };

        public Vector2IntRotatePreprocessor(int rotations)
        {
            for (int i = 0; i < 8; i++)
            {
                Map[Positions[i]] = Positions[(i + rotations) % 8];
            }
        }
    }
}