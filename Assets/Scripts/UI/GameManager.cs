using Assets.Scripts.UI.InputSystem;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager GameManagerInstance;
        public List<InputKey> InputKeys;

        private void Awake()
        {
            if (GameManagerInstance == null)
            {
                DontDestroyOnLoad(gameObject);
                GameManagerInstance = this;
            }
            else if (GameManagerInstance != this)
            {
                Destroy(gameObject);
            }
        }
    }
}