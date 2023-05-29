using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
    public class CrabInput : MonoBehaviour
    {
        [SerializeField]
        private Player player;

        public Input.InputManager inputManager;
        public PlayerInput2 playerInput;
        public GamepadInput gamepadInput;

        private const bool c_ArcadeMode = false;

        void Awake()
        {
            if (player == Player.Player1)
            {
                inputManager = new Input.InputManager();
                if (c_ArcadeMode)
                {
                    playerInput = PlayerInput2.Player1;
                }
                else
                {
                    gamepadInput = GamepadInput.Gamepad1;
                }
            }
            else
            {
                //inputManager = new Input.InputManager();
                if (c_ArcadeMode)
                {
                    playerInput = PlayerInput2.Player2;
                }
                else
                {
                    gamepadInput = GamepadInput.Gamepad2;
                }
            }
        }

        private void Update()
        {
            if (inputManager != null)
            {
                inputManager.Update();
            }

            if (playerInput != null)
            {
                playerInput.Update();
            }

            if (gamepadInput != null)
            {
                gamepadInput.Update();
            }
        }
    }
}