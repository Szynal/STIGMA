using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI.InputSystem
{
    public class InputKey : MonoBehaviour
    {
        private KeyCode KeyCode { get; set; }
        private string KeyName { get; set; }
        private string DefaultInputValue { get; set; }

        /// <summary>
        /// Crete new Key Input for PlayerInputPreferences
        /// </summary>
        /// <param name="keyName"> Example: jumpKey</param>
        /// <param name="defaultValue"> Example: Space</param>
        public InputKey(string keyName, string defaultValue)
        {
            KeyName = keyName;
            DefaultInputValue = defaultValue;
            KeyCode = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString(keyName, defaultValue));
        }

        public InputKey GetInputKeyByNeme(string name, List<InputKey> inputKeys)
        {
            foreach (var inputKey in inputKeys)
            {
                if (inputKey.KeyName == name)
                {
                    return inputKey;
                }
            }
            Debug.LogWarning("InputKey with this name does not exist.");
            Console.WriteLine("InputKey with this name does not exist.");
            return null;
        }
    }
}
