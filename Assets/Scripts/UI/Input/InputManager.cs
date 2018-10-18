using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Input
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager Input;
        public KeyCode Jump { get; set; }
        public KeyCode Left { get; set; }
        public KeyCode Right { get; set; }

        public GameObject LeftKey;
        public GameObject RightKey;
        public GameObject JumpKey;

        private Event keyEvent;
        private Text buttonText;
        private KeyCode newKey;
        private bool waitingForKey;


        private void Awake()
        {
            SingletonPattern();
            InputsInitialization();
        }

        private void Start()
        {
            waitingForKey = false;

            LeftKey.GetComponent<Text>().text = InputManager.Input.Left.ToString();
            RightKey.GetComponent<Text>().text = InputManager.Input.Right.ToString();
            JumpKey.GetComponent<Text>().text = InputManager.Input.Jump.ToString();
        }


        private void SingletonPattern()
        {
            if (Input == null)
            {
                DontDestroyOnLoad(gameObject);
                Input = this;
            }
            else if (Input != this)
            {
                Destroy(gameObject);
            }
        }

        private void InputsInitialization()
        {
            Jump = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("jumpKey", "Space"));
            Left = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("leftKey", "A"));
            Right = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("rightKey", "D"));
        }

        private void OnGUI()
        {

            keyEvent = Event.current;
            if (!keyEvent.isKey || !waitingForKey)
            {
                return;
            }

            newKey = keyEvent.keyCode;
            waitingForKey = false;

        }

        public void StartAssignment(string keyName)
        {
            if (!waitingForKey)
            {
                StartCoroutine(AssignKey(keyName));
            }
        }

        public void SendText(Text text)
        {
            buttonText = text;
        }

        private IEnumerator WaitForKey()
        {
            while (!keyEvent.isKey)
            {
                yield return null;
            }
        }

        public IEnumerator AssignKey(string keyName)
        {
            waitingForKey = true;

            yield return WaitForKey(); 

            switch (keyName)
            {

                case "left":

                    InputManager.Input.Left = newKey;
                    buttonText.text = InputManager.Input.Left.ToString();
                    PlayerPrefs.SetString("leftKey", InputManager.Input.Left.ToString());
                    break;

                case "right":

                    InputManager.Input.Right = newKey;
                    buttonText.text = InputManager.Input.Right.ToString();
                    PlayerPrefs.SetString("rightKey", InputManager.Input.Right.ToString());
                    break;

                case "jump":

                    InputManager.Input.Jump = newKey;
                    buttonText.text = InputManager.Input.Jump.ToString();
                    PlayerPrefs.SetString("jumpKey", InputManager.Input.Jump.ToString());

                    break;
            }
            yield return null;
        }


    }
}
