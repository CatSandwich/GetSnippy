using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
    public class InputManager
    {
        #region Events
        public event Action<CrabDirection> Move;
        public event Action<Vector2Int> ChangeDirection;
        public event Action Out;
        public event Action In;
        public event Action LungeLeft;
        public event Action LungeRight;
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
        public const float c_MoveThreshold = 0.3f;
        private bool _movementState;
        private float _lastMoveTime = float.MinValue;
        private CrabDirection _currentMoveDirection = CrabDirection.Left;

        private static readonly Dictionary<bool, (Vector2, Vector2)> MovementStates = new()
        {
            [false] = (Vector2.up, Vector2.down),
            [true] = (Vector2.down, Vector2.up)
        };
        #endregion

        private Vector2 _leftStick = Vector2.zero;
        private Vector2 _rightStick = Vector2.zero;

        #region Unity Events
        public void Update()
        {
            CheckLunge();

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
                    _leftStick += dir;
                    change = true;
                }

                if (GetKeyDown(RightKeyMapping[dir]))
                {
                    _rightStick += dir;
                    change = true;
                }

                if (GetKeyUp(LeftKeyMapping[dir]))
                {
                    _leftStick -= dir;
                    change = true;
                }

                if (GetKeyUp(RightKeyMapping[dir]))
                {
                    _rightStick -= dir;
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
                    ChangeDirection?.Invoke(Vector2Int.RoundToInt(dir));
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

        private void CheckLunge()
        {
            if (GetKeyDown(KeyCode.Space))
            {
                LungeLeft?.Invoke();
            }
            if (GetKeyDown(KeyCode.RightControl))
            {
                LungeRight?.Invoke();
            }
        }

        private void CheckMove()
        {
            if (Time.time - _lastMoveTime > c_MoveThreshold)
            {
                CheckNewMove();
            }
            else
            {
                CheckContinueMove();
            }
        }

        private void CheckNewMove()
        {
            if (_leftStick == Vector2Int.up && _rightStick == Vector2Int.down)
            {
                // Start moving left
                _currentMoveDirection = CrabDirection.Left;
                Move?.Invoke(_currentMoveDirection);
                _movementState = true;
                _lastMoveTime = Time.time;
            }
            else if (_leftStick == Vector2Int.down && _rightStick == Vector2Int.up)
            {
                // Start moving right
                _currentMoveDirection = CrabDirection.Right;
                Move?.Invoke(_currentMoveDirection);
                _movementState = false;
                _lastMoveTime = Time.time;
            }
        }

        private void CheckContinueMove()
        {
            (Vector2 left, Vector2 right) = MovementStates[_movementState];

            if (_leftStick == left && _rightStick == right)
            {
                Move?.Invoke(_currentMoveDirection);
                _movementState = !_movementState;
                _lastMoveTime = Time.time;
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