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

        void Awake()
        {
            if (player == Player.Player1)
            {
                inputManager = new Input.InputManager();
                playerInput = PlayerInput2.Player1;
            }
            else
            {
                //inputManager = new Input.InputManager();
                playerInput = PlayerInput2.Player2;
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
        }
    }
}