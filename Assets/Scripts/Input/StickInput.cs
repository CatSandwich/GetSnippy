using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace Input
{
    public class StickInput
    {
        public event Action<Vector2Int> PositionChanged;
        private readonly StickControl _stick;
        private Vector2Int _position;

        public StickInput(StickControl stick)
        {
            _stick = stick;
        }
        
        public void Update()
        {
            _setPosition(_stick.ReadValue().Round());
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

        private void _setPosition(Vector2Int position)
        {
            if (position != _position)
            {
                _position = position;
                PositionChanged?.Invoke(position);
            }
        }
    }
}