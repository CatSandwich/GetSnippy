using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
        public event Action Lunge;

        public JoystickInput LeftJoystick;
        public JoystickInput RightJoystick;
        
        #region Direction Mappings
        private static readonly Vector2Int[] Directions = 
        {
            Vector2Int.up,
            Vector2Int.right,
            Vector2Int.down,
            Vector2Int.left
        };
        #endregion

        #region Movement Mapping
        private bool _movementState;

        private static readonly Dictionary<bool, (Vector2Int, Vector2Int)> MovementStates = new()
        {
            [false] = (Vector2Int.up, Vector2Int.down),
            [true] = (Vector2Int.down, Vector2Int.up)
        };
        #endregion

        private int _numButtonsPressed;

        public PlayerInput(JoystickInput leftJoystick, JoystickInput rightJoystick)
        {
            LeftJoystick = leftJoystick;
            RightJoystick = rightJoystick;

            LeftJoystick.PositionChanged += _ => OnStickMoved();
            RightJoystick.PositionChanged += _ => OnStickMoved();

            LeftJoystick.ButtonPressed += _ => OnButtonPressed();
            RightJoystick.ButtonPressed += _ => OnButtonPressed();

            LeftJoystick.ButtonReleased += _ => OnButtonReleased();
            RightJoystick.ButtonReleased += _ => OnButtonReleased();
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

        private void OnButtonPressed()
        {
            _numButtonsPressed++;

            if (_numButtonsPressed == 2)
            {
                Lunge?.Invoke();
            }
        }

        private void OnButtonReleased()
        {
            _numButtonsPressed--;
        }

        private void CheckChangeDirection()
        {
            foreach (Vector2Int direction in Directions)
            {
                if (LeftJoystick.StickPosition == direction && RightJoystick.StickPosition == direction)
                {
                    ChangeDirection?.Invoke(direction);
                }
            }
        }

        private void CheckMove()
        {
            (Vector2Int left, Vector2Int right) = MovementStates[_movementState];

            if (LeftJoystick.StickPosition == left && RightJoystick.StickPosition == right)
            {
                Move?.Invoke();
                _movementState = !_movementState;
            }
        }

        private void CheckIn()
        {
            if (LeftJoystick.StickPosition == Vector2Int.right && RightJoystick.StickPosition == Vector2Int.left)
            {
                In?.Invoke();
            }
        }

        private void CheckOut()
        {
            if (LeftJoystick.StickPosition == Vector2Int.left && RightJoystick.StickPosition == Vector2Int.right)
            {
                Out?.Invoke();
            }
        }
    }
}