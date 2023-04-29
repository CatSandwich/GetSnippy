using System;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
    public class ArcadeInputDebug : MonoBehaviour
    {
        public PlayerInput Player1;
        public PlayerInput Player2;

        void Start()
        {
            Player1 = new PlayerInput(Joystick.all.ElementAtOrDefault(3), Joystick.all.ElementAtOrDefault(1));
            Player2 = new PlayerInput(Joystick.all.ElementAtOrDefault(0), Joystick.all.ElementAtOrDefault(2));

            Player1.ChangeDirection += v2 => Debug.LogError($"P1 Direction changed: {v2}");
            Player1.Move += () => Debug.LogError("P1 Move");
            Player1.In += () => Debug.LogError("P1 In");
            Player1.Out += () => Debug.LogError("P1 Out");
            Player1.Lunge += () => Debug.LogError("P1 Lunge");

            Player2.ChangeDirection += v2 => Debug.LogError($"P2 Direction changed: {v2}");
            Player2.Move += () => Debug.LogError("P2 Move");
            Player2.In += () => Debug.LogError("P2 In");
            Player2.Out += () => Debug.LogError("P2 Out");
            Player2.Lunge += () => Debug.LogError("P2 Lunge");
        }

        void Update()
        {
            Player1.Update();
            Player2.Update();
        }
    }
}