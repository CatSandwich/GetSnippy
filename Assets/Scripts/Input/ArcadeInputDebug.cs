using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

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
            Stick1 = _bind(0);
            Stick2 = _bind(1);
            Stick3 = _bind(2);
            Stick4 = _bind(3);
        }

        private StickInput _bind(int index)
        {
            StickInput input = new(Joystick.all[index].stick);
            input.PositionChanged += v2 => Debug.LogError($"Stick {index} changed: {v2}");
            return input;
        }
    }
}
