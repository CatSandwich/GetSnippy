using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace Input
{
    public class StickInput
    {
        public event Action<Vector2Int> PositionChanged;
        private readonly StickControl _stick;

        public Vector2Int Position
        {
            get => _position;
            set => _setPosition(value);
        }
        private Vector2Int _position = Vector2Int.zero;

        public StickInput(StickControl stick)
        {
            _stick = stick;

            if (_stick == null)
            {
                Debug.LogWarning("StickInput received a null StickControl. No input will be received.");
            }
        }
        
        public void Update()
        {
            if (_stick != null)
            {
                Position = new Vector2(_stick.x.ReadValue(), _stick.y.ReadValue()).Round();
            }
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
