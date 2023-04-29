using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace Input
{
    public class PlayerInput
    {
        public event Action<Vector2Int> ChangeDirection;
        public event Action Move;
        public event Action Out;
        public event Action In;

        public StickInput LeftStick;
        public StickInput RightStick;
        
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

        public PlayerInput(Joystick leftJoystick, Joystick rightJoystick)
        {
            LeftStick = new StickInput(leftJoystick);
            RightStick = new StickInput(rightJoystick);

            LeftStick.PositionChanged += _ => OnStickMoved();
            RightStick.PositionChanged += _ => OnStickMoved();
        }

        public void Update()
        {
            LeftStick.Update();
            RightStick.Update();
        }

        private void OnStickMoved()
        {
            CheckChangeDirection();
            CheckMove();
            CheckIn();
            CheckOut();
        }

        private void CheckChangeDirection()
        {
            foreach (Vector2Int direction in Directions)
            {
                if (LeftStick.Position == direction && RightStick.Position == direction)
                {
                    ChangeDirection?.Invoke(direction);
                }
            }
        }

        private void CheckMove()
        {
            (Vector2Int left, Vector2Int right) = MovementStates[_movementState];

            if (LeftStick.Position == left && RightStick.Position == right)
            {
                Move?.Invoke();
                _movementState = !_movementState;
            }
        }

        private void CheckIn()
        {
            if (LeftStick.Position == Vector2Int.right && RightStick.Position == Vector2Int.left)
            {
                In?.Invoke();
            }
        }

        private void CheckOut()
        {
            if (LeftStick.Position == Vector2Int.left && RightStick.Position == Vector2Int.right)
            {
                Out?.Invoke();
            }
        }
    }
}