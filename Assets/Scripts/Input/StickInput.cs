using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace Input
{
    public class StickInput
    {
        public event Action<Vector2Int> PositionChanged;
        private readonly Joystick _joystick;
        private readonly StickControl _stick;

        public Vector2Int Position
        {
            get => _position;
            set => _setPosition(value);
        }
        private Vector2Int _position = Vector2Int.zero;

        public StickInput(Joystick joystick)
        {
            _joystick = joystick;

            if (joystick == null)
            {
                Debug.LogWarning("StickInput received a null StickControl. No input will be received.");
                return;
            }

            _stick = joystick.stick;
        }
        
        public void Update()
        {
            if (_stick == null) return;

            foreach (ButtonControl button in _joystick.allControls.OfType<ButtonControl>())
            {
                if (button.ReadValue() != 0)
                {
                    Debug.LogError($"{button.name} pressed");
                }
            }

            foreach (DpadControl button in _joystick.allControls.OfType<DpadControl>())
            {
                if (button.x.ReadValue() != 0)
                {
                    Debug.LogError($"{button.x.name} moved");
                }

                if (button.y.ReadValue() != 0)
                {
                    Debug.LogError($"{button.y.name} moved");
                }
            }

            foreach (DpadControl.DpadAxisControl button in _joystick.allControls.OfType<DpadControl.DpadAxisControl>())
            {
                if (button.ReadValue() != 0)
                {
                    Debug.LogError($"{button.name} pressed");
                }
            }

            foreach (AxisControl button in _joystick.allControls.OfType<AxisControl>())
            {
                if (button.ReadValue() != 0)
                {
                    Debug.LogError($"{button.name} moved");
                }
            }

            Position = new Vector2(_stick.x.ReadValue(), _stick.y.ReadValue()).Round();
        }
        
        private void _setPosition(Vector2Int position)
        {
            if (position != _position)
            {
                _position = position;
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
    }
}
