using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace Input
{
    public class JoystickInput : MonoBehaviour
    {
        public bool HasStick => _stick != null;
    
        private StickControl _stick;
        private Vector2 _position;

        // Update is called once per frame
        void Update()
        {
            // Ensure we have a stick bound
            if (!_getJoystick())
            {
                return;
            }

            // Check if input changed
            if (!_read())
            {
                return;
            }

            Debug.LogError($"Stick: {_position}");
        }

        private bool _getJoystick()
        {
            // If already has bound stick
            if (HasStick)
            {
                return true;
            }

            // Find a stick with movement and bind to it
            foreach (Joystick joystick in Joystick.all)
            {
                StickControl stick = joystick.stick;

                if (stick.ReadValue() != Vector2.zero)
                {
                    _stick = stick;
                    return true;
                }
            }

            // Still no bound stick
            return false;
        }
        
        private bool _read()
        {
            Vector2 position = _stick.ReadValue();
            bool changed = position != _position;
            _position = position;
            return changed;
        }
    }
}
