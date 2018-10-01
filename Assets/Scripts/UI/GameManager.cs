using Assets.Scripts.UI.InputSystem;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager GameManagerInstance;
        public List<InputKey> InputKeys;
        // DO ZMIANY 
        public KeyCode jump { get; set; }
        public KeyCode left { get; set; }
        public KeyCode right { get; set; }
        //
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

            //DO ZMIANY
            jump = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("jumpKey", "Space"));
            left = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("leftKey", "A"));
            right = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("rightKey", "D"));
        }

        private void AddInputKey(string keyName, string defaultValue)
        {
            InputKeys.Add(new InputKey(keyName, defaultValue));
        }

        internal void AddInputKey(SerializedProperty kyName, SerializedProperty defaultInputValue)
        {
            throw new NotImplementedException();
        }
    }
}