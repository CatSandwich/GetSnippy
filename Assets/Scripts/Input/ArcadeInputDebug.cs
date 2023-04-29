using System.Linq;
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
            foreach (Joystick joystick in InputSystem.devices.OfType<Joystick>())
            {
                Debug.LogError("----- Joystick -----");
                foreach (InputControl device in joystick.allControls)
                {
                    Debug.LogError(device.GetType());
                }
            }

            Player1 = new PlayerInput(Joystick.all.ElementAtOrDefault(3), Joystick.all.ElementAtOrDefault(1));
            Player2 = new PlayerInput(Joystick.all.ElementAtOrDefault(0), Joystick.all.ElementAtOrDefault(2));

            Player1.ChangeDirection += v2 => Debug.LogError($"P1 Direction changed: {v2}");
            Player1.Move += () => Debug.LogError("P1 Move");
            Player1.In += () => Debug.LogError("P1 In");
            Player1.Out += () => Debug.LogError("P1 Out");

            Player2.ChangeDirection += v2 => Debug.LogError($"P2 Direction changed: {v2}");
            Player2.Move += () => Debug.LogError("P2 Move");
            Player2.In += () => Debug.LogError("P2 In");
            Player2.Out += () => Debug.LogError("P2 Out");
        }

        void Update()
        {
            Player1.Update();
            Player2.Update();
        }
    }
}