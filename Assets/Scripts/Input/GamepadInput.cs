using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace Input
{
    public class GamepadInput : Input.IPlayerInput
    {
        public static GamepadInput Gamepad1 => new(Gamepad.all.ElementAtOrDefault(0));
        public static GamepadInput Gamepad2 => new(Gamepad.all.ElementAtOrDefault(1));

        // IPlayerInput
        public event Action<CrabDirection> Move;
        public event Action<CrabClawPose> ChangeClawPose;
        public event Action LungeLeft;
        public event Action LungeRight;

        private readonly Gamepad _gamepad;

        private const float c_StickThreshold = 0.65f;
        private const float c_MoveTimeThreshold = 0.4f;
        private float _lastMoveTime = float.MinValue;
        private CrabDirection _currentMoveDirection = CrabDirection.Left;
        private StickDirection _nextLeftStickDirection = StickDirection.Neutral;

        enum StickDirection
        {
            Neutral,
            Left,
            Right,
            Up,
            Down
        }

        public GamepadInput(Gamepad gamepad)
        {
            _gamepad = gamepad;

            if (gamepad == null)
            {
                Debug.LogWarning("GamepadInput received a null Gamepad. No input will be received.");
                return;
            }
        }

        public void Update()
        {
            if (_gamepad == null)
            {
                return;
            }

            UpdateStickInput();
            UpdateLungeInput();
        }

        private void UpdateStickInput()
        {
            StickDirection leftStickDirection = GetStickDirection(_gamepad.leftStick);
            StickDirection rightStickDirection = GetStickDirection(_gamepad.rightStick);

            if (leftStickDirection != StickDirection.Neutral)
            {
                if (leftStickDirection == rightStickDirection)
                {
                    SameDirectionInput(leftStickDirection);
                }
                else if (leftStickDirection == GetOppositeDirection(rightStickDirection))
                {
                    OppositeDirectionInput(leftStickDirection);
                }
            }
        }

        private StickDirection GetStickDirection(StickControl stick)
        {
            Vector2 stickVec = stick.ReadValue();
            float absX = Mathf.Abs(stickVec.x);
            float absY = Mathf.Abs(stickVec.y);

            if (absX > c_StickThreshold && absX > absY)
            {
                if (stickVec.x > 0)
                {
                    return StickDirection.Right;
                }
                else
                {
                    return StickDirection.Left;
                }
            }
            else if (absY > c_StickThreshold && absY > absX)
            {
                if (stickVec.y > 0)
                {
                    return StickDirection.Up;
                }
                else
                {
                    return StickDirection.Down;
                }
            }
            else
            {
                return StickDirection.Neutral;
            }
        }

        private void SameDirectionInput(StickDirection direction)
        {
            switch (direction)
            {
                case StickDirection.Up: Move?.Invoke(CrabDirection.Forward); break;
                case StickDirection.Down: Move?.Invoke(CrabDirection.Backward); break;
                case StickDirection.Left: ChangeClawPose?.Invoke(CrabClawPose.Left); break;
                case StickDirection.Right: ChangeClawPose?.Invoke(CrabClawPose.Right); break;
            }
        }

        private StickDirection GetOppositeDirection(StickDirection direction)
        {
            switch (direction)
            {
                case StickDirection.Left: return StickDirection.Right;
                case StickDirection.Right: return StickDirection.Left;
                case StickDirection.Up: return StickDirection.Down;
                case StickDirection.Down: return StickDirection.Up;
                default: return StickDirection.Neutral;
            }
        }

        private void OppositeDirectionInput(StickDirection leftStickDirection)
        {
            switch (leftStickDirection)
            {
                case StickDirection.Left:
                    ChangeClawPose?.Invoke(CrabClawPose.Open);
                    break;
                case StickDirection.Right:
                    ChangeClawPose?.Invoke(CrabClawPose.Closed);
                    break;

                case StickDirection.Up:
                case StickDirection.Down:
                    HandleWalkSideways(leftStickDirection);
                    break;
            }
        }

        private void HandleWalkSideways(StickDirection leftStickDirection)
        {
            if (Time.time - _lastMoveTime > c_MoveTimeThreshold)
            {
                CheckNewMove(leftStickDirection);
            }
            else
            {
                CheckContinueMove(leftStickDirection);
            }
        }

        private void CheckNewMove(StickDirection leftStickDirection)
        {
            if (leftStickDirection == StickDirection.Up)
            {
                // Start moving left
                _currentMoveDirection = CrabDirection.Left;
                Move?.Invoke(_currentMoveDirection);
                _nextLeftStickDirection = StickDirection.Down;
                _lastMoveTime = Time.time;
            }
            else if (leftStickDirection == StickDirection.Down)
            {
                // Start moving right
                _currentMoveDirection = CrabDirection.Right;
                Move?.Invoke(_currentMoveDirection);
                _nextLeftStickDirection = StickDirection.Up;
                _lastMoveTime = Time.time;
            }
        }

        private void CheckContinueMove(StickDirection leftStickDirection)
        {
            if (leftStickDirection == _nextLeftStickDirection)
            {
                Move?.Invoke(_currentMoveDirection);
                _nextLeftStickDirection = (_nextLeftStickDirection == StickDirection.Left)
                    ? StickDirection.Right
                    : StickDirection.Left;
                _lastMoveTime = Time.time;
            }
        }

        private void UpdateLungeInput()
        {
            if (CheckForLunge(_gamepad.leftTrigger, _gamepad.leftStickButton))
            {
                LungeLeft?.Invoke();
            }
            if (CheckForLunge(_gamepad.rightTrigger, _gamepad.rightStickButton))
            {
                LungeRight?.Invoke();
            }
        }

        private bool CheckForLunge(ButtonControl button1, ButtonControl button2)
        {
            return (button1.IsPressed() && button2.IsPressed())
                && (button1.wasPressedThisFrame || button2.wasPressedThisFrame);
        }
    }
}
