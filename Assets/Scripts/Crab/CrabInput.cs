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

        public Input.InputManager inputManager;
        public PlayerInput playerInput;

        void Awake()
        {
            if (player == Player.Player1)
            {
                inputManager = new Input.InputManager();
                playerInput = new PlayerInput(Joystick.all.ElementAtOrDefault(PLAYER1_JOYSTICK1), Joystick.all.ElementAtOrDefault(PLAYER1_JOYSTICK2));
            }
            else
            {
                //inputManager = new Input.InputManager();
                playerInput = new PlayerInput(Joystick.all.ElementAtOrDefault(PLAYER2_JOYSTICK1), Joystick.all.ElementAtOrDefault(PLAYER2_JOYSTICK2));
            }
        }

        private void Update()
        {
            if (inputManager != null)
            {
                inputManager.Update();
            }

            playerInput.Update();
        }
    }
}