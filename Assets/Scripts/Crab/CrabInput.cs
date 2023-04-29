using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
    public class CrabInput : MonoBehaviour
    {
        const int PLAYER1_JOYSTICK1 = 3;
        const int PLAYER1_JOYSTICK2 = 1;

        const int PLAYER2_JOYSTICK1 = 0;
        const int PLAYER2_JOYSTICK2 = 2;

        [SerializeField]
        private Player player;

        public Input.PlayerInput input;

        void Awake()
        {
            if (player == Player.Player1)
            {
                input = new PlayerInput(Joystick.all.ElementAtOrDefault(PLAYER1_JOYSTICK1), Joystick.all.ElementAtOrDefault(PLAYER1_JOYSTICK2));
            }
            else
            {
                input = new PlayerInput(Joystick.all.ElementAtOrDefault(PLAYER2_JOYSTICK1), Joystick.all.ElementAtOrDefault(PLAYER2_JOYSTICK2));

            }
        }

        private void Update()
        {
            input.Update();
        }
    }
}