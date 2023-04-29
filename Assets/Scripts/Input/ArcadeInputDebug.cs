using System;
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
            foreach ((Joystick joystick, int index) in Joystick.all.Select((j, i) => (j, i)))
            {
                StickControl stick = joystick.stick;

                float x = stick.x.ReadValue();
                if (x != 0)
                {
                    Debug.LogError($"Stick {index} x: {x}");
                }

                float y = stick.y.ReadValue();
                if (y != 0)
                {
                    Debug.LogError($"Stick {index} y: {y}");
                }

            }
        }

        void Try(Action action)
        {
            try
            {
                action();
            }
            catch(Exception)
            {
                // This is a horrible idea
            }
        }
    }
}
