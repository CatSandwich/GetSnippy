using System;
using System.Collections.Generic;
using UnityEngine;

namespace Input
{
    public class PlayerInput
    {
        public static PlayerInput Player1 => new(JoystickInput.Joystick1, JoystickInput.Joystick2);
        public static PlayerInput Player2 => new(JoystickInput.Joystick3, JoystickInput.Joystick4);

        public event Action<Vector2Int> ChangeDirection;
        public event Action Move;
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

        private int _numLeftButtonsPressed;
        private int _numRightButtonsPressed;
        private bool _movementState;

        public PlayerInput(JoystickInput leftJoystick, JoystickInput rightJoystick)
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
            (Vector2Int left, Vector2Int right) = MovementStates[_movementState];

            // Check left exact and right approximate
            if (LeftJoystick.StickPosition == left && RightJoystick.StickPosition.y == right.y)
            {
                Move?.Invoke();
                _movementState = !_movementState;
            }
            // Check right exact and left approximate
            else if (RightJoystick.StickPosition == right && LeftJoystick.StickPosition.y == left.y)
            {
                Move?.Invoke();
                _movementState = !_movementState;
            }
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