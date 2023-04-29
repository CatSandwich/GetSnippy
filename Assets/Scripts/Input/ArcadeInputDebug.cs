/*using System;
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
                            Debug.LogError($"Joystick hatswitch x: {x}");
                        }

                        y = hatSwich.y.ReadValue();
                        if (y != 0)
                        {
                            Debug.LogError($"Joystick hatswitch y: {y}");
                        }
                    });


                Try(() =>
                    {
                        StickControl stick = joystick.stick;
                        x = stick.x.ReadValue();
                        if (x != 0)
                        {
                            Debug.LogError($"Joystick stick x: {x}");
                        }

                        y = stick.y.ReadValue();
                        if (y != 0)
                        {
                            Debug.LogError($"Joystick stick y: {y}");
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
                            Debug.LogError($"Gamepad left stick x: {x}");
                        }

                        y = stick.y.ReadValue();
                        if (y != 0)
                        {
                            Debug.LogError($"Gamepad left stick y: {y}");
                        }
                    });


                Try(() =>
                    {
                        StickControl stick = gamepad.rightStick;
                        x = stick.x.ReadValue();
                        if (x != 0)
                        {
                            Debug.LogError($"Gamepad right stick x: {x}");
                        }

                        y = stick.y.ReadValue();
                        if (y != 0)
                        {
                            Debug.LogError($"Gamepad right stick y: {y}");
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
            catch (Exception)
            {
                // This is a horrible idea
            }
        }
    }
}*/

using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
    public class ArcadeInputDebug : MonoBehaviour
    {
        public StickInput Stick1;
        public StickInput Stick2;
        public StickInput Stick3;
        public StickInput Stick4;

        void Start()
        {
            Application.targetFrameRate = 2;
            Stick1 = _bind(0);
            Stick2 = _bind(1);
            Stick3 = _bind(2);
            Stick4 = _bind(3);
        }

        void Update()
        {
            Stick1.Update();
            Stick2.Update();
            Stick3.Update();
            Stick4.Update();
        }

        private StickInput _bind(int index)
        {
            StickInput input = new(Joystick.all[index].stick);
            input.PositionChanged += v2 => Debug.LogError($"Stick {index} changed: {v2}");
            return input;
        }
    }
}