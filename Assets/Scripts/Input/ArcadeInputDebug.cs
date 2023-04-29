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
            Debug.LogError($"Binding to stick {index}");
            StickInput input = new(Joystick.all[index].stick);
            input.PositionChanged += v2 => Debug.LogError($"Stick {index} changed: {v2}");
            return input;
        }
    }
}
