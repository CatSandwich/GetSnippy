using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

namespace Input
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance;

        #region Events
        public event Action Move;
        public event Action<Vector2> DirectionChanged;
        public event Action Lunge;
        #endregion

        #region Direction Mappings
        private static readonly Vector2[] Directions = new[]
        {
            Vector2.up,
            Vector2.right,
            Vector2.down,
            Vector2.left
        };

        private static readonly Dictionary<Vector2, KeyCode> LeftKeyMapping = new()
        {
            [Vector2.up] = KeyCode.W,
            [Vector2.right] = KeyCode.D,
            [Vector2.down] = KeyCode.S,
            [Vector2.left] = KeyCode.A
        };

        private static readonly Dictionary<Vector2, KeyCode> RightKeyMapping = new()
        {
            [Vector2.up] = KeyCode.W,
            [Vector2.right] = KeyCode.D,
            [Vector2.down] = KeyCode.S,
            [Vector2.left] = KeyCode.A
        };
        #endregion

        #region Unity Events
        public void Awake()
        {
            // Singleton pattern
            if (Instance)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void Update()
        {
            UpdateDirection();
        }
        #endregion

        private void UpdateDirection()
        {
            foreach (Vector2 direction in Directions)
            {
                KeyCode left = LeftKeyMapping[direction];
                KeyCode right = RightKeyMapping[direction];

                // Check if either key was pressed this frame while the other is also down
                if (UnityEngine.Input.GetKeyDown(left) && UnityEngine.Input.GetKey(right))
                {
                    DirectionChanged?.Invoke(direction);
                }
                else if (UnityEngine.Input.GetKey(left) && UnityEngine.Input.GetKeyDown(right))
                {
                    DirectionChanged?.Invoke(direction);
                }
            }
        }
    }
}