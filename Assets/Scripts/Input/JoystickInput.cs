using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace Input
{
    public class JoystickInput
    {
        public event Action<Vector2Int> PositionChanged;
        public event Action<ButtonColor> ButtonPressed;
        public event Action<ButtonColor> ButtonReleased;

        private readonly Joystick _joystick;
        private readonly StickControl _stick;
        private readonly Dictionary<ButtonColor, ButtonControl> _buttons;
        private readonly Dictionary<ButtonColor, bool> _buttonStates;

        public Vector2Int StickPosition
        {
            get => _stickPosition;
            set => _setPosition(value);
        }
        private Vector2Int _stickPosition = Vector2Int.zero;

        public JoystickInput(Joystick joystick)
        {
            _joystick = joystick;

            if (joystick == null)
            {
                Debug.LogWarning("JoystickInput received a null Joystick. No input will be received.");
                return;
            }

            _stick = joystick.stick;

            _buttons = new Dictionary<ButtonColor, ButtonControl>
            {
                [ButtonColor.Yellow] = (ButtonControl) joystick["trigger"],
                [ButtonColor.Red] = (ButtonControl) joystick["button2"],
                [ButtonColor.Green] = (ButtonControl) joystick["button3"],
                [ButtonColor.Blue] = (ButtonControl) joystick["button4"]
            };

            _buttonStates = new Dictionary<ButtonColor, bool>
            {
                [ButtonColor.Yellow] = false,
                [ButtonColor.Red] = false,
                [ButtonColor.Green] = false,
                [ButtonColor.Blue] = false
            };
        }
        
        public void Update()
        {
            if (_joystick == null) return;

            _updateStickPosition();
            _updateButtons();
        }

        private void _updateStickPosition()
        {
            StickPosition = new Vector2(_stick.x.ReadValue(), _stick.y.ReadValue()).Round();
        }

        private void _updateButtons()
        {
            foreach (ButtonColor color in _buttons.Keys)
            {
                bool oldState = _buttonStates[color];
                bool newState = _buttons[color].ReadValue() != 0;

                if (oldState != newState)
                {
                    _buttonStates[color] = newState;

                    if (newState)
                    {
                        ButtonPressed?.Invoke(color);
                    }
                    else
                    {
                        ButtonReleased?.Invoke(color);
                    }
                }
            }
        }
        
        private void _setPosition(Vector2Int position)
        {
            if (position != _stickPosition)
            {
                _stickPosition = position;
                PositionChanged?.Invoke(position);
            }
        }

        public static bool TryBindJoystick(out StickControl stick)
        {
            // Find a stick with movement and bind to it
            foreach (Joystick joystick in Joystick.all)
            {
                if (joystick.stick.ReadValue().Round() != Vector2Int.zero)
                {
                    stick = joystick.stick;
                    return true;
                }
            }
            
            stick = null;
            return false;
        }

        public enum ButtonColor
        {
            Yellow,
            Red,
            Green,
            Blue
        }
    }
}