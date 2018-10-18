using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.UI
{
    public class PauseMenu : MonoBehaviour
    {
        public GameObject UIPause;

        private bool paused = false;

        private void Start()
        {
            UIPause.SetActive(false);
        }

        private void Update()
        {
            //if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Delete))
            //{
            //    paused = !paused;
            //}

            UIPause.SetActive(paused);
        }

        public void Resume()
        {
            paused = false;
        }

        public void MainMenu()
        {
            DestroyOnLoad();
            SceneManager.LoadScene(0);
        }

        public void Quit()
        {
            Application.Quit();
        }

        private void DestroyOnLoad()
        {
            if (SceneManager.GetSceneByName("Multiplayer") == SceneManager.GetActiveScene())
            {
                Destroy(GameObject.Find("NetworkManager"));
            }
        }
    }
}
