using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class MenuScript : MonoBehaviour
    {
        private Transform menuPanel;
        private Event keyEvent;
        private Text buttonText;
        private KeyCode newKey;
        private bool waitingForKey;

        private void Start()
        {
            menuPanel = transform.Find("InputControler");
            waitingForKey = false;

            for (var i = 0; i < menuPanel.childCount; i++)
            {
                if (menuPanel.GetChild(i).name == "LeftKey")
                {
                    menuPanel.GetChild(i).GetComponentInChildren<Text>().text = GameManager.GameManagerInstance.left.ToString();
                }
                else if (menuPanel.GetChild(i).name == "RightKey")
                {
                    menuPanel.GetChild(i).GetComponentInChildren<Text>().text = GameManager.GameManagerInstance.right.ToString();
                }
                else if (menuPanel.GetChild(i).name == "JumpKey")
                {
                    menuPanel.GetChild(i).GetComponentInChildren<Text>().text = GameManager.GameManagerInstance.jump.ToString();
                }
            }
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

            if (keyName == "left")
            {
                GameManager.GameManagerInstance.left = newKey;
                buttonText.text = GameManager.GameManagerInstance.left.ToString();
                PlayerPrefs.SetString("leftKey", GameManager.GameManagerInstance.left.ToString());
            }
            else if (keyName == "right")
            {
                GameManager.GameManagerInstance.right = newKey;
                buttonText.text = GameManager.GameManagerInstance.right.ToString();
                PlayerPrefs.SetString("rightKey", GameManager.GameManagerInstance.right.ToString());
            }
            else if (keyName == "jump")
            {
                GameManager.GameManagerInstance.jump = newKey;
                buttonText.text = GameManager.GameManagerInstance.jump.ToString();
                PlayerPrefs.SetString("jumpKey", GameManager.GameManagerInstance.jump.ToString());
            }
            yield return null;
        }
    }
}