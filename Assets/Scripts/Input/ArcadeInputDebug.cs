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
            foreach (Joystick joystick in Joystick.all)
            {
                float x, y;

                Try(() =>
                    {
                        Vector2Control hatSwich = joystick.hatswitch;
                        x = hatSwich.x.ReadValue();
                        if (x != 0)
                        {
                            Debug.LogError($"Hatswitch x: {x}");
                        }

                        y = hatSwich.y.ReadValue();
                        if (y != 0)
                        {
                            Debug.LogError($"Hatswitch y: {y}");
                        }
                    });


                Try(() =>
                    {
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
                    });

            }

            foreach (Gamepad gamepad in Gamepad.all)
            {
                
                float x, y;

                Try(() =>
                {
                    StickControl stick = gamepad.leftStick;
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
                });
                

                Try(() =>
                {
                    StickControl stick = gamepad.rightStick;
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
                });

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
