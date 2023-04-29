using System;
using UnityEngine;

namespace Input
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance;

        public event Action Move;
        public event Action<Vector2> DirectionChanged;
        public event Action Lunge;

        public void Awake()
        {
            // Singleton pattern
            if (Instance)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}