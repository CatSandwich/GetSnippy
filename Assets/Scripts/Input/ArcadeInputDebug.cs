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
            Player1 = new PlayerInput(Joystick.all[3].stick, Joystick.all[1].stick);
            Player2 = new PlayerInput(Joystick.all[0].stick, Joystick.all[2].stick);

            Player1.ChangeDirection += v2 => Debug.LogError($"P1 Direction changed: {v2}");
            Player1.Move += () => Debug.Log("P1 Move");
            Player1.In += () => Debug.Log("P1 In");
            Player1.Out += () => Debug.Log("P1 Out");

            Player2.ChangeDirection += v2 => Debug.LogError($"P2 Direction changed: {v2}");
            Player2.Move += () => Debug.Log("P2 Move");
            Player2.In += () => Debug.Log("P2 In");
            Player2.Out += () => Debug.Log("P2 Out");
        }

        void Update()
        {
            Player1.Update();
            Player2.Update();
        }
    }
}