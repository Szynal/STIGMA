using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI.Input
{
    public class InputKey
    {
        public KeyCode KeyCode { get; set; }
        public string KeyName { get; set; }
        public string DefaultInputValue { get; set; }
        public List<InputKey> InputKeys { get; set; }

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
            AddNewInputKey(this);
        }

        public void AddNewInputKey(InputKey inputKey)
        {
            InputKeys.Add(inputKey);
        }

        public InputKey GetInputKeyByNeme(string name)
        {
            foreach (var inputKey in InputKeys)
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
