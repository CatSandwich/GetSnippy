using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace Input
{
    public class ArcadeInputDebug : MonoBehaviour
    {
        void Start()
        {
            Debug.LogError($"# joysticks: {Joystick.all.Count}");
            Debug.LogError($"# gamepads: {Gamepad.all.Count}");
        }

        // Update is called once per frame
        void Update()
        {
            foreach (Joystick joystick in Joystick.all)
            {
                Vector2Control hatSwich = joystick.hatswitch;

                float x = hatSwich.x.ReadValue();
                if (x != 0)
                {
                    Debug.LogError($"Hatswitch x: {x}");
                }

                float y = hatSwich.y.ReadValue();
                if (y != 0)
                {
                    Debug.LogError($"Hatswitch y: {y}");
                }

                StickControl stick = joystick.stick;
                x = stick.x.ReadValue();
                if (x != 0)
                {
                    Debug.LogError($"Stick x: {x}");
                }

                y = stick.y.ReadValue();
                if (y != 0)
                {
                    Debug.LogError($"Stick y: {y}");
                }
            }

            foreach (Gamepad gamepad in Gamepad.all)
            {
                StickControl stick = gamepad.leftStick;
                
                float x = stick.x.ReadValue();
                if (x != 0)
                {
                    Debug.LogError($"Stick x: {x}");
                }

                float y = stick.y.ReadValue();
                if (y != 0)
                {
                    Debug.LogError($"Stick y: {y}");
                }
                
                stick = gamepad.rightStick;

                x = stick.x.ReadValue();
                if (x != 0)
                {
                    Debug.LogError($"Stick x: {x}");
                }

                y = stick.y.ReadValue();
                if (y != 0)
                {
                    Debug.LogError($"Stick y: {y}");
                }
            }
        }
    }
}
