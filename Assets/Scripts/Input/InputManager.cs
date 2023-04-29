using System;
using System.Collections.Generic;
using UnityEngine;

namespace Input
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance;

        #region Events
        public event Action Move;
        public event Action<Vector2> DirectionChanged;
        public event Action Out;
        public event Action In;
        #endregion

        #region Direction Mappings
        private static readonly Vector2[] Directions = new[]
        {
            Vector2.up,
            Vector2.right,
            Vector2.down,
            Vector2.left
        };

        private static readonly Dictionary<Vector2, string> DirectionStrings = new()
        {
            [Vector2.up] = "Up",
            [Vector2.right] = "Right",
            [Vector2.down] = "Down",
            [Vector2.left] = "Left"
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
            [Vector2.up] = KeyCode.UpArrow,
            [Vector2.right] = KeyCode.RightArrow,
            [Vector2.down] = KeyCode.DownArrow,
            [Vector2.left] = KeyCode.LeftArrow
        };
        #endregion

        #region Movement Mapping
        private bool _movementState;

        private static readonly Dictionary<bool, (Vector2, Vector2)> MovementStates = new()
        {
            [false] = (Vector2.up, Vector2.down),
            [true] = (Vector2.down, Vector2.up)
        };
        #endregion

        private Vector2? _leftStick;
        private Vector2? _rightStick;

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

        public void Start()
        {
            DirectionChanged += dir =>
            {
                Debug.Log(DirectionStrings[dir]);
            };

            Out += () => Debug.Log("Out");
            In += () => Debug.Log("In");
            Move += () => Debug.Log("Move");
        }

        public void Update()
        {
            // Update joystick configurations, and exit if nothing changed
            if (!UpdateSticks())
            {
                return;
            }

            CheckDirectionChange();
            CheckIn();
            CheckOut();
            CheckMove();
        }
        #endregion

        private bool UpdateSticks()
        {
            bool change = false;
            foreach (Vector2 dir in Directions)
            {
                if (GetKeyDown(LeftKeyMapping[dir]))
                {
                    _leftStick = dir;
                    change = true;
                }

                if (GetKeyDown(RightKeyMapping[dir]))
                {
                    _rightStick = dir;
                    change = true;
                }

                if (GetKeyUp(LeftKeyMapping[dir]))
                {
                    _leftStick = null;
                    change = true;
                }

                if (GetKeyUp(RightKeyMapping[dir]))
                {
                    _rightStick = null;
                    change = true;
                }
            }
            return change;
        }

        private void CheckDirectionChange()
        {
            foreach (Vector2 dir in Directions)
            {
                if (_leftStick == dir && _rightStick == dir)
                {
                    DirectionChanged?.Invoke(dir);
                }
            }
        }

        private void CheckIn()
        {
            if (_leftStick == Vector2.right && _rightStick == Vector2.left)
            {
                In?.Invoke();
            }
        }

        private void CheckOut()
        {
            if (_leftStick == Vector2.left && _rightStick == Vector2.right)
            {
                Out?.Invoke();
            }
        }

        private void CheckMove()
        {
            (Vector2 left, Vector2 right) = MovementStates[_movementState];

            if (_leftStick == left && _rightStick == right)
            {
                Move?.Invoke();
                _movementState = !_movementState;
            }
        }

        #region Input Wrappers
        private static bool GetKeyDown(KeyCode keycode)
        {
            return UnityEngine.Input.GetKeyDown(keycode);
        }

        private static bool GetKeyUp(KeyCode keycode)
        {
            return UnityEngine.Input.GetKeyUp(keycode);
        }
        #endregion
    }
}