using System;
using System.Collections.Generic;
using UnityEngine;

namespace Input
{
    public class PlayerInput2
    {
        public static PlayerInput2 Player1 => new(JoystickInput.Joystick1, JoystickInput.Joystick2);
        public static PlayerInput2 Player2 => new(JoystickInput.Joystick3, JoystickInput.Joystick4);

        public event Action<Vector2Int> ChangeDirection;
        public event Action<CrabDirection> Move;
        public event Action Out;
        public event Action In;
        public event Action LungeLeft;
        public event Action LungeRight;

        public JoystickInput LeftJoystick;
        public JoystickInput RightJoystick;

        private static readonly Dictionary<bool, (Vector2Int, Vector2Int)> MovementStates = new()
        {
            [false] = (Vector2Int.up, Vector2Int.down),
            [true] = (Vector2Int.down, Vector2Int.up)
        };

        public const float c_MoveThreshold = 0.3f;
        private int _numLeftButtonsPressed;
        private int _numRightButtonsPressed;
        private bool _movementState;
        private float _lastMoveTime = float.MinValue;
        private CrabDirection _currentMoveDirection = CrabDirection.Left;

        public PlayerInput2(JoystickInput leftJoystick, JoystickInput rightJoystick)
        {
            LeftJoystick = leftJoystick;
            RightJoystick = rightJoystick;

            LeftJoystick.PositionChanged += _ => OnStickMoved();
            RightJoystick.PositionChanged += _ => OnStickMoved();

            LeftJoystick.ButtonPressed += _ => OnLeftButtonPressed();
            LeftJoystick.ButtonReleased += _ => OnLeftButtonReleased();
            
            RightJoystick.ButtonPressed += _ => OnRightButtonPressed();
            RightJoystick.ButtonReleased += _ => OnRightButtonReleased();
        }

        public void Update()
        {
            LeftJoystick.Update();
            RightJoystick.Update();
        }

        private void OnStickMoved()
        {
            CheckChangeDirection();
            CheckMove();
            CheckIn();
            CheckOut();
        }

        private void OnLeftButtonPressed()
        {
            _numLeftButtonsPressed++;

            if (_numLeftButtonsPressed == 2)
            {
                LungeLeft?.Invoke();
            }
        }

        private void OnLeftButtonReleased()
        {
            _numLeftButtonsPressed--;
        }

        private void OnRightButtonPressed()
        {
            _numRightButtonsPressed++;

            if (_numRightButtonsPressed == 2)
            {
                LungeRight?.Invoke();
            }
        }

        private void OnRightButtonReleased()
        {
            _numRightButtonsPressed--;
        }

        private void CheckChangeDirection()
        {
            // Check up with left exact and right approximate
            if (LeftJoystick.StickPosition == Vector2Int.up && RightJoystick.StickPosition.y == 1)
            {
                ChangeDirection?.Invoke(Vector2Int.up);
            }
            // Check up with right exact and left approximate
            else if (RightJoystick.StickPosition == Vector2Int.up && LeftJoystick.StickPosition.y == 1)
            {
                ChangeDirection?.Invoke(Vector2Int.up);
            }
            // Check right with left exact and right approximate
            else if (LeftJoystick.StickPosition == Vector2Int.right && RightJoystick.StickPosition.x == 1)
            {
                ChangeDirection?.Invoke(Vector2Int.right);
            }
            // Check right with right exact and left approximate
            else if (RightJoystick.StickPosition == Vector2Int.right && LeftJoystick.StickPosition.x == 1)
            {
                ChangeDirection?.Invoke(Vector2Int.right);
            }
            // Check down with left exact and right approximate
            else if (LeftJoystick.StickPosition == Vector2Int.down && RightJoystick.StickPosition.y == -1)
            {
                ChangeDirection?.Invoke(Vector2Int.down);
            }
            // Check down with right exact and left approximate
            else if (RightJoystick.StickPosition == Vector2Int.down && LeftJoystick.StickPosition.y == -1)
            {
                ChangeDirection?.Invoke(Vector2Int.down);
            }
            // Check left with left exact and right approximate
            else if (LeftJoystick.StickPosition == Vector2Int.left && RightJoystick.StickPosition.x == -1)
            {
                ChangeDirection?.Invoke(Vector2Int.left);
            }
            // Check left with right exact and left approximate
            else if (RightJoystick.StickPosition == Vector2Int.left && LeftJoystick.StickPosition.x == -1)
            {
                ChangeDirection?.Invoke(Vector2Int.left);
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

        // If time since last move was higher than threshold, then we consider this a new movement.
        // Set the move direction based on which stick was pushed forwards.
        private void CheckNewMove()
        {
            if (IsInMovePosition(Vector2Int.up, Vector2Int.down))
            {
                // Start moving left
                _currentMoveDirection = CrabDirection.Left;
                Move?.Invoke(_currentMoveDirection);
                _movementState = true;
                _lastMoveTime = Time.time;
            }
            else if (IsInMovePosition(Vector2Int.down, Vector2Int.up))
            {
                // Start moving right
                _currentMoveDirection = CrabDirection.Right;
                Move?.Invoke(_currentMoveDirection);
                _movementState = false;
                _lastMoveTime = Time.time;
            }
        }

        // If user wiggles sticks correctly, continue moving in the current direction.
        private void CheckContinueMove()
        {
            (Vector2Int, Vector2Int) nextRequiredState = MovementStates[_movementState];
            if (IsInMovePosition(nextRequiredState.Item1, nextRequiredState.Item2))
            {
                Move?.Invoke(_currentMoveDirection);
                _movementState = !_movementState;
                _lastMoveTime = Time.time;
            }
        }

        bool IsInMovePosition(Vector2Int left, Vector2Int right)
        {
            return LeftJoystick.StickPosition == left && RightJoystick.StickPosition.y == right.y ||
                   RightJoystick.StickPosition == right && LeftJoystick.StickPosition.y == left.y;
        }

        private void CheckIn()
        {
            // Check with left exact and right approximate
            if (LeftJoystick.StickPosition == Vector2Int.right && RightJoystick.StickPosition.x == -1)
            {
                In?.Invoke();
            }
            // Check with right exact and left approximate
            else if (RightJoystick.StickPosition == Vector2Int.left && LeftJoystick.StickPosition.x == 1)
            {
                In?.Invoke();
            }
        }

        private void CheckOut()
        {
            // Check with left exact and right approximate
            if (LeftJoystick.StickPosition == Vector2Int.left && RightJoystick.StickPosition.x == 1)
            {
                Out?.Invoke();
            }
            // Check with right exact and left approximate
            else if (RightJoystick.StickPosition == Vector2Int.right && LeftJoystick.StickPosition.x == -1)
            {
                Out?.Invoke();
            }
        }
    }
}